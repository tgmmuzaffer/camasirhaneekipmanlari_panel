using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace panel.RepoExtension
{
    public static class ClearString
    {
        public static string Clear(string val)
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
    }
}
