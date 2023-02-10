using CodeGenie.Ui.Wpf.Controls.CodeEditor.Contracts;
using CodeGenie.Ui.Wpf.Controls.Shared;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
        ILogger<MainViewModel> _logger;
        public MainViewModel(ILogger<MainViewModel> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            Title = "CodeGenie TestApp";
            ServiceProvider = serviceProvider;
            TextListener = ServiceProvider.GetService<ITextUpdateListener>();
            CopyTextCommand = new RelayCommand(() =>
            {
                try
                {
                    var text = TextListener.CurrentText;
                    using (var writer = new StringWriter())
                    using (var provider = CodeDomProvider.CreateProvider("CSharp"))
                    {
                        provider.GenerateCodeFromExpression(new CodePrimitiveExpression(text), writer, null);
                        Clipboard.SetText(writer.ToString());
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"{nameof(CopyTextCommand)} had an error. {ex}");
                }
            });
        }

        public IServiceProvider ServiceProvider { get => Get<IServiceProvider>(); set => Set(value); }
        public ITextUpdateListener TextListener { get; set; }
        public string Title { get => Get<string>(); set => Set(value); }

        public RelayCommand CopyTextCommand { get; set; }
    }
}
