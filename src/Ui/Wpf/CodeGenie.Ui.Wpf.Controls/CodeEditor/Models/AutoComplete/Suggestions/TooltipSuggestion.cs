using CodeGenie.Ui.Wpf.Controls.CodeEditor.Models.Events;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CodeGenie.Ui.Wpf.Controls.CodeEditor.Models.AutoComplete.Suggestions
{
    public class TooltipSuggestion : ICompletionData
    {
        public TooltipSuggestion(string text, string description)
        {
            _text = text;
            _description = description;
        }

        private string _text;
        public string Text => _text;

        private string _description;
        public object Description => _description;

        public double Priority => int.MaxValue;

        public ImageSource Image => null;

        public object Content => Text;

        public void Complete(TextArea textArea, ISegment completionSegment, EventArgs insertionRequestEventArgs) { }
    }
}
