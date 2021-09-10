using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolMrg : MonoBehaviour
{
    #region 初始化
    private static ObjectPoolMrg mInstance;

    /// <summary>
    /// 获取对象池实例
    /// </summary>
    /// <returns></returns>
    public static ObjectPoolMrg GetInstance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = new GameObject("_ObjectPoolMrg").AddComponent<ObjectPoolMrg>();
            }
            return mInstance;
        }

    }

    #endregion
    /// <summary>
    /// 出池
    /// </summary>
    /// <param name="ObjName"></param>
    /// <returns></returns>
    public GameObject OutPool(string ObjName) 
    {
        for (int i = 0; i < transform.childCount; i++) 
        {
            if (ObjName == transform.GetChild(i).name) 
            {
                GameObject gameObject = transform.GetChild(i).gameObject;
                gameObject.SetActive(true);
                return gameObject;
            }
        }
        return null;
    }
    /// <summary>
    /// 入池
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="name"></param>
    public void EnterPool(GameObject gameObject, string name=null) 
    {
        gameObject.SetActive(false);
        if (name != null) 
        {
            gameObject.name = name;
        }
        gameObject.transform.SetParent(transform);
    }
 
}
