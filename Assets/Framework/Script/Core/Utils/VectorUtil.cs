using UnityEngine;
namespace FrameWork
{
    /// <summary>
    /// 
    /// </summary>
    public static class VectorUtil
    {
        /// <summary>
        /// 字符串转Vector3
        /// </summary>
        /// <param name="_value">要转化的字符串 格式为：(1.0,2.0,3,0)</param>
        /// <returns></returns>
        public static Vector3 ToVector3(this string _value)
        {
            _value = _value.Replace("(", "").Replace(")", "");
            string[] Vectors = _value.Trim(' ').Split(',');
            if (Vectors != null && Vectors.Length == 3)
                return new Vector3(Vectors[0].ToFloat(), Vectors[1].ToFloat(), Vectors[2].ToFloat());
            return Vector3.zero;
        }

        /// <summary>
        /// 字符串转Vector2
        /// </summary>
        /// <param name="_value">要转化的字符串 格式为：(1.0,2.0)</param>
        /// <returns></returns>
        public static Vector2 ToVector2(this string _value)
        {
            _value = _value.Replace("(", "").Replace(")", "");
            string[] Vectors = _value.Trim(' ').Split(',');
            if (Vectors != null && Vectors.Length == 2)
                return new Vector2(Vectors[0].ToFloat(), Vectors[1].ToFloat());
            return Vector2.zero;
        }
    }
}