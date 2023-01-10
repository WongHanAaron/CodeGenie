using CodeGenie.Ui.Wpf.Controls.CodeEditor.Contracts;
using CodeGenie.Ui.Wpf.Controls.CodeEditor.Models.Events;
using CodeGenie.Ui.Wpf.Controls.Shared.Services;
using ICSharpCode.AvalonEdit;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenie.Ui.Wpf.Controls.CodeEditor.Services.Setup
{
    /// <summary>
    /// Configures the AvalonEdit TextBox for the right configurations and components 
    /// to handle CodeGenie class format
    /// </summary>
    public interface IAvalonEditConfigurer
    {
        void Configure(TextEditor textEditor);
        void TearDown(TextEditor textEditor);
    }

    public class AvalonEditConfigurer : IAvalonEditConfigurer
    {
        protected readonly ILogger<AvalonEditConfigurer> Logger;
        protected readonly IDateTimeProvider DateTimeProvider;
        protected readonly IHighlightDefinitionAccessor HighlightSchemaAccessor;
        protected readonly ITextUpdateListener TextUpdateListener;

        protected TextEditor LastConfiguredEditor { get; set; }
        public AvalonEditConfigurer(ILogger<AvalonEditConfigurer> logger,
                                    IDateTimeProvider dateTimeProvider,
                                    IHighlightDefinitionAccessor highlightSchemaAccessor,
                                    ITextUpdateListener textUpdateListener)
        {
            Logger = logger;
            DateTimeProvider = dateTimeProvider;
            HighlightSchemaAccessor = highlightSchemaAccessor;
            TextUpdateListener = textUpdateListener;
        }

        public void Configure(TextEditor textEditor)
        {
            LastConfiguredEditor = textEditor;
            ConfigureSyntaxHighlighting(textEditor);
            ConfigureErrorHighlighting(textEditor);
            ConfigureTextUpdateListener(textEditor);
        }

        protected void ConfigureSyntaxHighlighting(TextEditor textEditor)
        {
            textEditor.SyntaxHighlighting = HighlightSchemaAccessor.GetHighlightingDefinition();
        }

        protected void ConfigureErrorHighlighting(TextEditor textEditor)
        {

        }

        protected void ConfigureTextUpdateListener(TextEditor textEditor)
        {
            textEditor.TextChanged += TextEditor_TextChanged;
        }

        private void TextEditor_TextChanged(object? sender, EventArgs e)
        {
            TextUpdateListener.TextWasUpdated(sender, new TextUpdateEvent()
            {
                Text = LastConfiguredEditor.Text,
                DateTime = DateTimeProvider.Now
            });
        }

        public void TearDown(TextEditor textEditor)
        {
            textEditor.TextChanged -= TextEditor_TextChanged;
        }
    }
}
