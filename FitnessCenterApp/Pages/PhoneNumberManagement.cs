using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessCenterApp.Pages
{
    static class PhoneNumberManagement
    {
        public static long StringToNumber(string input)
        {
            if (string.IsNullOrEmpty(input))
                return 0;
            char[] unformatted = input.Where(char.IsDigit).ToArray();
            long result = long.Parse(new String(unformatted));
            if (input[0] != '+')
                result -= (long)Math.Pow(10, unformatted.Length - 1); // Хранить +7 вместо 8
            return result;
        }

        public static string NumberToFormattedString(long number)
        {
            StringBuilder formattedNumber = new StringBuilder();
            formattedNumber.Append(number);
            int countryCodeLength = 0;
            switch (formattedNumber[0])
            {
                case '1':
                case '7':
                    countryCodeLength = 1;
                    break;
                default:
                    countryCodeLength = 2;
                    break;
            }
            formattedNumber.Insert(0, '+');
            formattedNumber.Insert(countryCodeLength + 1, " (");
            formattedNumber.Insert(countryCodeLength + 6, ") ");
            formattedNumber.Insert(countryCodeLength + 11, '-');
            formattedNumber.Insert(countryCodeLength + 14, '-');
            return formattedNumber.ToString();
        }

        public static bool IsNumberCorrect(string input)
        {
            return input.Count(Char.IsDigit) >= 11;
        }
    }
}
