using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using System;
using System.Xml;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeGenie.Ui.Wpf.Controls.CodeEditor.Resources;

namespace CodeGenie.Ui.Wpf.Controls.CodeEditor.Services
{
    public interface IHighlightSchemaAccessor
    {
        IHighlightingDefinition GetHighlightingDefinition();
    }

    public class HighlightingDefinitionAccessor : IHighlightSchemaAccessor
    {
        public IHighlightingDefinition GetHighlightingDefinition()
        {
            var type = typeof(HighlightingDefinitionAccessor);
            var fullPath = typeof(ResourceAccessor).Namespace + ".ComponentDefinitionHighlighting.xshd";
            using (var s = type.Assembly.GetManifestResourceStream(fullPath))
            using (var reader = XmlReader.Create(s))
            {
                return HighlightingLoader.Load(reader, HighlightingManager.Instance);
            }
        }
    }
}
