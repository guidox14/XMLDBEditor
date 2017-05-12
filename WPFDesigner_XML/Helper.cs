using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFDesigner_XML
{
    public static class Helper
    {
        public static bool ValidateBoolFromString(string currentBool)
        {
            if (currentBool.ToLower().Equals("false"))
                return false;
            else
                return true;
        }
    }
}
