using CodeGenie.Core.Models.ComponentDefinitions.Syntax;
using CodeGenie.Ui.Wpf.Controls.CodeEditor.Models.Events;
using CodeGenie.Ui.Wpf.Controls.CodeEditor.Services.AutoComplete.Suggestions;
using ICSharpCode.AvalonEdit.CodeCompletion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenie.Ui.Wpf.Controls.CodeEditor.Services.AutoComplete.Suggester
{
    public class RelationshipTypeSuggester : SyntaxSuggesterBase
    {
        public const string Depends = "depends";
        public const string Aggregates = "aggregates";
        public const string Composes = "composes";
        public const string Realizes = "realizes";
        public const string Specializes = "specializes";

        public RelationshipTypeSuggester() : 
                    base(SyntaxValidityOption.Invalid, SyntaxDescriptor.WithinRelationshipsDetails)
        {
        }

        protected override void CollectOtherSuggestions(SyntaxDescription description, TextEnterEventArgs textEnterArgs, List<ICompletionData> toBeReturned)
        {
            toBeReturned.Add(new SimpleTextSuggestion(Depends, "Dependency relationship", textEnterArgs));
            toBeReturned.Add(new SimpleTextSuggestion(Aggregates, "Aggregation relationship", textEnterArgs));
            toBeReturned.Add(new SimpleTextSuggestion(Composes, "Composition relationship", textEnterArgs));
            toBeReturned.Add(new SimpleTextSuggestion(Realizes, "Realization relationship", textEnterArgs));
            toBeReturned.Add(new SimpleTextSuggestion(Specializes, "Specialization relationship", textEnterArgs));
        }
    }
}
