using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace FoodDeliveryAdmin.ViewModel
{
    public class CategoryNameConverter : IValueConverter
    {

        //From number to string
        public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            // check value
            if (value == null || !(value is Int32))
                return Binding.DoNothing;

            // check parameter
            if (parameter == null || !(parameter is IEnumerable<String>))
                return Binding.DoNothing;

            List<String> catNames = (parameter as IEnumerable<String>).ToList();
            Int32 index = (Int32)value - 1; //CategoryID is indexed from 1

            if (index < 0 || index >= catNames.Count)
                return Binding.DoNothing;

            return catNames[index];
        }

        //From string to number
        public Object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            // check value
            if (value == null || !(value is String))
                return DependencyProperty.UnsetValue;

            // check parameter
            if (parameter == null || !(parameter is IEnumerable<String>))
                return Binding.DoNothing;

            List<String> catNames = (parameter as IEnumerable<String>).ToList();
            String name = (String)value;

            // search whether name exists
            if (!catNames.Contains(name))
                return DependencyProperty.UnsetValue;

            Int32 result = (Int32)catNames.IndexOf(name);

            return  result + 1; //this will actually conform to the category ID
        }
    }
}
