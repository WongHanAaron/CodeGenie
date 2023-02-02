using CodeGenie.Ui.Wpf.Controls.CodeEditor.Contracts;
using CodeGenie.Ui.Wpf.Controls.CodeEditor.Models.Events;
using CodeGenie.Ui.Wpf.Controls.CodeEditor.Services.AutoComplete;
using CodeGenie.Ui.Wpf.Controls.CodeEditor.Services.AutoComplete.Suggestions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenie.Ui.Wpf.Controls.Tests.CodeEditor.AutoComplete
{
    [TestFixture]
    public class CompletionDataSuggester_Tests : CodeEditorTestBase
    {
        protected ICompletionDataSuggester? Suggester;
        protected Mock<ITextUpdateListener> MockTextUpdateListener;
        protected ITextUpdateListener TextUpdateListener;

        [SetUp]
        public void SetUp()
        {
            MockTextUpdateListener = new Mock<ITextUpdateListener>();
            TextUpdateListener = MockTextUpdateListener.Object;

            ServiceProvider = BuildServiceProvider(m =>
            {
                m.OverrideService<ITextUpdateListener>(TextUpdateListener);
            });

            Suggester = ServiceProvider.GetService<ICompletionDataSuggester>() as ICompletionDataSuggester;
            Assert.That(Suggester, Is.Not.Null);
        }

        [TestCase(1, 2, "+ ", typeof(TooltipSuggestion))]
        [TestCase(1, 0, "", typeof(SimpleTextSuggestion), typeof(SimpleTextSuggestion), typeof(SimpleTextSuggestion))]
        public void Script_Suggests_Correct_Types(int lineNumber, int columnNumber, string fullContents, params Type[] expectedTypes)
        {
            // SETUP
            if (Suggester == null) throw new ArgumentException($"The '{nameof(Suggester)}' is null at test start");

            var args = CreateEventArgs(fullContents, lineNumber, columnNumber);
            MockTextUpdateListener.Setup(m => m.CurrentText).Returns(fullContents);

            var suggestions = Suggester.GetSuggestions(args);

            Assert.That(suggestions.Count(), Is.EqualTo(expectedTypes.Count()));

            var groupedSuggestion = suggestions.GroupBy(g => g.GetType());

            foreach (var expectedSuggestion in expectedTypes.GroupBy(t => t))
            {
                var matchingGroup = groupedSuggestion.FirstOrDefault(g => g.Key.Equals(expectedSuggestion.Key));

                Assert.That(matchingGroup, Is.Not.Null, $"Unable to find suggestion of type '{expectedSuggestion.Key}'");

                Assert.That(matchingGroup.Count(), Is.EqualTo(expectedSuggestion.Count()));               
            }
        }

        protected List<Type> GetExpectedSuggestionTypes(string expectedSuggestionsStringCsv)
        {
            var typeNames = expectedSuggestionsStringCsv.Split(", ".ToArray(), StringSplitOptions.RemoveEmptyEntries);
            var nullableTypes = typeNames.Where(t => Type.GetType(t) != null).Select(t => Type.GetType(t)).Select(t => t).ToList();

            var returned = new List<Type>();
            nullableTypes.ForEach(t => 
            {
                if (t != null) returned.Add(t);
            });

            return returned;
        }

        protected TextEnterEventArgs CreateEventArgs(string fullContents, int lineNumber, int columnNumber)
        {
            var lines = fullContents.Split("\n");
            var lineContent = lines.ElementAtOrDefault(lineNumber - 1);
            // if (string.IsNullOrEmpty(lineContent)) throw new ArgumentException($"No contents on line");

            var lineOffset = fullContents.IndexOf(lineContent);

            return new TextEnterEventArgs()
            {
                ColumnNumber = columnNumber,
                LineNumber = lineNumber,
                DateTime = DateTime.Now,
                LineContent = lineContent,
                Length = lineContent?.Length ?? -1,
                Text = fullContents,
                Offset = lineOffset + columnNumber
            };
        }
    }
}
