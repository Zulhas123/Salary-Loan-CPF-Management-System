using System;
using System.Linq;

namespace LMS_Web.Common
{
    public static class Converter
    {
        public static string EnglishToBanglaNumberConvert(decimal number)
        {
            return string.Concat(number.ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".");

        }

        internal static string EnglishToBanglaNumberConvert(string value)
        {
            if(value == "0/0")
            {
                value= "0";
            }
            return string.Concat(value.Select(c => (char)('\u09E6' + c - '0'))).Replace("৥", "/").Replace("৤", ".");
           
        }
    }
}
