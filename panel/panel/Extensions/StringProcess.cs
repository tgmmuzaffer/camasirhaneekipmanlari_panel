using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace panel.Extensions
{
    public static class StringProcess
    {
        public static string ClearString(string val)
        {
            if (!string.IsNullOrEmpty(val))
            {
                string returnText = val.ToLower();
                string[] oldValue = new string[] { "ö", "Ö", "ü", "Ü", "ç", "Ç", "İ", "ı", "Ğ", "ğ", "Ş", "ş", "-", " ", "(", ")", ";", ":", ".", "*", "\\", "/" };
                string[] newValue = new string[] { "o", "O", "u", "U", "c", "C", "I", "i", "G", "g", "S", "s", "", "", "", "", "", "", "", "", "", "" };

                for (int i = 0; i < oldValue.Length; i++)
                {
                    returnText = returnText.Replace(oldValue[i], newValue[i]);

                }

                return returnText;
            }
            return null;
        }

        public static string GenerateString()
        {
            Random res = new Random();
            String str = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            int size = 8;
            String randomstring = "";

            for (int i = 0; i < size; i++)
            {
                int x = res.Next(str.Length);
                randomstring = randomstring + str[x];
            }
            return randomstring;
        }
    }
}
