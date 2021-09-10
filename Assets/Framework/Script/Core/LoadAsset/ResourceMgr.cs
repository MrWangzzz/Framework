using System.Collections;

using UnityEngine;
namespace FrameWork
{
    public class ResourceMgr : MonoBehaviour
    {
        #region 初始化
        private static ResourceMgr mInstance;

        /// <summary>
        /// 获取资源加载实例
        /// </summary>
        /// <returns></returns>
        public static ResourceMgr GetInstance
        {
            get
            {
                if (mInstance == null)
                {
                    mInstance = new GameObject("_ResourceMgr").AddComponent<ResourceMgr>();
                }
                return mInstance;
            }

        }
        private ResourceMgr()
        {
            hashtable = new Hashtable();
        }
        #endregion

        /// <summary> 资源缓存容器 </summary>
        private Hashtable hashtable;

        /// <summary>
        /// Load 资源
        /// </summary>
        /// <typeparam name="T">资源类型</typeparam>
        /// <param name="path">资源路径</param>
        /// <param name="cacheAsset">是否要缓存资源</param>
        /// <returns></returns>
        public T Load<T>(string path, bool cache) where T : UnityEngine.Object
        {
            if (hashtable.Contains(path))
            {
                return hashtable[path] as T;
            }

            T assetObj = Resources.Load<T>(path);
            if (assetObj == null)
            {
                DebugUtil.LogWarn("Resources中找不到资源：", path);
            }
            if (cache)
            {
                hashtable.Add(path, assetObj);
                DebugUtil.Log("Asset对象被缓存,Resource'path=", path);
            }
            return assetObj;
        }


        /// <summary>
        /// 创建Resource中GameObject对象
        /// </summary>
        /// <param name="path"资源路径</param>
        /// <param name="cacheAsset">是否要缓存Asset对象</param>
        /// <returns></returns>
        public GameObject CreateGameObject(string path, bool cache)
        {
            GameObject assetObj = Load<GameObject>(path, cache);
            GameObject go = Instantiate(assetObj) as GameObject;
            if (go == null)
            {
                DebugUtil.LogWarn("从Resource创建对象失败：", path);
            }
            go.name = go.name.Replace("(Clone)", "");
            return go;
        }

        public Transform CreateTransform(string path, bool cache)
        {
            Transform assetObj = Load<Transform>(path, cache);
            Transform go = Instantiate(assetObj) as Transform;
            if (go == null)
            {
                DebugUtil.LogWarn("从Resource创建对象失败：" , path);
            }
            go.name = go.name.Replace("(Clone)", "");
            return go;
        }
    }
}