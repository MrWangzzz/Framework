using System;

using UnityEngine;
namespace FrameWork
{
    /// <summary>
    /// 
    /// </summary>
    public static class ComponentEx
    {
        /// <summary>
        /// 根据名称查找物体 默认返回查找到的第一个
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="transform">父物体</param>
        /// <param name="name">名称</param>
        /// <returns></returns>
        public static T SeachTrs<T>(this Component transform, string name) where T : Component
        {
            T[] trs = transform.GetComponentsInChildren<T>(true);
            for (int i = 0; i < trs.Length; i++)
            {
                if (trs[i].name == name)
                    return trs[i];
            }
            return null;
        }

        /// <summary>
        /// 根据名称查找物体 默认返回查找到的第一个
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="transform">父物体</param>
        /// <param name="name">名称</param>
        /// <returns></returns>
        public static T SeachTrs<T>(this GameObject transform, string name) where T : Component
        {
            T[] trs = transform.GetComponentsInChildren<T>(true);
            for (int i = 0; i < trs.Length; i++)
            {
                if (trs[i].name == name)
                    return trs[i];
            }
            return null;
        }

        /// <summary>
        /// /// 根据名称查找物体 默认返回查找到的第一个
        /// </summary>
        /// <param name="transform">父物体</param>
        /// <param name="name">名称</param>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public static Component SeachTrs(this Component transform, string name, Type type)
        {
            Component[] trs = transform.GetComponentsInChildren(type, true);
            for (int i = 0; i < trs.Length; i++)
            {
                if (trs[i].name == name)
                    return trs[i];
            }

            return null;
        }

        /// <summary>
        /// 物体获取组件 为null则自动添加
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="transform">父物体</param>
        /// <returns></returns>
        public static T GetOrAddComponent<T>(this Component transform) where T : Component
        {
            T t = transform.GetComponent<T>();
            if (t == null)
                t = transform.gameObject.AddComponent<T>();
            return t;
        }

        /// <summary>
        /// 物体获取组件 为null则自动添加
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="gameObject">父物体</param>
        /// <returns></returns>
        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
        {
            //T t = gameObject.GetComponent<T>();
            //if (t == null)
            //    t = gameObject.AddComponent<T>();
            //return t;
            return gameObject.transform.GetOrAddComponent<T>();
        }

        /// <summary>
        /// 位置信息
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="selfComponent">该物体</param>
        /// <param name="position"></param>
        /// <returns>position</returns>
        public static T position<T>(this T selfComponent, Vector3 position) where T : Component
        {
            selfComponent.transform.position = position;
            return selfComponent;
        }

        /// <summary>
        /// 世界位置信息
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="selfComponent">该物体</param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns> new Vector3(x, y, z)</returns>
        public static T position<T>(this T selfComponent, float x, float y, float z) where T : Component
        {
            selfComponent.transform.position = new Vector3(x, y, z);
            return selfComponent;
        }

        /// <summary>
        /// 世界位置信息
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="selfComponent">该物体</param>
        /// <param name="xyz"></param>
        /// <returns> Vector3.one * xyz </returns>
        public static T position<T>(this T selfComponent, float xyz) where T : Component
        {
            selfComponent.transform.position = Vector3.one * xyz;
            return selfComponent;
        }

        /// <summary>
        /// 世界位置信息
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="selfComponent">该物体</param>
        /// <returns> selfComponent.transform.position </returns>
        public static Vector3 position<T>(this T selfComponent) where T : Component
        {
            return selfComponent.transform.position;
        }

        /// <summary>
        /// 世界角度信息
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="selfComponent">该物体</param>
        /// <param name="rotation"></param>
        /// <returns> rotation </returns>
        public static T rotation<T>(this T selfComponent, Quaternion rotation) where T : Component
        {
            selfComponent.transform.rotation = rotation;
            return selfComponent;
        }

        /// <summary>
        /// 世界角度信息
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="selfComponent">该物体</param>
        /// <returns> selfComponent.transform.rotation </returns>
        public static Quaternion rotation<T>(this T selfComponent) where T : Component
        {
            return selfComponent.transform.rotation;
        }

        /// <summary>
        /// 世界角度信息
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="selfComponent">该物体</param>
        /// <param name="eulerAngles"></param>
        /// <returns> eulerAngles </returns>
        public static T eulerAngles<T>(this T selfComponent, Vector3 eulerAngles) where T : Component
        {
            selfComponent.transform.eulerAngles = eulerAngles;
            return selfComponent;
        }

        /// <summary>
        /// 世界角度信息
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="selfComponent">该物体</param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns> new Vector3(x, y, z) </returns>
        public static T eulerAngles<T>(this T selfComponent, float x, float y, float z) where T : Component
        {
            selfComponent.transform.eulerAngles = new Vector3(x, y, z);
            return selfComponent;
        }

        /// <summary>
        /// 世界角度信息
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="selfComponent">该物体</param>
        /// <returns> selfComponent.transform.localEulerAngles </returns>
        public static Vector3 eulerAngles<T>(this T selfComponent) where T : Component
        {
            return selfComponent.transform.eulerAngles;
        }

        /// <summary>
        /// 自身缩放信息
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="selfComponent">该物体</param>
        /// <param name="xyz"></param>
        /// <returns> Vector3.one * xyz </returns>
        public static T localScale<T>(this T selfComponent, float xyz) where T : Component
        {
            selfComponent.transform.localScale = Vector3.one * xyz;
            return selfComponent;
        }

        /// <summary>
        /// 自身缩放信息
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="selfComponent">该物体</param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns> new Vector3(x, y, z) </returns>
        public static T localScale<T>(this T selfComponent, float x, float y, float z) where T : Component
        {
            selfComponent.transform.localScale = new Vector3(x, y, z);
            return selfComponent;
        }

        /// <summary>
        /// 自身缩放信息
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="selfComponent">该物体</param>
        /// <param name="scale"></param>
        /// <returns> scale </returns>
        public static T localScale<T>(this T selfComponent, Vector3 scale) where T : Component
        {
            selfComponent.transform.localScale = scale;
            return selfComponent;
        }

        /// <summary>
        /// 自身位置信息
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="selfComponent">该物体</param>
        /// <param name="position"></param>
        /// <returns> position </returns>
        public static T localPosition<T>(this T selfComponent, Vector3 position) where T : Component
        {
            selfComponent.transform.localPosition = position;
            return selfComponent;
        }

        /// <summary>
        /// 自身位置信息
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="selfComponent">该物体</param>
        /// <param name="xyz"></param>
        /// <returns> Vector3.one *xyz </returns>
        public static T localPosition<T>(this T selfComponent, float xyz) where T : Component
        {
            selfComponent.transform.localPosition = Vector3.one * xyz;
            return selfComponent;
        }

        /// <summary>
        /// 自身位置信息
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="selfComponent">该物体</param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns> new Vector3(x, y, z) </returns>
        public static T localPosition<T>(this T selfComponent, float x, float y, float z) where T : Component
        {
            selfComponent.transform.localPosition = new Vector3(x, y, z);
            return selfComponent;
        }

        /// <summary>
        /// 自身位置信息
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="selfComponent">该物体</param>
        /// <returns> selfComponent.transform.localPosition </returns>
        public static Vector3 localPosition<T>(this T selfComponent) where T : Component
        {
            return selfComponent.transform.localPosition;
        }

        /// <summary>
        /// 自身角度信息
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="selfComponent">该物体</param>
        /// <param name="rotation"></param>
        /// <returns> rotation </returns>
        public static T localRotation<T>(this T selfComponent, Quaternion rotation) where T : Component
        {
            selfComponent.transform.localRotation = rotation;
            return selfComponent;
        }

        /// <summary>
        /// 自身角度信息
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="selfComponent">该物体</param>
        /// <returns> selfComponent.transform.localRotation </returns>
        public static Quaternion localRotation<T>(this T selfComponent) where T : Component
        {
            return selfComponent.transform.localRotation;
        }

        /// <summary>
        /// 自身角度信息
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="selfComponent">该物体</param>
        /// <param name="eulerAngles"></param>
        /// <returns> eulerAngles </returns>
        public static T localEulerAngles<T>(this T selfComponent, Vector3 eulerAngles) where T : Component
        {
            selfComponent.transform.localEulerAngles = eulerAngles;
            return selfComponent;
        }

        /// <summary>
        /// 自身角度信息
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="selfComponent">该物体</param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns> new Vector3(x, y, z) </returns>
        public static T localEulerAngles<T>(this T selfComponent, float x, float y, float z) where T : Component
        {
            selfComponent.transform.localEulerAngles = new Vector3(x, y, z);
            return selfComponent;
        }


        /// <summary>
        /// 自身角度信息
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="selfComponent">该物体</param>
        /// <param name="xyz"></param>
        /// <returns>  Vector3.one *xyz </returns>
        public static T localEulerAngles<T>(this T selfComponent, float xyz) where T : Component
        {
            selfComponent.transform.localEulerAngles = Vector3.one * xyz;
            return selfComponent;
        }

        /// <summary>
        /// 自身角度信息
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="selfComponent">该物体</param>
        /// <returns> selfComponent.transform.localEulerAngles </returns>
        public static Vector3 localEulerAngles<T>(this T selfComponent) where T : Component
        {
            return selfComponent.transform.localEulerAngles;
        }

        /// <summary>
        /// UI坐标信息
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="selfComponent">该物体</param>
        /// <param name="position"></param>
        /// <returns> position </returns>
        public static T anchoredPosition3D<T>(this T selfComponent, Vector3 position) where T : Component
        {
            selfComponent.GetComponent<RectTransform>().anchoredPosition3D = position;
            return selfComponent;
        }

        /// <summary>
        /// UI坐标信息
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="selfComponent">该物体</param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns> new Vector3(x, y, z) </returns>
        public static T anchoredPosition3D<T>(this T selfComponent, float x, float y, float z) where T : Component
        {
            selfComponent.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(x, y, z);
            return selfComponent;
        }

        /// <summary>
        /// UI坐标信息
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="selfComponent">该物体</param>
        /// <returns> anchoredPosition3D </returns>
        public static Vector3 anchoredPosition3D<T>(this T selfComponent) where T : Component
        {
            return selfComponent.GetComponent<RectTransform>().anchoredPosition3D;
        }

        /// <summary>
        /// UI坐标信息
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="selfComponent">该物体</param>
        /// <param name="position"></param>
        /// <returns> position </returns>
        public static T anchoredPosition<T>(this T selfComponent, Vector2 position) where T : Component
        {
            selfComponent.GetComponent<RectTransform>().anchoredPosition = position;
            return selfComponent;
        }

        /// <summary>
        /// UI坐标信息
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="selfComponent">该物体</param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns> new Vector2(x, y) </returns>
        public static T anchoredPosition<T>(this T selfComponent, float x, float y) where T : Component
        {
            selfComponent.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);
            return selfComponent;
        }

        /// <summary>
        /// UI坐标信息
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="selfComponent">该物体</param>
        /// <returns> anchoredPosition </returns>
        public static Vector2 anchoredPosition<T>(this T selfComponent) where T : Component
        {
            return selfComponent.GetComponent<RectTransform>().anchoredPosition;
        }

        /// <summary>
        /// UI尺寸大小
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="selfComponent">该物体</param>
        /// <param name="sizeDelta"></param>
        /// <returns> sizeDelta </returns>
        public static T sizeDelta<T>(this T selfComponent, Vector2 sizeDelta) where T : Component
        {
            selfComponent.GetComponent<RectTransform>().sizeDelta = sizeDelta;
            return selfComponent;
        }

        /// <summary>
        /// UI尺寸大小
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="selfComponent">该物体</param>
        /// <param name="xy"></param>
        /// <returns> sizeDelta *= xy </returns>
        public static T sizeDelta<T>(this T selfComponent, float xy) where T : Component
        {
            selfComponent.GetComponent<RectTransform>().sizeDelta *= xy;
            return selfComponent;
        }

        /// <summary>
        /// UI尺寸大小
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="selfComponent">该物体</param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns> new Vector2(x, y) </returns>
        public static T sizeDelta<T>(this T selfComponent, float x, float y) where T : Component
        {
            selfComponent.GetComponent<RectTransform>().sizeDelta = new Vector2(x, y);
            return selfComponent;
        }

        /// <summary>
        /// UI尺寸大小
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="selfComponent">该物体</param>
        /// <returns> sizeDelta </returns>
        public static Vector2 sizeDelta<T>(this T selfComponent) where T : Component
        {
            return selfComponent.GetComponent<RectTransform>().sizeDelta;
        }

        /// <summary>
        /// 锚点在四角时设置
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="selfComponent">该物体</param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns> new Vector2(x, y) </returns>
        public static T offsetMax<T>(this T selfComponent, float x, float y) where T : Component
        {
            selfComponent.GetComponent<RectTransform>().offsetMax = new Vector2(x, y);
            return selfComponent;
        }

        /// <summary>
        /// UI右上角的位置信息
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="selfComponent">该物体</param>
        /// <param name="xy"></param>
        /// <returns> offsetMax *= xy </returns>
        public static T offsetMax<T>(this T selfComponent, float xy) where T : Component
        {
            selfComponent.GetComponent<RectTransform>().offsetMax *= xy;
            return selfComponent;
        }

        /// <summary>
        /// UI右上角的位置信息
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="selfComponent">该物体</param>
        /// <param name="offsetMax"></param>
        /// <returns> offsetMax </returns>
        public static T offsetMax<T>(this T selfComponent, Vector2 offsetMax) where T : Component
        {
            selfComponent.GetComponent<RectTransform>().offsetMax = offsetMax;
            return selfComponent;
        }

        /// <summary>
        /// UI右上角的位置信息
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="selfComponent">该物体</param>
        /// <returns> offsetMax </returns>
        public static Vector2 offsetMax<T>(this T selfComponent) where T : Component
        {
            return selfComponent.GetComponent<RectTransform>().offsetMax;
        }

        /// <summary>
        /// UI左下角的位置信息
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="selfComponent">该物体</param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns> new Vector2(x, y) </returns>
        public static T offsetMin<T>(this T selfComponent, float x, float y) where T : Component
        {
            selfComponent.GetComponent<RectTransform>().offsetMin = new Vector2(x, y);
            return selfComponent;
        }

        /// <summary>
        /// UI左下角的位置信息
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="selfComponent">该物体</param>
        /// <param name="xy"></param>
        /// <returns> offsetMin *= xy </returns>
        public static T offsetMin<T>(this T selfComponent, float xy) where T : Component
        {
            selfComponent.GetComponent<RectTransform>().offsetMin *= xy;
            return selfComponent;
        }

        /// <summary>
        /// UI左下角的位置信息
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="selfComponent">该物体</param>
        /// <param name="offsetMin"></param>
        /// <returns> offsetMin </returns>
        public static T offsetMin<T>(this T selfComponent, Vector2 offsetMin) where T : Component
        {
            selfComponent.GetComponent<RectTransform>().offsetMin = offsetMin;
            return selfComponent;
        }

        /// <summary>
        /// UI左下角的位置信息
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="selfComponent">该物体</param>
        /// <returns> offsetMin </returns>
        public static Vector2 offsetMin<T>(this T selfComponent) where T : Component
        {
            return selfComponent.GetComponent<RectTransform>().offsetMin;
        }

    }
}