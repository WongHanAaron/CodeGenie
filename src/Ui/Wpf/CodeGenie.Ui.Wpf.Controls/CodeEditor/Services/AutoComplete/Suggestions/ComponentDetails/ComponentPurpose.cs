﻿using CodeGenie.Ui.Wpf.Controls.CodeEditor.Models.Events;
using CodeGenie.Ui.Wpf.Controls.CodeEditor.Services.AutoComplete.Suggestions;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenie.Ui.Wpf.Controls.CodeEditor.Services.AutoComplete.Suggestions.ComponentDetails
{
    public class ComponentPurpose : SuggestionBase
    {
        public const string TextValue = "{ purpose : \"\"}";

        bool _includeExternalBraces;
        public ComponentPurpose(TextEnterEventArgs eventArgs, bool includeExternalBraces) : base(eventArgs)
        {
            _includeExternalBraces = includeExternalBraces;
        }

        public override string Text => TextValue;

        public override object Description => "Add a component purpose";

        public override double Priority => 1;

        public override string GetReplacementText(EventArgs insertionRequestEventArgs)
        {
            var builder = new StringBuilder();
            if (_includeExternalBraces) builder.Append("\n{");
            builder.Append("\n\tpurpose : \"\"");
            if (_includeExternalBraces) builder.Append("\n}");

            CaretLineNumberPlacement = EventArguments.LineNumber + 1;
            if (_includeExternalBraces) CaretLineNumberPlacement += 1;
            CaretColumnPlacement = 13;

            return builder.ToString();
        }
    }
}
