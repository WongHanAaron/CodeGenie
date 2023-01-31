using CodeGenie.Ui.Wpf.Controls.CodeEditor.Models.Visuals;
using CodeGenie.Ui.Wpf.Controls.Shared.Contracts;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace CodeGenie.Ui.Wpf.Controls.CodeEditor.Services.TextMarkers
{
    /// <summary> The TextMarkerService that is used to  </summary>
    public interface ITextMarkerService : IBackgroundRenderer, IVisualLineTransformer, IInjectsEditor
    {
        /// <summary> Gets the text markers at this offset location </summary>
        IEnumerable<TextMarker> GetMarkersAtOffset(int offset);

        /// <summary> Clears all the current set of markers </summary>
        void Clear();

        /// <summary> Create a new text marker at that offset with a specific length with a specific message </summary>
        void Create(int offset, int length, string message);
    }

    public class TextMarkerService : ITextMarkerService
    {
        private TextEditor? _textEditor;
        private TextSegmentCollection<TextMarker>? _markers;
        protected ILogger<TextMarkerService> Logger;

        public void InjectEditor(TextEditor textEditor)
        {
            _textEditor = textEditor;
            _markers = new TextSegmentCollection<TextMarker>(textEditor.Document);
            TextView textView = textEditor.TextArea.TextView;
            textView.BackgroundRenderers.Add(this);
            textView.LineTransformers.Add(this);
        }

        public void TearDownEditor(TextEditor textEditor)
        {
            TextView textView = textEditor.TextArea.TextView;
            textView.BackgroundRenderers.Remove(this);
            textView.LineTransformers.Remove(this);
            _textEditor = null;
            _markers = null;
        }

        public TextMarkerService(ILogger<TextMarkerService> logger)
        {
            Logger = logger;
        }

        public void Draw(TextView textView, DrawingContext drawingContext)
        {
            if (_markers == null || !textView.VisualLinesValid)
            {
                return;
            }
            var visualLines = textView.VisualLines;
            if (visualLines.Count == 0)
            {
                return;
            }
            int viewStart = visualLines.First().FirstDocumentLine.Offset;
            int viewEnd = visualLines.Last().LastDocumentLine.EndOffset;
            foreach (TextMarker marker in _markers.FindOverlappingSegments(viewStart, viewEnd - viewStart))
            {
                if (marker.BackgroundColor != null)
                {
                    var geoBuilder = new BackgroundGeometryBuilder { AlignToWholePixels = true, CornerRadius = 3 };
                    geoBuilder.AddSegment(textView, marker);
                    Geometry geometry = geoBuilder.CreateGeometry();
                    if (geometry != null)
                    {
                        Color color = marker.BackgroundColor.Value;
                        var brush = new SolidColorBrush(color);
                        brush.Freeze();
                        drawingContext.DrawGeometry(brush, null, geometry);
                    }
                }
                foreach (Rect r in BackgroundGeometryBuilder.GetRectsForSegment(textView, marker))
                {
                    Point startPoint = r.BottomLeft;
                    Point endPoint = r.BottomRight;

                    var usedPen = new Pen(new SolidColorBrush(marker.MarkerColor), 1);
                    usedPen.Freeze();
                    const double offset = 2.5;

                    int count = Math.Max((int)((endPoint.X - startPoint.X) / offset) + 1, 4);

                    var geometry = new StreamGeometry();

                    using (StreamGeometryContext ctx = geometry.Open())
                    {
                        ctx.BeginFigure(startPoint, false, false);
                        ctx.PolyLineTo(CreatePoints(startPoint, endPoint, offset, count).ToArray(), true, false);
                    }

                    geometry.Freeze();

                    drawingContext.DrawGeometry(Brushes.Transparent, usedPen, geometry);
                    break;
                }
            }
        }

        public KnownLayer Layer
        {
            get { return KnownLayer.Selection; }
        }

        public void Transform(ITextRunConstructionContext context, IList<VisualLineElement> elements)
        { }

        private IEnumerable<Point> CreatePoints(Point start, Point end, double offset, int count)
        {
            for (int i = 0; i < count; i++)
            {
                yield return new Point(start.X + (i * offset), start.Y - ((i + 1) % 2 == 0 ? offset : 0));
            }
        }

        public void Clear()
        {
            if (_markers == null) return;
            foreach (TextMarker m in _markers)
            {
                Remove(m);
            }
        }

        private void Remove(TextMarker marker)
        {
            if (_markers == null) return;
            if (_markers.Remove(marker))
            {
                Redraw(marker);
            }
        }

        private void Redraw(ISegment segment)
        {
            _textEditor?.Dispatcher.Invoke(() =>
            {
                _textEditor.TextArea.TextView.Redraw(segment);
            });
        }

        public void Create(int offset, int length, string message)
        {
            var m = new TextMarker(offset, length);
            _markers.Add(m);
            m.MarkerColor = Colors.Red;
            m.ToolTip = message;
            Redraw(m);
        }

        public IEnumerable<TextMarker> GetMarkersAtOffset(int offset)
        {
            return _markers == null ? Enumerable.Empty<TextMarker>() : _markers.FindSegmentsContaining(offset);
        }

    }
}
