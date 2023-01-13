using CodeGenie.Ui.Wpf.Controls.CodeEditor.Contracts;
using CodeGenie.Ui.Wpf.Controls.CodeEditor.Models.Events;
using CodeGenie.Ui.Wpf.Controls.Shared.Contracts;
using CodeGenie.Ui.Wpf.Controls.Shared.Services;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Document;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CodeGenie.Ui.Wpf.Controls.CodeEditor.Services.EditorTracking
{
    public class TextUpdateListener : ITextUpdateListener
    {
        protected TextEditor _editor;
        protected readonly ILogger<TextUpdateListener> Logger;
        protected readonly IDateTimeProvider DateTimeProvider;
        public EventHandler<TextUpdateEventArgs> OnTextUpdated { get; set; }
        public EventHandler<TextEnterEventArgs> OnTextEntered { get; set; }
        public EventHandler<TextEnterEventArgs> OnTextEntering { get; set; }


        public TextUpdateListener(ILogger<TextUpdateListener> logger, 
                                  IDateTimeProvider dateTimeProvider)
        {
            Logger = logger;
            DateTimeProvider = dateTimeProvider;
        }

        public void InjectEditor(TextEditor editor)
        {
            _editor = editor;
            AttachEvents();
        }

        protected void AttachEvents()
        {
            _editor.TextChanged += Text_TextChanged;
            _editor.TextArea.TextEntered += TextArea_TextEntered;
            _editor.TextArea.TextEntering += TextArea_TextEntering;
        }

        protected void DetachEvents()
        {
            _editor.TextChanged -= Text_TextChanged;
            _editor.TextArea.TextEntered -= TextArea_TextEntered;
            _editor.TextArea.TextEntering -= TextArea_TextEntering;
        }

        private void Text_TextChanged(object? sender, EventArgs e)
        {
            if (OnTextUpdated != null)
            {
                OnTextUpdated?.Invoke(sender, new TextUpdateEventArgs()
                {
                    Text = _editor.Text,
                    DateTime = DateTimeProvider.Now
                });
            }
        }
        private void TextArea_TextEntered(object sender, TextCompositionEventArgs e)
        {
            if (OnTextEntered != null)
            {
                var lineDetails = GetCurrentLineDetails();
                if (!lineDetails.Line.IsDeleted)
                {
                    OnTextEntered?.Invoke(sender, CreateTextEnterArgs(lineDetails, e));
                }
            }
        }

        private void TextArea_TextEntering(object sender, TextCompositionEventArgs e)
        {
            if (OnTextEntering != null)
            {
                var lineDetails = GetCurrentLineDetails();
                if (!lineDetails.Line.IsDeleted)
                {
                    OnTextEntering?.Invoke(sender, CreateTextEnterArgs(lineDetails, e));
                }
            }
        }

        protected TextEnterEventArgs CreateTextEnterArgs(CurrentLineDetails lineDetails, TextCompositionEventArgs e)
        {
            if (lineDetails.Offset.HasValue &&
                lineDetails.Length.HasValue &&
                lineDetails.LineNumber.HasValue)
            {
                return new TextEnterEventArgs()
                {
                    DateTime = DateTimeProvider.Now,
                    Text = e.Text,
                    DocumentLine = lineDetails.Line,
                    Offset = lineDetails.Offset.Value,
                    Length = lineDetails.Length.Value,
                    LineNumber = lineDetails.LineNumber.Value,
                    LineContent = lineDetails.LineContent
                };
            }
            else
            {
                return new TextEnterEventArgs()
                {
                    DateTime = DateTimeProvider.Now,
                    Text = e.Text,
                    LineContent = lineDetails.LineContent
                };
            }
        }

        protected CurrentLineDetails GetCurrentLineDetails()
        {
            int editorOffset = _editor.CaretOffset;
            DocumentLine line = _editor.Document.GetLineByOffset(editorOffset);
            if (!line.IsDeleted)
            {
                var lineNumber = line.LineNumber;
                var offset = line.Offset;
                var length = line.Length;
                var lineString = _editor.Document.GetText(offset, length);
                return new CurrentLineDetails
                {
                    Line = line,
                    LineNumber = lineNumber,
                    Offset = offset,
                    Length = length,
                    LineContent = lineString
                };
            }
            else
            {
                return new CurrentLineDetails()
                {
                    Line = line
                };
            }
        }

        public sealed class CurrentLineDetails
        {
            public DocumentLine Line { get; set; }
            public int? LineNumber { get; set; }
            public int? Offset { get; set; }
            public int? Length { get; set; }
            public string LineContent { get; set; }
        }

        public void TearDownEditor(TextEditor editor)
        {
            DetachEvents();
            _editor = null;
        }
    }
}
