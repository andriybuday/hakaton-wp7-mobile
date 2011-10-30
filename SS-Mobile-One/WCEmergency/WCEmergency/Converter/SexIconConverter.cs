using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using WCEmergency.WCServiceReference;

namespace WCEmergency.Converter
{
    public class SexIconConverter : IValueConverter
    {
        private const string Male =
            "../Resources/Images/Male.png";
        private const string Female =
            "../Resources/Images/Female.png";
        private const string Unisex =
            "../Resources/Images/Unisex.png";

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var _sex = (Sex)value;
            switch (_sex)
            {
                case Sex.Male:
                    return Male;
                case Sex.Female:
                    return Female;
                case Sex.Unisex:
                    return Unisex;
                default: 
                    break;
            }

            return Unisex;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
