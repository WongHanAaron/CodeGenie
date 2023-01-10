using CodeGenie.Ui.Wpf.Controls.CodeEditor.Contracts;
using CodeGenie.Ui.Wpf.Controls.CodeEditor.Models.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenie.Ui.Wpf.Controls.CodeEditor.Services.EditorTracking
{
    public class TextUpdateListener : ITextUpdateListener
    {
        public EventHandler<TextUpdateEvent> OnTextUpdated { get; set; }

        public void TextWasUpdated(object sender, TextUpdateEvent evt)
        {
            OnTextUpdated?.Invoke(sender, evt);
        }
    }
}
