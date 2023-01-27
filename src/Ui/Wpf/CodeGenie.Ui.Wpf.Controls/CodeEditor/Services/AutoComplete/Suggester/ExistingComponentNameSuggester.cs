using CodeGenie.Core.Models.ComponentDefinitions.Definitions;
using CodeGenie.Core.Models.ComponentDefinitions.State;
using CodeGenie.Ui.Wpf.Controls.CodeEditor.Models.Events;
using CodeGenie.Ui.Wpf.Controls.CodeEditor.Services.AutoComplete.Suggestions;
using CodeGenie.Ui.Wpf.Controls.CodeEditor.Services.Compiling;
using ICSharpCode.AvalonEdit.CodeCompletion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenie.Ui.Wpf.Controls.CodeEditor.Services.AutoComplete.Suggester
{
    public class ExistingComponentNameSuggester : SyntaxSuggesterBase
    {
        protected readonly IComponentRepository Repository;
        public ExistingComponentNameSuggester(IComponentRepository repository) :
                                        base(SyntaxDescriptor.BeforeRelatedComponentNameDefinition)
        {
            Repository = repository;
        }

        protected override void CollectOtherSuggestions(SyntaxDescriptor descriptor, TextEnterEventArgs textEnterArgs, List<ICompletionData> toBeReturned)
        {
            if (Repository.LastValidComponents?.Components == null) return;
            if (!Repository.LastValidComponents.Components.Any()) return;

            var shouldPrefixWhitespace = !textEnterArgs.LineContent.EndsWith(' ');

            foreach (var component in Repository.LastValidComponents.Components)
            {
                if (component.Scope != Scope.Public) continue;
                var componentType = component.IsInterface ? "interface" : "class";
                var description = string.IsNullOrWhiteSpace(component.Purpose) ? $"A {componentType} of name '{component.Name}'" : $"A {componentType} of name '{component.Name}'. {component.Purpose}";
                var value = component.Name;
                if (shouldPrefixWhitespace) value = " " + value;
                toBeReturned.Add(new SimpleTextSuggestion(component.Name, description, textEnterArgs, value));
            }
        }
    }
}
