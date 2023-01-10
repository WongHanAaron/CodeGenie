using CodeGenie.Core.Models.ComponentDefinitions.ParsedDefinitions;
using CodeGenie.Ui.Wpf.Controls.CodeEditor.Contracts;
using CodeGenie.Ui.Wpf.Controls.CodeEditor.Services.EditorTracking;
using CodeGenie.Ui.Wpf.Controls.Shared.Contracts;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Document;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CodeGenie.Ui.Wpf.Controls.CodeEditor.Services.TextMarkers
{
    public interface IComponentDefinitionMarker : IInjectsEditor
    {
        
    }

    public class ComponentDefinitionErrorMarker : IComponentDefinitionMarker
    {
        protected readonly ILogger<ComponentDefinitionErrorMarker> Logger;
        protected readonly ITextMarkerService TextMarkerService;
        protected readonly ITextViewEventListener TextViewEventListener;
        protected readonly IComponentDefinitionProvider ComponentDefinitionProvider;
        protected TextEditor TextEditor;
        protected ToolTip ToolTip;

        public ComponentDefinitionErrorMarker(ILogger<ComponentDefinitionErrorMarker> logger,
                                              ITextMarkerService textMarkerService,
                                              ITextViewEventListener textViewEventListener,
                                              IComponentDefinitionProvider componentDefinitionProvider)
        {
            Logger = logger;
            TextMarkerService = textMarkerService;
            TextViewEventListener = textViewEventListener;
            ComponentDefinitionProvider = componentDefinitionProvider;
            ComponentDefinitionProvider.OnComponentDefinitionsDefined += (o, e) => MarkErrors(o, e);
            TextViewEventListener.MouseHover += MouseHover;
            TextViewEventListener.MouseHoverStopped += TextEditorMouseHoverStopped;
            TextViewEventListener.VisualLinesChanged += VisualLinesChanged;
        }

        private void MouseHover(object sender, MouseEventArgs e)
        {
            var pos = TextEditor.TextArea.TextView.GetPositionFloor(e.GetPosition(TextEditor.TextArea.TextView) + TextEditor.TextArea.TextView.ScrollOffset);
            bool inDocument = pos.HasValue;
            if (inDocument)
            {
                TextLocation logicalPosition = pos.Value.Location;
                int offset = TextEditor.Document.GetOffset(logicalPosition);

                var markersAtOffset = TextMarkerService.GetMarkersAtOffset(offset);
                var markerWithToolTip = markersAtOffset.FirstOrDefault(marker => marker.ToolTip != null);

                if (markerWithToolTip != null)
                {
                    if (ToolTip == null)
                    {
                        ToolTip = new ToolTip();
                        ToolTip.Closed += ToolTipClosed;
                        ToolTip.PlacementTarget = TextEditor;
                        ToolTip.Content = new TextBlock
                        {
                            Text = markerWithToolTip.ToolTip,
                            TextWrapping = TextWrapping.Wrap
                        };
                        ToolTip.IsOpen = true;
                        e.Handled = true;
                    }
                }
            }
        }

        void ToolTipClosed(object sender, RoutedEventArgs e)
        {
            ToolTip = null;
        }

        void TextEditorMouseHoverStopped(object sender, MouseEventArgs e)
        {
            if (ToolTip != null)
            {
                ToolTip.IsOpen = false;
                e.Handled = true;
            }
        }

        private void VisualLinesChanged(object sender, EventArgs e)
        {
            if (ToolTip != null)
            {
                ToolTip.IsOpen = false;
            }
        }

        private void MarkErrors(object sender, ParsingResult parsingResult)
        {
            try
            {
                TextMarkerService.Clear();

                foreach (var error in parsingResult.Errors)
                {
                    DisplayValidationError(error.Exception?.Message ?? "Unknown error", error.ColumnIndex, error.LineNumber);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError($"An exception occurred in {nameof(MarkErrors)} for new components. {ex}");
            }
        }

        private void DisplayValidationError(string message, int linePosition, int lineNumber)
        {
            TextEditor.Dispatcher.BeginInvoke(() => 
            {
                if (lineNumber >= 1 && lineNumber <= TextEditor.Document.LineCount)
                {
                    int offset = TextEditor.Document.GetOffset(new TextLocation(lineNumber, linePosition));
                    int endOffset = TextUtilities.GetNextCaretPosition(TextEditor.Document, offset, System.Windows.Documents.LogicalDirection.Forward, CaretPositioningMode.WordBorderOrSymbol);
                    if (endOffset < 0)
                    {
                        endOffset = TextEditor.Document.TextLength;
                    }
                    int length = endOffset - offset;

                    if (length < 2)
                    {
                        length = Math.Min(2, TextEditor.Document.TextLength - offset);
                    }

                    TextMarkerService.Create(offset, length, message);
                }
            });
        }

        public void InjectEditor(TextEditor editor)
        {
            TextEditor = editor;
        }

        public void TearDownEditor(TextEditor editor)
        {
            TextEditor = null;
        }
    }
}
