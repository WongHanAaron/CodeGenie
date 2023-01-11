using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenie.Ui.Wpf.Controls.Shared
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        private Dictionary<string, object> _propertyValues = new Dictionary<string, object>();
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected T Get<T>([CallerMemberName] string propertyName = null)
            => _propertyValues.TryGetValue(propertyName, out var val) && val is T ? (T)val : default(T);

        protected void Set<T>(T value, [CallerMemberName] string propertyName = null)
        {
            _propertyValues[propertyName] = value;
            OnPropertyChanged(propertyName);
        }
    }
}
