using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenie.Ui.Wpf.Controls.Shared.Services
{
    /// <summary> Component to provide an abstraction over the DateTime object </summary>
    public interface IDateTimeProvider
    {
        DateTime UtcNow { get; }
        DateTime Now { get; }
    }

    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime UtcNow => DateTime.UtcNow;
        public DateTime Now => DateTime.Now;
    }
}
