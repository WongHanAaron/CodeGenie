using CodeGenie.Ui.Wpf.Controls.CodeEditor.Contracts;
using CodeGenie.Ui.Wpf.Controls.CodeEditor.Models.Events;
using CodeGenie.Ui.Wpf.Controls.CodeEditor.Services.EditorTracking;
using CodeGenie.Ui.Wpf.Controls.CodeEditor.Services.TextMarkers;
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
        protected readonly ITextMarkerService TextMarkerService;
        protected readonly ITextViewEventListener TextViewEventListener;
        protected readonly IComponentDefinitionMarker ComponentDefinitionMarker;

        protected TextEditor LastConfiguredEditor { get; set; }
        public AvalonEditConfigurer(ILogger<AvalonEditConfigurer> logger,
                                    IDateTimeProvider dateTimeProvider,
                                    IHighlightDefinitionAccessor highlightSchemaAccessor,
                                    ITextUpdateListener textUpdateListener,
                                    ITextMarkerService textMarkerService,
                                    ITextViewEventListener textViewEventListener,
                                    IComponentDefinitionMarker componentDefinitionMarker)
        {
            Logger = logger;
            DateTimeProvider = dateTimeProvider;
            HighlightSchemaAccessor = highlightSchemaAccessor;
            TextUpdateListener = textUpdateListener;
            TextMarkerService = textMarkerService;
            TextViewEventListener = textViewEventListener;
            ComponentDefinitionMarker = componentDefinitionMarker;
        }

        public void Configure(TextEditor textEditor)
        {
            LastConfiguredEditor = textEditor;
            ConfigureSyntaxHighlighting(textEditor);
            ConfigureErrorHighlighting(textEditor);
            ConfigureTrackers(textEditor);
        }

        protected void ConfigureSyntaxHighlighting(TextEditor textEditor)
        {
            textEditor.SyntaxHighlighting = HighlightSchemaAccessor.GetHighlightingDefinition();
        }

        protected void ConfigureErrorHighlighting(TextEditor textEditor)
        {
            TextMarkerService.InjectEditor(textEditor);
            ComponentDefinitionMarker.InjectEditor(textEditor);
        }

        protected void ConfigureTrackers(TextEditor textEditor)
        {
            TextUpdateListener.InjectEditor(textEditor);
            TextViewEventListener.InjectEditor(textEditor);
        }

        public void TearDown(TextEditor textEditor)
        {
            TextUpdateListener.TearDownEditor(textEditor);
            TextMarkerService.TearDownEditor(textEditor);
            TextViewEventListener.TearDownEditor(textEditor);
            ComponentDefinitionMarker.TearDownEditor(textEditor);
        }
    }
}
