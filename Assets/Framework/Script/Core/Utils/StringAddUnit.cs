using System.Text.RegularExpressions;

namespace FrameWork
{
    /// <summary>
    /// 
    /// </summary>
    public static class StringAddUnit
    {
        /// <summary>
        /// 字符串转int
        /// </summary>
        /// <param name="_value"></param>
        /// <returns></returns>
        public static int ToInt(this string _value)
        {
            if (Regex.IsMatch(_value, @"^[+-]?[0-9]*$"))
                return int.Parse(_value);
            else
            {
                DebugUtil.LogError($"ToInt", _value);
                return 0;
            }
        }

        /// <summary>
        /// 字符串转float
        /// </summary>
        /// <param name="_value"></param>
        /// <returns></returns>
        public static float ToFloat(this string _value)
        {
            if (Regex.IsMatch(_value, @"^([0-9]{1,}[.][0-9]*)$"))
                return float.Parse(_value);
            else
            {
                DebugUtil.LogError($"ToFloat", _value);
                return 0;
            }
        }


        /// <summary>
        /// 添加单位
        /// </summary>
        /// <param name="_value"></param>
        /// <returns></returns>
        public static string AddUnit(this string _value)
        {
            return Numdispose(_value);
        }

        /// <summary>
        /// 添加单位
        /// </summary>
        /// <param name="_value"></param>
        /// <returns></returns>
        public static string AddUnit(this int _value)
        {
            return Numdispose(_value.ToString());
        }

        /// <summary>
        /// 数字换算
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        private static string Numdispose(string num)
        {
            string[] symbol = { "K", "M", "B", "T", "aa", "ab", "ac", "ad" };
            string str1 = string.Empty;
            string str2 = string.Empty;
            if (num.Length > 4)
            {
                int a = (num.Length - 4) / 3;
                str1 = num.Substring(0, (num.Length - (3 * (a + 1))));
                int b = num.Length - (3 * (a + 1));
                str2 = num[b].ToString();
                if (int.Parse(str2) >= 5) str1 = (int.Parse(str1) + 1).ToString();
                if (str1.Length > 3) return str1.Insert(str1.Length - 3, ",") + symbol[a];
                return str1 + symbol[a];
            }
            return num;
        }
    }
}