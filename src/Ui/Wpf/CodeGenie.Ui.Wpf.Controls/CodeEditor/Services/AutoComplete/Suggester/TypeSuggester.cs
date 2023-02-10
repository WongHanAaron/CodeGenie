using CodeGenie.Core.Models.ComponentDefinitions.Definitions;
using CodeGenie.Core.Models.ComponentDefinitions.Syntax;
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
    public class TypeSuggester : SyntaxSuggesterBase
    {
        protected readonly IComponentRepository Repository;
        public TypeSuggester(IComponentRepository repository) :
                            base(SyntaxValidityOption.Invalid, 
                                 SyntaxDescriptor.BeforeAttributeTypeDefinition)
        {
            Repository = repository;
        }

        protected override void CollectOtherSuggestions(SyntaxDescription description, TextEnterEventArgs textEnterArgs, List<ICompletionData> toBeReturned)
        {
            CollectPrimitiveTypes(description, textEnterArgs, toBeReturned);

            if (Repository.LastValidComponents?.Components == null) return;
            if (!Repository.LastValidComponents.Components.Any()) return;

            var shouldPrefixWhitespace = !textEnterArgs.LineContent.EndsWith(' ');

            foreach (var component in Repository.LastValidComponents.Components)
            {
                if (component.Scope != Scope.Public) continue;
                var componentType = component.IsInterface ? "interface" : "class";
                var purposeDescription = string.IsNullOrWhiteSpace(component.Purpose) ? $"A {componentType} of name '{component.Name}'" : $"A {componentType} of name '{component.Name}'. {component.Purpose}";
                var value = component.Name;
                if (shouldPrefixWhitespace) value = " " + value;
                toBeReturned.Add(new SimpleTextSuggestion(component.Name, purposeDescription, textEnterArgs, value));
            }
        }

        public const string String = "string";
        public const string Integer = "int";
        public const string Float = "float";
        public const string Double = "double";
        public const string DateTime = "datetime";


        protected void CollectPrimitiveTypes(SyntaxDescription description, TextEnterEventArgs textEnterArgs, List<ICompletionData> toBeReturned)
        {
            toBeReturned.Add(new SimpleTextSuggestion(String, "Primitive string", textEnterArgs)); 
            toBeReturned.Add(new SimpleTextSuggestion(Integer, "Primitive integer", textEnterArgs)); 
            toBeReturned.Add(new SimpleTextSuggestion(Float, "Primitive floating point number", textEnterArgs));
            toBeReturned.Add(new SimpleTextSuggestion(Double, "Primitive double precision floating point number", textEnterArgs));
            toBeReturned.Add(new SimpleTextSuggestion(DateTime, "Primitive date time object", textEnterArgs));
        }
    }
}
