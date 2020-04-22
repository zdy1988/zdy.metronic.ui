using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ZDY.Metronic.UI
{
    public static class StringExtension
    {
        /// <summary>
        /// 得到字符串长度，一个汉字长度为2
        /// </summary>
        /// <param name="input">参数字符串</param>
        /// <returns></returns>
        public static int GetByteLength(this string input)
        {
            if (String.IsNullOrWhiteSpace(input))
                return 0;

            return Regex.Replace(input, "[^\x00-\xff]", "**").Length;
        }

        /// <summary>
        /// 按字节截取字符串
        /// </summary>
        /// <param name="input"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string SubByteString(this string input, int length)
        {
            if (String.IsNullOrWhiteSpace(input))
                return "";

            var array = input.ToCharArray();

            var array2 = new List<String>();

            foreach (var item in array)
            {
                array2.Add(item.ToString());

                length -= item.ToString().GetByteLength();

                if (length <= 0)
                {
                    break;
                }
            }

            return array2.Count() > 0 ? String.Join("", array2) : "";
        }
    }
}
