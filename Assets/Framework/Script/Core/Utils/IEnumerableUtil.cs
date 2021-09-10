using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using System;
using Object = UnityEngine.Object;

namespace FrameWork
{


    public static class IEnumerableUtil
    {

        public enum dicType
        {
            Key,//打印Key
            Value,//打印value
            KeyValue,//key对应value
            KeyAndValue,//显示key和value日志
        }

        /// <summary>集合转字符串</summary>
        /// <param name="_list">集合或数组</param>
        /// <param name="_separator">分隔符</param>
        /// <param name="_prefix">前缀</param>
        /// <param name="_suffix">后缀</param>
        /// <returns>返回字符串</returns>
        public static string ListToString(this IEnumerable _list, string _separator, string _prefix = "", string _suffix = "", bool isShType = true)
        {
            string value = "";
            if (_list == null)
            {
                return "null";
            }

            foreach (object item in _list)
            {
                value += GetString(item, _separator, _prefix, _suffix, isShType);
            }
            return value.Substring(0, value.ToString().LastIndexOf(_separator));
        }

        /// <summary>集合转字符串</summary>
        /// <param name="_list">集合或数组</param>
        /// <param name="_separator">分隔符</param>
        /// <param name="_type">dicType的类型</param>
        /// <param name="_prefix">前缀</param>
        /// <param name="_suffix">后缀</param>
        /// <returns>返回字符串</returns>
        public static string DicToString(this IEnumerable _list, string _separator, dicType _type, string _prefix = "", string _suffix = "", bool isShType = true)
        {
            StringBuilder value = new StringBuilder();
            if (_list == null)
            {
                return value.ToString();
            }

            string _str = "";
            foreach (object item in _list)
            {
                KeyValuePair<object, object> _item = (KeyValuePair<object, object>)item;
                switch (_type)
                {
                    case dicType.Key: _str = GetString(_item.Key, _separator, $"{_prefix} Key = ", _suffix, isShType); break;
                    case dicType.Value: _str = GetString(_item.Value, _separator, $"{_prefix} Value = ", _suffix, isShType); break;
                    case dicType.KeyValue: _str = $"{_prefix} { GetString(_item.Key, isShType: isShType) } = {GetString(_item.Value, isShType: isShType)} {_suffix} {_separator}"; break;
                    case dicType.KeyAndValue: _str = $"{_prefix} Key = { GetString(_item.Key, isShType: isShType) } || Value = {GetString(_item.Value, isShType: isShType)} {_suffix} {_separator}"; break;
                }
                value.Append(_str);
            }
            return value.ToString() == "" ? "" : value.ToString().Substring(0, value.ToString().LastIndexOf(_separator));
        }

        /// <summary>
        /// 判断并转换string输出
        /// </summary>
        /// <param name="item"></param>
        /// <param name="_separator"></param>
        /// <param name="_prefix"></param>
        /// <param name="_suffix"></param>
        /// <param name="isShType"></param>
        /// <returns></returns>
        private static string GetString(object item, string _separator = "", string _prefix = "", string _suffix = "", bool isShType = true)
        {
            StringBuilder value = new StringBuilder();
            if (item is null)
            {
                string str = "null";
                value.Append(_prefix + str + _suffix + _separator);
            }
            else if (item is string)
            {
                string str = ((string)item).TrimEnding();
                value.Append(_prefix + str + "(string)" + _suffix + _separator);
            }
            else if (item.GetType().IsValueType)
            {
                string str = ((ValueType)item).ToString().TrimEnding();
                value.Append(_prefix + str + (isShType ? $"({item.GetType()})" : "") + _suffix + _separator);
            }
            else if (item is Object)
            {
                string str = item.ToString().TrimEnding();
                value.Append(_prefix + str + _suffix + _separator);
            }
            else
            {
                string str = item.GetType().Name;
                value.Append(_prefix + str + ( isShType ? $"({item.GetType()})" : "" ) + _suffix + _separator);
            }

            return value.ToString();
        }

        public static string TrimEnding(this string v)
        {
            if (v == "")
            {
                return "";
            }

            char[] buff = v.ToCharArray();
            if (buff[buff.Length - 1] == '\0')
                buff[buff.Length - 1] = ' ';
            return new string(buff).Trim();
        }
    }
}