using CodeGenie.Ui.Wpf.Controls.CodeEditor.Models.AutoCompletions.Syntax;
using CodeGenie.Ui.Wpf.Controls.CodeEditor.Models.Events;
using ICSharpCode.AvalonEdit.CodeCompletion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenie.Ui.Wpf.Controls.CodeEditor.Services.AutoComplete
{
    /// <summary> Component that suggests completion data for the code editor </summary>
    public interface ICompletionDataSuggester
    {
        /// <summary> Get suggestions based on the text entered args </summary>
        IEnumerable<ICompletionData> GetSuggestions(TextEnterEventArgs eventArgs);
    }

    public class CompletionDataSuggester : ICompletionDataSuggester
    {
        public IEnumerable<ICompletionData> GetSuggestions(TextEnterEventArgs eventArgs)
        {
            return new List<ICompletionData>() { new ClassPartCompletion() };
        }
    }
}
