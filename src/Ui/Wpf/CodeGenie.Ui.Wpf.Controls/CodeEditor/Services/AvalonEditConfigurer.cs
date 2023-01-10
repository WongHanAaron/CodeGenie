using ICSharpCode.AvalonEdit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenie.Ui.Wpf.Controls.CodeEditor.Services
{
    /// <summary>
    /// Configures the AvalonEdit TextBox for the right configurations and components 
    /// to handle CodeGenie class format
    /// </summary>
    public interface IAvalonEditConfigurer
    {
        void Configure(TextEditor textEditor);
    }

    public class AvalonEditConfigurer : IAvalonEditConfigurer
    {
        public void Configure(TextEditor textEditor)
        {
            ConfigureSyntaxHighlighting(textEditor);
            ConfigureErrorHighlighting(textEditor);
        }

        protected void ConfigureSyntaxHighlighting(TextEditor textEditor)
        {
            textEditor.SyntaxHighlighting = new HighlightingDefinitionAccessor().GetHighlightingDefinition();
        }

        protected void ConfigureErrorHighlighting(TextEditor textEditor)
        {

        }
    }
}
