using System;

namespace LMS_Web.Models
{
    public static class GetMonthName
    {
        public static string Info(int month)
        {
            DateTime date = new DateTime(2020, month, 1);

            return date.ToString("MMM");
        }
        public static string GetFullName(int month)
        {
            DateTime date = new DateTime(2020, month, 1);

            string name= date.ToString("MMMM");
            return name;
        }
        public static  string MonthInBangla(int month)
        {
            switch (month)
            {
                case 1:
                    return "জানুয়ারি";
                    break;
                case 2:
                    return "ফেব্রুয়ারি";
                    break;
                case 3:
                    return "মার্চ";
                    break;
                case 4:
                    return "এপ্রিল";
                    break;
                case 5:
                    return "মে";
                    break;
                case 6:
                    return "জুন";
                    break;
                case 7:
                    return "জুলাই";
                    break;
                case 8:
                    return "আগস্ট";
                    break;
                case 9:
                    return "সেপ্টেম্বর";
                    break;
                case 10:
                    return "অক্টোবর";
                    break;
                case 11:
                    return "নভেম্বর";
                    break;
                case 12:
                    return "ডিসেম্বর";
                    break;
                default:
                    return "";
                    break;

            }

        }
    }
}
