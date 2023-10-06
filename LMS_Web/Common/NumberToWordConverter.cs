using System;
using System.Linq;

namespace LMS_Web.Common
{
    public static class NumberToWordConverter
    {
        private static String ones(String Number)
        {
            int _Number = Convert.ToInt32(Number);
            String name = "";
            switch (_Number)
            {

                case 1:
                    name = "One";
                    break;
                case 2:
                    name = "Two";
                    break;
                case 3:
                    name = "Three";
                    break;
                case 4:
                    name = "Four";
                    break;
                case 5:
                    name = "Five";
                    break;
                case 6:
                    name = "Six";
                    break;
                case 7:
                    name = "Seven";
                    break;
                case 8:
                    name = "Eight";
                    break;
                case 9:
                    name = "Nine";
                    break;
            }
            return name;
        }
        private static String tens(String Number)
        {
            int _Number = Convert.ToInt32(Number);
            String name = null;
            switch (_Number)
            {
                case 10:
                    name = "Ten";
                    break;
                case 11:
                    name = "Eleven";
                    break;
                case 12:
                    name = "Twelve";
                    break;
                case 13:
                    name = "Thirteen";
                    break;
                case 14:
                    name = "Fourteen";
                    break;
                case 15:
                    name = "Fifteen";
                    break;
                case 16:
                    name = "Sixteen";
                    break;
                case 17:
                    name = "Seventeen";
                    break;
                case 18:
                    name = "Eighteen";
                    break;
                case 19:
                    name = "Nineteen";
                    break;
                case 20:
                    name = "Twenty";
                    break;
                case 30:
                    name = "Thirty";
                    break;
                case 40:
                    name = "Fourty";
                    break;
                case 50:
                    name = "Fifty";
                    break;
                case 60:
                    name = "Sixty";
                    break;
                case 70:
                    name = "Seventy";
                    break;
                case 80:
                    name = "Eighty";
                    break;
                case 90:
                    name = "Ninety";
                    break;
                default:
                    if (_Number > 0)
                    {
                        name = tens(Number.Substring(0, 1) + "0") + " " + ones(Number.Substring(1));
                    }
                    break;
            }
            return name;
        }
        private static String ConvertWholeNumber(String Number)
        {
            string word = "";
            try
            {
                bool beginsZero = false;//tests for 0XX    
                bool isDone = false;//test if already translated    
                double dblAmt = (Convert.ToDouble(Number));
                //if ((dblAmt > 0) && number.StartsWith("0"))    
                if (dblAmt > 0)
                {//test for zero or digit zero in a nuemric    
                    beginsZero = Number.StartsWith("0");

                    int numDigits = Number.Length;
                    int pos = 0;//store digit grouping    
                    String place = "";//digit grouping name:hundres,thousand,etc...    
                    switch (numDigits)
                    {
                        case 1://ones' range    

                            word = ones(Number);
                            isDone = true;
                            break;
                        case 2://tens' range    
                            word = tens(Number);
                            isDone = true;
                            break;
                        case 3://hundreds' range    
                            pos = (numDigits % 3) + 1;
                            place = " Hundred ";
                            break;
                        case 4://thousands' range    
                        case 5:
                            pos = (numDigits % 4) + 1;
                            place = " Thousand ";
                            break;
                        case 6://lac range
                            pos = (numDigits % 6) + 1;
                            place = " Lakh ";
                            break;
                        case 7://millions' range
                        case 8:
                        case 9:
                            pos = (numDigits % 7) + 1;
                            place = " Million ";
                            break;
                        case 10://Billions's range    
                        case 11:
                        case 12:

                            pos = (numDigits % 10) + 1;
                            place = " Billion ";
                            break;
                        //add extra case options for anything above Billion...    
                        default:
                            isDone = true;
                            break;
                    }
                    if (!isDone)
                    {//if transalation is not done, continue...(Recursion comes in now!!)    
                        if (Number.Substring(0, pos) != "0" && Number.Substring(pos) != "0")
                        {
                            try
                            {
                                word = ConvertWholeNumber(Number.Substring(0, pos)) + place + ConvertWholeNumber(Number.Substring(pos));
                            }
                            catch { }
                        }
                        else
                        {
                            word = ConvertWholeNumber(Number.Substring(0, pos)) + ConvertWholeNumber(Number.Substring(pos));
                        }

                        //check for trailing zeros    
                        //if (beginsZero) word = " and " + word.Trim();    
                    }
                    //ignore digit grouping names    
                    if (word.Trim().Equals(place.Trim())) word = "";
                }
            }
            catch { }
            return word.Trim();
        }
        private static String ConvertDecimals(String number)
        {
            String cd = "", digit = "", engOne = "";
            for (int i = 0; i < number.Length; i++)
            {
                digit = number[i].ToString();
                if (digit.Equals("0"))
                {
                    engOne = "Zero";
                }
                else
                {
                    engOne = ones(digit);
                }
                cd += " " + engOne;
            }
            return cd;
        }
        public static String ConvertToWords(String numb)
        {
            String val = "", wholeNo = numb, points = "", andStr = "", pointStr = "";
            String endStr = "Only";
            try
            {
                int decimalPlace = numb.IndexOf(".");
                if (decimalPlace > 0)
                {
                    wholeNo = numb.Substring(0, decimalPlace);
                    points = numb.Substring(decimalPlace + 1);
                    if (Convert.ToInt32(points) > 0)
                    {
                        andStr = "and";// just to separate whole numbers from points/cents    
                        endStr = "Paisa " + endStr;//Cents    
                        pointStr = ConvertDecimals(points);
                    }
                }
                val = String.Format("{0} {1}{2} {3}", ConvertWholeNumber(wholeNo).Trim(), andStr, pointStr, endStr);
            }
            catch { }
            return val;
        }



        private static String onesBangla(String Number)
        {
            int _Number = Convert.ToInt32(Number);
            String name = "";
            switch (_Number)
            {
                case 0:
                    name = "শুন্য";
                    break;
                case 1:
                    name = "এক";
                    break;
                case 2:
                    name = "দুই";
                    break;
                case 3:
                    name = "তিন";
                    break;
                case 4:
                    name = "চার";
                    break;
                case 5:
                    name = "পাঁচ";
                    break;
                case 6:
                    name = "ছয়";
                    break;
                case 7:
                    name = "সাত";
                    break;
                case 8:
                    name = "আট";
                    break;
                case 9:
                    name = "নয়";
                    break;

                case 10:
                    name = "দশ";
                    break;
                case 11:
                    name = "এগারো";
                    break;
                case 12:
                    name = "বারো";
                    break;
                case 13:
                    name = "তেরো";
                    break;
                case 14:
                    name = "চৌদ্দ";
                    break;
                case 15:
                    name = "পনেরো";
                    break;
                case 16:
                    name = "ষোলো";
                    break;
                case 17:
                    name = "সতেরো";
                    break;
                case 18:
                    name = "আঠারো";
                    break;

                case 19:
                    name = "উনিশ";
                    break;
                case 20:
                    name = "বিশ";
                    break;
                case 21:
                    name = "একুশ";
                    break;
                case 22:
                    name = "বাইশ";
                    break;
                case 23:
                    name = "তেইশ";
                    break;
                case 24:
                    name = "চব্বিশ";
                    break;
                case 25:
                    name = "পঁচিশ";
                    break;
                case 26:
                    name = "ছাব্বিশ";
                    break;
                case 27:
                    name = "সাতাশ";
                    break;

                case 28:
                    name = "আটাশ";
                    break;
                case 29:
                    name = "উনত্রিশ";
                    break;
                case 30:
                    name = "ত্রিশ";
                    break;
                case 31:
                    name = "একত্রিশ";
                    break;
                case 32:
                    name = "বত্রিশ";
                    break;
                case 33:
                    name = "তেত্রিশ";
                    break;
                case 34:
                    name = "চৌত্রিশ";
                    break;
                case 35:
                    name = "পঁয়ত্রিশ";
                    break;
                case 36:
                    name = "ছত্রিশ";
                    break;

                case 37:
                    name = "সাইত্রিশ";
                    break;
                case 38:
                    name = "আটত্রিশ";
                    break;
                case 39:
                    name = "উনচল্লিশ";
                    break;
                case 40:
                    name = "চল্লিশ";
                    break;
                case 41:
                    name = "একচল্লিশ";
                    break;
                case 42:
                    name = "বিয়াল্লিশ";
                    break;
                case 43:
                    name = "তেতাল্লিশ";
                    break;
                case 44:
                    name = "চুয়াল্লিশ";
                    break;
                case 45:
                    name = "পঁয়তাল্লিশ";
                    break;

                case 46:
                    name = "ছেচল্লিশ";
                    break;
                case 47:
                    name = "সাতচল্লিশ";
                    break;
                case 48:
                    name = "আটচল্লিশ";
                    break;
                case 49:
                    name = "উনপঞ্চাশ";
                    break;
                case 50:
                    name = "পঞ্চাশ";
                    break;
                case 51:
                    name = "একান্ন";
                    break;
                case 52:
                    name = "বায়ান্ন";
                    break;
                case 53:
                    name = "তিপ্পান্ন";
                    break;
                case 54:
                    name = "চুয়ান্ন";
                    break;

                case 55:
                    name = "পঞ্চান্ন";
                    break;
                case 56:
                    name = "ছাপান্ন";
                    break;
                case 57:
                    name = "সাতান্ন";
                    break;
                case 58:
                    name = "আটান্ন";
                    break;
                case 59:
                    name = "উনষাট";
                    break;
                case 60:
                    name = "ষাট";
                    break;
                case 61:
                    name = "একষট্টি";
                    break;
                case 62:
                    name = "বাষট্টি";
                    break;
                case 63:
                    name = "তেষট্টি";
                    break;

                case 64:
                    name = "চৌষট্টি";
                    break;
                case 65:
                    name = "পঁয়ষট্টি";
                    break;
                case 66:
                    name = "ছেষট্টি";
                    break;
                case 67:
                    name = "সাতষট্টি";
                    break;
                case 68:
                    name = "আটষট্টি";
                    break;
                case 69:
                    name = "উনসত্তর";
                    break;
                case 70:
                    name = "সত্তর";
                    break;
                case 71:
                    name = "একাত্তর";
                    break;
                case 72:
                    name = "বাহাত্তর";
                    break;

                case 73:
                    name = "তিয়াত্তর";
                    break;
                case 74:
                    name = "চুয়াত্তর";
                    break;
                case 75:
                    name = "পঁচাত্তর";
                    break;
                case 76:
                    name = "ছিয়াত্তর";
                    break;
                case 77:
                    name = "সাতাত্তর";
                    break;

                case 78:
                    name = "আটাত্তর";
                    break;
                case 79:
                    name = "উনআশি";
                    break;
                case 80:
                    name = "আশি";
                    break;
                case 81:
                    name = "একাশি";
                    break;

                case 82:
                    name = "বিরাশি";
                    break;
                case 83:
                    name = "তিরাশি";
                    break;
                case 84:
                    name = "চুরাশি";
                    break;
                case 85:
                    name = "পঁচাশি";
                    break;
                case 86:
                    name = "ছিয়াশি";
                    break;
                case 87:
                    name = "সাতাশি";
                    break;
                case 88:
                    name = "আটাশি";
                    break;
                case 89:
                    name = "উননব্বই";
                    break;
                case 90:
                    name = "নব্বই";
                    break;

                case 91:
                    name = "একানব্বই";
                    break;
                case 92:
                    name = "বিরানব্বই";
                    break;
                case 93:
                    name = "তিরানব্বই";
                    break;
                case 94:
                    name = "চুরানব্বই";
                    break;
                case 95:
                    name = "পঁচানব্বই";
                    break;
                case 96:
                    name = "ছিয়ানব্বই";
                    break;
                case 97:
                    name = "সাতানব্বই";
                    break;
                case 98:
                    name = "আটানব্বই";
                    break;
                case 99:
                    name = "নিরানব্বই";
                    break;
                    //case 100:
                    //    name = "একশ";
                    //    break;

            }
            return name;
        }
        private static String ConvertWholeNumberBangla(String Number)
        {
            string word = "";
            try
            {
                bool beginsZero = false;//tests for 0XX    
                bool isDone = false;//test if already translated    
                double dblAmt = (Convert.ToDouble(Number));
                //if ((dblAmt > 0) && number.StartsWith("0"))    
                if (dblAmt >= 0)
                {//test for zero or digit zero in a nuemric    
                    beginsZero = Number.StartsWith("0");

                    int numDigits = Number.Length;
                    int pos = 0;//store digit grouping    
                    String place = "";//digit grouping name:hundres,thousand,etc...    
                    switch (numDigits)
                    {
                        case 1://ones' range    

                            word = onesBangla(Number);
                            isDone = true;
                            break;
                        case 2://tens' range    
                            word = onesBangla(Number);
                            isDone = true;
                            break;
                        case 3://hundreds' range    
                            pos = (numDigits % 3) + 1;
                            place = " শত ";
                            break;
                        case 4://thousands' range    
                        case 5:
                            pos = (numDigits % 4) + 1;
                            place = " হাজার ";
                            break;
                        case 6://lac range
                        case 7://millions' range
                            pos = (numDigits % 6) + 1;
                            place = " লাখ ";
                            break;
                        case 8:
                        case 9:
                        case 10://Billions's range    
                        case 11:
                        case 12:
                            pos = (numDigits % 8) + 1;
                            place = " কোটি ";
                            break;
                        //add extra case options for anything above Billion...    
                        default:
                            isDone = true;
                            break;
                    }
                    if (!isDone)
                    {//if transalation is not done, continue...(Recursion comes in now!!)    
                        if (Number.Substring(0, pos) != "0" && Number.Substring(pos) != "0")
                        {
                            try
                            {
                                word = ConvertWholeNumberBangla(Number.Substring(0, pos)) + place + ConvertWholeNumberBangla(Number.Substring(pos));
                            }
                            catch { }
                        }
                        else
                        {
                            word = ConvertWholeNumberBangla(Number.Substring(0, pos)) + ConvertWholeNumberBangla(Number.Substring(pos));
                        }

                        //check for trailing zeros    
                        //if (beginsZero) word = " and " + word.Trim();    
                    }
                    //ignore digit grouping names    
                    if (word.Trim().Equals(place.Trim())) word = "";
                }
            }
            catch { }
            return word.Trim();
        }
        private static String ConvertDecimalsBangla(String number)
        {
            String cd = "", digit = "", engOne = "";
            for (int i = 0; i < number.Length; i++)
            {
                digit = number[i].ToString();
                if (digit.Equals("0"))
                {
                    engOne = "শুন্য";
                }
                else
                {
                    engOne = onesBangla(digit);
                }
                cd += " " + engOne;
            }
            return cd;
        }
        public static String ConvertToWordsBangla(String numb)
        {
            String val = "", wholeNo = numb, points = "", andStr = "", pointStr = "";
            String endStr = "মাত্র";
            try
            {
                int decimalPlace = numb.IndexOf(".");
                if (decimalPlace > 0)
                {
                    wholeNo = numb.Substring(0, decimalPlace);
                    points = numb.Substring(decimalPlace + 1);
                    if (Convert.ToInt32(points) >= 0)
                    {
                        andStr = "টাকা এবং";// just to separate whole numbers from points/cents    
                        endStr = "পয়সা " + endStr;//Cents    
                        pointStr = ConvertDecimalsBangla(points);
                    }
                }
                else
                {
                    andStr = "টাকা";

                }
                val = String.Format("{0} {1}{2} {3}", ConvertWholeNumberBangla(wholeNo).Trim(), andStr, pointStr, endStr);
            }
            catch { }
            return val;
        }

        public static String EnglishToBanglaDate(DateTime date)
        {
            string banglaDate = "";
            var day = string.Concat(date.Day.ToString().Select(c => (char)('\u09E6' + c - '0')));
            var month = string.Concat(date.Month.ToString().Select(c => (char)('\u09E6' + c - '0')));
            var year = string.Concat(date.Year.ToString().Select(c => (char)('\u09E6' + c - '0')));

            banglaDate=day + "-" + month +"-"+ year;


            return banglaDate;
        }


    }
}
