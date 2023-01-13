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
            _editor.TextChanged += _editor_TextChanged;
            _editor.TextArea.TextEntered += TextArea_TextEntered;
            _editor.TextArea.TextEntering += TextArea_TextEntering;
        }

        protected void DetachEvents()
        {
            _editor.TextChanged -= _editor_TextChanged;
            _editor.TextArea.TextEntered -= TextArea_TextEntered;
            _editor.TextArea.TextEntering -= TextArea_TextEntering;
        }

        private void _editor_TextChanged(object? sender, EventArgs e)
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
                    OnTextEntered?.Invoke(sender, new TextEnterEventArgs()
                    {
                        DateTime = DateTimeProvider.Now,
                        DocumentLine = lineDetails.Line,
                        Offset = lineDetails.Offset.Value,
                        Length = lineDetails.Length.Value,
                        LineNumber = lineDetails.LineNumber.Value,
                        LineContent= lineDetails.LineContent
                    });
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
                    OnTextEntered?.Invoke(sender, new TextEnterEventArgs()
                    {
                        DateTime = DateTimeProvider.Now,
                        DocumentLine = lineDetails.Line,
                        Offset = lineDetails.Offset.Value,
                        Length = lineDetails.Length.Value,
                        LineNumber = lineDetails.LineNumber.Value,
                        LineContent = lineDetails.LineContent
                    });
                }
            }
        }

        protected (DocumentLine Line, int? LineNumber, int? Offset, int? Length, string LineContent) GetCurrentLineDetails()
        {
            int editorOffset = _editor.CaretOffset;
            DocumentLine line = _editor.Document.GetLineByOffset(editorOffset);
            if (line.IsDeleted)
            {
                var lineNumber = line.LineNumber;
                var offset = line.Offset;
                var length = line.Length;
                var lineString = _editor.Document.GetText(offset, length);
                return (line, lineNumber, offset, length, lineString);
            }
            else
            {
                return (line, null, null, null, null);
            }
        }

        public void TearDownEditor(TextEditor editor)
        {
            DetachEvents();
            _editor = null;
        }
    }
}
