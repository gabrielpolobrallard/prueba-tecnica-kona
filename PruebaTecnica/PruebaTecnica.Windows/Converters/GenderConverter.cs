using PruebaTecnica.Models;
using System;
using Windows.UI.Xaml.Data;

namespace PruebaTecnica.Converters
{
    public class GenderConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var result = value is Gender;
            if (result)
            {
                if ((Gender)value == Gender.Male)
                {
                    return "Masculino";
                }
                else
                {
                    return "Femenino";
                }
            }
            return "Indefinido";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}