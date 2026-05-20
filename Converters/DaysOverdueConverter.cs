using System;
using System.Globalization;
using System.Windows.Data;

namespace LibraryManagementSystem.Converters
{
    public class DaysOverdueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime dueDate)
            {
                var days = (DateTime.Now - dueDate).Days;
                return days > 0 ? days.ToString() : "0";
            }
            return "0";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
