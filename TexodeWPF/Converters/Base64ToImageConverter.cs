using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace TexodeWPF.Converters
{
    public class Base64ToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string baseString = (string)value;
            BitmapImage bitmap = new BitmapImage();
            if (!string.IsNullOrEmpty(baseString))
            {
                
                bitmap.BeginInit();
                bitmap.StreamSource = new MemoryStream(System.Convert.FromBase64String(baseString));
                bitmap.EndInit();
                
                return bitmap;
            }
            bitmap.BeginInit();
            bitmap.UriSource = new Uri("pack://application:,,,/Resources/Images/noimage.png", UriKind.RelativeOrAbsolute);
            bitmap.EndInit();
            return bitmap;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                return System.Convert.ToBase64String((byte[])value);
            }
            return null;
        }
    }
}
