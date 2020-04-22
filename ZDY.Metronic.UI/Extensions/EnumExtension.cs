using System;
using System.Text.RegularExpressions;

namespace ZDY.Metronic.UI
{
    internal static class EnumExtension
    {
        /// <summary>
        /// 返回转化为小写的 Enum 名称
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        internal static string ToValue(this object input)
        {                
            return input.ToString().ParseValue().ToLower();
        }

        /// <summary>
        /// 将驼峰命名的 Enum 名称的单词分离，并用 - 连接
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        internal static string ParseValue(this string input)
        {
            var matchs = new Regex("[A-Z]").Matches(input);

            if (matchs.Count > 1)
            {
                foreach (Match match in matchs)
                {
                    input = input.Replace(match.Value, $"-{match.Value}");
                }

                input = input.Substring(1);
            }

            return Regex.Replace(input, "_", "-");
        }

        /// <summary>
        /// 使用了除 None 之外的值即返回 true
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        internal static bool IsUsed(this object input)
        {
            return Convert.ToInt32(input) > 0;
        }
    }
}
