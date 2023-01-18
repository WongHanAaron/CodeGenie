using CodeGenie.Ui.Wpf.Controls.CodeEditor.Models.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenie.Ui.Wpf.Controls.CodeEditor.Models.AutoComplete.Suggestions
{
    public class SimpleTextSuggestion : SuggestionBase
    {   
        public SimpleTextSuggestion(string caption, string description, TextEnterEventArgs args, string replacementText = null, double priority = 1) : base(args)
        {
            _text = caption;
            _description = description;
            _priority = priority;
            _replacementText = replacementText ?? _text;
        }

        private string _replacementText;

        private string _text;
        public override string Text => _text;

        private string _description;
        public override object Description => _description;

        private double _priority;

        public override double Priority => _priority;

        public override string GetReplacementText(EventArgs insertionRequestEventArgs)
        {
            return _replacementText;
        }
    }
}
