
using System;
using System.Collections.Generic;

using UnityEngine;
namespace FrameWork
{
    public class PanelMgr : Singleton<PanelMgr>
    {

        public delegate void OnSwitchingPanel(string type);

        /// <summary>
        /// 当更换场景时委派
        /// </summary>
        public OnSwitchingPanel OnSwitchingPanelHandler;
        /// <summary>
        /// 存储当前已经实例化的场景
        /// </summary>
        public Dictionary<string, PanelBase> panels = new Dictionary<string, PanelBase>();
        /// <summary>
        /// 当前场景
        /// </summary>
        public PanelBase current;
        /// <summary>
        /// 记录切换数据
        /// </summary>
        private List<SwitchRecorder> switchRecoders = new List<SwitchRecorder>();
        /// <summary>
        /// 主场景
        /// </summary>
        private const string mainPanelType = "";

        public void Destroy()
        {
            OnSwitchingPanelHandler = null;

            switchRecoders.Clear();
            switchRecoders = null;

            panels.Clear();
            panels = null;
        }

        /// <summary>
        /// 场景切换
        /// </summary>
        /// <param name="panelType"></param>
        /// <param name="panelArgs">场景参数</param>
        public void SwitchingPanel(string panelType, params object[] panelArgs)
        {
            if( current != null )
            {
                if( panelType == current.name )
                {
                    DebugUtil.LogWarn("试图切换场景至当前场景：", panelType.ToString());
                    return;
                }
            }
            if( panelType == mainPanelType )//进入主场景，把切换场景记录清空
            {
                switchRecoders.Clear();
            }
            switchRecoders.Add(new SwitchRecorder(panelType, panelArgs));//切换记录
            HideCurrentPanel();
            ShowPanel(panelType, panelArgs);
            OnSwitchingPanelHandler?.Invoke(panelType);
        }

        /// <summary>
        /// 切换至上一个场景
        /// </summary>
        public void SwitchingToPrevPanel()
        {
            if( switchRecoders.Count < 2 )
            {
                DebugUtil.LogWarn("Tip", "切换至上一个场景时，没有上一个场景记录！请检查逻辑!");
                return;
            }
            SwitchRecorder sr = switchRecoders[switchRecoders.Count - 2];
            switchRecoders.RemoveRange(switchRecoders.Count - 2, 2);//切换至上一个场景后，记录请除最后一个场景（即当前场景）和上一场景
            SwitchingPanel(sr.panelType, sr.panelArgs);
        }

        /// <summary>
        /// 打开指定场景
        /// </summary>
        /// <param name="panelType"></param>
        /// <param name="panelArgs">场景参数</param>
        private void ShowPanel(string panelType, params object[] panelArgs)
        {
            if( panels.ContainsKey(panelType) )
            {
                current = panels[panelType];
                current.OnShowing();
                current.OnResetArgs(panelArgs);
                current.gameObject.SetActive(true);
                current.OnShowed();
            }
            else
            {
                if( panelType =="" )
                {
                    current = null;
                    return;
                }
                GameObject go = new GameObject(panelType.ToString());
                Type mType = Type.GetType("FrameWork." + panelType.ToString());
                current = go.AddComponent(mType) as PanelBase; //PanelType.tostring等于该场景的classname
                current.OnInit(panelArgs);
                panels.Add(current.name, current);
                current.OnShowing();
                LayerMgr.GetInstance.SetLayer(current.gameObject, LayerType.Panel);
                go.transform.localPosition(Vector3.zero).localRotation(Quaternion.identity).localScale(1);
                current.OnShowed();
            }
        }

        /// <summary>
        /// 关闭当前场景
        /// </summary>
        private void HideCurrentPanel()
        {
            if( current != null )
            {
                current.OnHideFront();
                //NGUITools.SetActive(current.gameObject, false);
                current.OnHideDone();
                if( !current.cache )
                {
                    panels.Remove(current.name);
                    GameObject.Destroy(current.gameObject);
                }
            }
        }

        /// <summary>
        /// 记录
        /// </summary>
        internal struct SwitchRecorder
        {
            internal string panelType;
            internal object[] panelArgs;

            internal SwitchRecorder(string panelType, params object[] panelArgs)
            {
                this.panelType = panelType;
                this.panelArgs = panelArgs;
            }
        }
    }
}