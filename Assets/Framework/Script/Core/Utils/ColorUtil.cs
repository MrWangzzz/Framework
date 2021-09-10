using UnityEngine;
namespace FrameWork
{
    /// <summary>
    /// 
    /// </summary>
    public static class ColorUtil
    {
        /// <summary>
        /// hax转颜色
        /// </summary>
        /// <param name="hax"> 不加#号时 可直接输入red, cyan, blue, darkblue, lightblue, purple, yellow, lime, fuchsia, white, silver, grey, black, orange, brown, maroon, green, olive, navy, teal, aqua, magenta 加#号随意 </param>
        /// <param name="hax"> 不加#号时 可直接输入 红色，青色，蓝色，深蓝色，浅蓝色，紫色，黄色，石灰，紫红色，白色，银色，灰色，黑色，橙色，棕色，栗色，绿色，橄榄色，海军，蓝绿色，浅绿色，洋红色。 加#号随意 </param>
        /// <returns>成功时转换颜色，失败时默认白色</returns>
        public static Color GetColorByHax(this string hax)
        {
            Color nowColor;
            bool isSuccess = ColorUtility.TryParseHtmlString(hax, out nowColor);
            if (isSuccess)
                return nowColor;
            else
            {
                DebugUtil.LogError("颜色hax错误：", hax);
                return Color.white;
            }
        }

        /// <summary>
        /// 颜色转rgb
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static string GetRGBByColor(this Color color)
        {
            return ColorUtility.ToHtmlStringRGB(color);
        }

        /// <summary>
        /// 颜色转rgba
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static string GetRGBAByColor(this Color color)
        {
            return ColorUtility.ToHtmlStringRGBA(color);
        }
    }
}