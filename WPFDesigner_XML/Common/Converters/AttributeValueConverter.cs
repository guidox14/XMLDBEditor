using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Data;
using WPFDesigner_XML.Common.Models;
using WPFDesigner_XML.Common.Models.UIModels;

namespace WPFDesigner_XML.Common.Converters
{
    [ValueConversion(typeof(object), typeof(String))]
    public class AttributeValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null )
            {
                return null;
            }
            if (!targetType.Equals(typeof(string)))
            {
                throw new ArgumentOutOfRangeException("targetType", "The convert method can only convert to string, to convert from string to the parameter type use ConverBack"); 
            }

            if (parameter != null)
            {
                switch ((string)parameter)
                {
                    case "color":
                        {
                            if (value is System.String)
                            {
                                return value.ToString();
                            }
                            else
                            {
                                StyleHelper.FormattedStringFromColor((System.Windows.Media.Color)value);
                            }
                        }
                        break;
                    case "regex":
                        {
                            Regex regex = value as Regex;
                            return regex.ToString();
                        }
                    case "Collection":
                        {
                            TrulyObservableCollection<UIControlInstanceModel> collection = value as TrulyObservableCollection<UIControlInstanceModel>;
                            return collection.Count().ToString(culture);
                        }
                    case "rect":
                        {
                            Rect rectvalue = (Rect)value;
                            StringBuilder stringValue = new StringBuilder();
                            stringValue.Append("{");
                            stringValue.Append("{");
                            stringValue.AppendFormat("{0},{1}", rectvalue.Left, rectvalue.Top);
                            stringValue.Append("}");
                            stringValue.Append(",");
                            stringValue.Append("{");
                            stringValue.AppendFormat("{0},{1}", rectvalue.Width, rectvalue.Height);
                            stringValue.Append("}");
                            stringValue.Append("}");
                            return stringValue.ToString();
                        }
                    case "inset":
                        {
                            Rect rectValue = (Rect)value;
                            StringBuilder stringValue = new StringBuilder();
                            stringValue.Append("{");
                            stringValue.AppendFormat("{0},{1},{2},{3}",rectValue.Top, rectValue.Left, rectValue.Height, rectValue.Width);
                            stringValue.Append("}");
                            return stringValue.ToString();
                        }
                }
            }

            if (value != null)
            {
                return value.ToString();
            }
            else
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string stringValue = value as string;
            if (targetType.Equals(typeof(string)))
            {
                throw new ArgumentOutOfRangeException("targetType", "The ConvertBack method can only convert from string, to convert to string Convert method");
            }
            switch (((string)parameter).ToLowerInvariant())
            {
                case "color":
                    {
                        return StyleHelper.ColorFromFormattedString(stringValue);
                    }
                    break;
                case "int":
                    int intValue = 0;
                    if (int.TryParse(stringValue, out intValue))
                    {
                        return intValue;
                    }
                    else
                    {
                        throw new InvalidCastException(string.Format("The value {0} can't be cast to {1}", stringValue, typeof(int).Name));
                    }
                case "double":
                    double doubleValue = 0;
                    if (double.TryParse(stringValue, out doubleValue))
                    {
                        return doubleValue;
                    }
                    else
                    {
                        throw new InvalidCastException(string.Format("The value {0} can't be cast to {1}", stringValue, typeof(int).Name));
                    }
                case "bool":
                case "boolean":
                    bool boolValue = false;
                    if (bool.TryParse(stringValue, out boolValue))
                    {
                        return boolValue;
                    }
                    else
                    {
                        throw new InvalidCastException(string.Format("The value {0} can't be cast to {1}", stringValue, typeof(int).Name));
                    }
                case "datetime":
                    DateTime datetimeValue = DateTime.MinValue;
                    if (DateTime.TryParse(stringValue, out datetimeValue))
                    {
                        return datetimeValue;
                    }
                    else
                    {
                        throw new InvalidCastException(string.Format("The value {0} can't be cast to {1}", stringValue, typeof(int).Name));
                    }
                case "regex":
                    Regex regex = new Regex(stringValue);
                    return regex;
                case "Collection":
                    return new TrulyObservableCollection<UIControlInstanceModel>();
                case "rect":
                    Rect rectangle = ParseRectangle(stringValue);
                    return rectangle;
                case "inset":
                    Rect insetRectangle = ParseInset(stringValue);
                    return insetRectangle;
                case "string":
                    return stringValue;
            }

           
            return DependencyProperty.UnsetValue;
        }
        public static Rect ParseInset(string stringValue)
        {
            double top = 0, left = 0, right = 0, bottom = 0;

            if (stringValue.StartsWith("{") && stringValue.EndsWith("}"))
            {
                string rectString = stringValue.Substring(1, stringValue.Length - 2); // remove side elements

                String[] values = rectString.Split(',');

                if (values.Length == 4)
                {
                    double.TryParse(values[0], NumberStyles.Number, CultureInfo.InvariantCulture, out top);
                    double.TryParse(values[1], NumberStyles.Number, CultureInfo.InvariantCulture, out left);
                    double.TryParse(values[2], NumberStyles.Number, CultureInfo.InvariantCulture, out bottom);
                    double.TryParse(values[3], NumberStyles.Number, CultureInfo.InvariantCulture, out right);
                }
            }

            return new Rect(left, top, right, bottom);
        }

        public static Rect ParseRectangle(string stringValue)
        {
            double x = 0, y = 0, width = 0, height = 0;

            if (stringValue.StartsWith("{") && stringValue.EndsWith("}"))
            {
                string rectString = stringValue.Substring(1, stringValue.Length - 2); // remove side elements
                Regex regx = new Regex(@"[{]\s*[0-9]*([.][0-9]*|)\s*,\s*[0-9]*([.][0-9]*|)\s*[}]");
                var matches = regx.Matches(rectString);
                if (matches.Count > 0)
                {
                    string[] values = matches[0].Value.Substring(1, matches[0].Value.Length - 2).Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (values.Length == 2)
                    {
                        double.TryParse(values[0], NumberStyles.Number, CultureInfo.InvariantCulture, out x);
                        double.TryParse(values[1], NumberStyles.Number, CultureInfo.InvariantCulture, out y);
                    }

                    if (matches.Count == 2)
                    {
                        values = matches[1].Value.Substring(1, matches[1].Value.Length - 2).Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        if (values.Length == 2)
                        {
                            double.TryParse(values[0], NumberStyles.Number, CultureInfo.InvariantCulture, out width);
                            double.TryParse(values[1], NumberStyles.Number, CultureInfo.InvariantCulture, out height);
                        }
                    }
                }
            }

            return new Rect(x, y, width, height);
        }
    }
}
