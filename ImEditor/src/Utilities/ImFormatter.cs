using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ImEditor.Utilities
{
    public static class ImFormatter 
    {
        public static string Format(string source, string format, params string[] args)
        {
            StringBuilder sbResult = new StringBuilder();

            var array = source.ToCharArray();

            bool inBrackeys = false;

            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == format[0])
                {
                    inBrackeys = true;
                }
                else if(inBrackeys)
                {
                    int index = int.Parse(array[i].ToString());
                    sbResult.Append(args[index]);
                    inBrackeys = false;
                }
                else if (array[i] == format[1])
                {
                }
                else
                {
                    sbResult.Append(array[i]);
                }
            }

            return sbResult.ToString();
        }
    }
}
