using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace FrameWork
{
    public class LayerMgr : MonoBehaviour
    {
        private static LayerMgr mInstance;
        private int lastSortingOrder;
        /// <summary>
        /// 获取资源加载实例
        /// </summary>
        /// <returns></returns>
        public static LayerMgr GetInstance
        {
            get
            {
                if (mInstance == null)
                {
                    mInstance = new GameObject("_LayerMgr").AddComponent<LayerMgr>();
                }
                return mInstance;
            }
        }

        private LayerMgr()
        {
            mLayerDic = new Dictionary<LayerType, GameObject>();
        }

        public Dictionary<LayerType, GameObject> mLayerDic;
        private GameObject mParent;

        public void LayerInit()
        {
            mParent = GameObject.Find("Canvas");
            if (mParent == null)
            {
                DebugUtil.LogError("Tip","场景中不存在Canvas ,请创建！");
            }
            //获取一个枚举的个数长度
            int nums = Enum.GetNames(typeof(LayerType)).Length;
            for (int i = 0; i < nums; i++)
            {
                //获取枚举的索引位置的值
                object obj = Enum.GetValues(typeof(LayerType)).GetValue(i);
                mLayerDic.Add((LayerType)obj, CreateLayerGameObject(obj.ToString(), (LayerType)obj));
            }
        }

        private GameObject CreateLayerGameObject(string name, LayerType type)
        {
            GameObject layer = new GameObject(name);
            layer.transform.parent = mParent.transform;
            layer.GetOrAddComponent<GraphicRaycaster>();
            Canvas canvas = layer.GetOrAddComponent<Canvas>();
            canvas.overrideSorting = true;
            canvas.sortingOrder = (int)type;
            layer.transform.localPosition(0).localEulerAngles(0).localScale(1);
            return layer;
        }

        public void SetLayer(GameObject current, LayerType type)
        {
            if (mLayerDic.Count < Enum.GetNames(typeof(LayerType)).Length)
            {
                LayerInit();
            }
            current.transform.SetParent(mLayerDic[type].transform);

            Canvas canvas = current.GetOrAddComponent<Canvas>();
            current.GetOrAddComponent<GraphicRaycaster>();
            canvas.overrideSorting = true;
            canvas.sortingOrder = 1;

            Canvas[] panelArr = current.GetComponentsInChildren<Canvas>(true);
            foreach( Canvas panel in panelArr )
            {
                panel.sortingOrder += (int) type;
                if( type == LayerType.Dialog )
                {
                    lastSortingOrder = Mathf.Max(panel.sortingOrder, lastSortingOrder);
                    if( panel.name.Equals("DialogMask") )
                        panel.sortingOrder = canvas.sortingOrder - 1;
                    else
                        panel.sortingOrder = lastSortingOrder;
                }
                DebugUtil.Log("lastSortingOrder", $"{ panel.name }----{ panel.sortingOrder}");
                Renderer renderer = panel.GetComponent<Renderer>();//设置粒子的层级
                if( renderer != null )
                    renderer.sortingOrder = panel.sortingOrder;
            }
            if( type == LayerType.Dialog )
                lastSortingOrder += 10;

        }

        /// <summary>根据面板数组先后顺序设置深度 最后一个Dialog深度最高</summary>
        public void SetDialogsLayer(List<DialogBase> pbList)
        {

        }

        public void ClearLayer(LayerType type = LayerType.Dialog, bool isSet = true)
        {
            if( type == LayerType.Dialog )
                lastSortingOrder = isSet ? 0 : lastSortingOrder - 10;
        }
    }
    /// <summary>
    /// 分层类型
    /// </summary>
    public enum LayerType
    {
        /// <summary>场景</summary>
        Panel = 50,
        /// <summary>弹框</summary>
        Dialog = 200,
        /// <summary>提示</summary>
        Tips = 800,
        /// <summary>公告层</summary>
        Notice = 1000,
    }
}