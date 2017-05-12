using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace WPFDesigner_XML.Common.Converters
{
    internal class StyleHelper
    {
        /// <summary>
        /// Colors from formatted string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", MessageId = "System.Int32.TryParse(System.String,System.Int32@)")]
        public static Color? ColorFromFormattedString(string value)
        {
            Color? color = null;
            try
            {
                string[] vals = value.Split(',');
                if (vals.Count() == 4)
                {
                    vals[0] = vals[0].Substring(1, vals[0].Length - 1);
                    vals[3] = vals[3].Substring(0, vals[3].Length - 1);
                    int r, g, b, a = 0;

                    int.TryParse(vals[0], out r);
                    int.TryParse(vals[1], out g);
                    int.TryParse(vals[2], out b);
                    int.TryParse(vals[3], out a);

                    color = Color.FromArgb(Convert.ToByte(a), Convert.ToByte(r), Convert.ToByte(g), Convert.ToByte(b));
                }
            }
            catch
            { }

            return color;
        }

        /// <summary>
        /// Colors from formatted string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", MessageId = "System.Int32.TryParse(System.String,System.Int32@)")]
        public static string FormattedStringFromColor(Color value)
        {
            return string.Format("{{{0},{1},{2},{3}}}", value.R.ToString(), value.G.ToString(), value.B.ToString(), value.A.ToString());
        }
    }
}
