using CodeGenie.Ui.Wpf.Controls.CodeEditor.Contracts;
using CodeGenie.Ui.Wpf.Controls.Shared;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CodeGenie.Ui.Wpf.TestApp
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel(IServiceProvider serviceProvider)
        {
            Title = "CodeGenie TestApp";
            ServiceProvider = serviceProvider;
            TextListener = ServiceProvider.GetService<ITextUpdateListener>();
            CopyTextCommand = new RelayCommand(() =>
            {
                var text = TextListener.CurrentText;
                using (var writer = new StringWriter())
                using (var provider = CodeDomProvider.CreateProvider("CSharp"))
                {
                    provider.GenerateCodeFromExpression(new CodePrimitiveExpression(text), writer, null);
                    Clipboard.SetText(writer.ToString());
                }
            });
        }

        public IServiceProvider ServiceProvider { get => Get<IServiceProvider>(); set => Set(value); }
        public ITextUpdateListener TextListener { get; set; }
        public string Title { get => Get<string>(); set => Set(value); }

        public RelayCommand CopyTextCommand { get; set; }
    }
}
