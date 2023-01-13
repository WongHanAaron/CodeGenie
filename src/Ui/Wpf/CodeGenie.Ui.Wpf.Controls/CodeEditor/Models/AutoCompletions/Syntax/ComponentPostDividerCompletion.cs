using CodeGenie.Ui.Wpf.Controls.CodeEditor.Models.Events;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CodeGenie.Ui.Wpf.Controls.CodeEditor.Models.AutoCompletions.Syntax
{
    public class ComponentPostDividerCompletion : AutoCompletionBase
    {
        public ComponentPostDividerCompletion(string componentType, TextEnterEventArgs eventArgs) : base(eventArgs)
        {
            _text = componentType;
        }

        private string _text;
        public override string Text => _text;

        public override object Description => $"Complete a component as an {_text}";

        public override double Priority => 1;

        public override string GetReplacementText(EventArgs insertionRequestEventArgs)
        {
            if (Regex.IsMatch(EventArguments.LineContent, @"\s"))
            {
                return Text;
            }
            else
            {
                return " " + Text;
            }
        }
    }
}
