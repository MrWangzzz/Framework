using System;
using System.Collections.Generic;
using System.Reflection;

using UnityEngine;
using UnityEngine.UI;
namespace FrameWork
{

    public class DynamicLoad : Attribute
    {
        public string goName { get; set; }
        public DynamicLoad() { }
        public DynamicLoad(string gameObjectName) => goName = gameObjectName;
    }

    public class ViewBase : MonoBehaviour
    {
        /// <summary>
        /// 所有Transform
        /// </summary>
        private List<Transform> transList = new List<Transform>();

        /// <summary>
        /// 主皮肤
        /// </summary>
        private string mainSkinPath;

        /// <summary>
        /// 是否初始化
        /// </summary>
        private bool isInit;

        private GameObject _skin;
        public GameObject skin { get => _skin; }
        public Transform skinTrs { get => _skin.transform; }
        private GameObject m_Canvas;
        public RectTransform M_Canvas { get => m_Canvas.GetComponent<RectTransform>(); }

        #region ButtonEx的事件
        protected virtual void OnClick(Transform target)
        {
            target.GetAudio().SetClip("BtnClick").Play();
        }
        protected virtual void OnDown(Transform target) { }
        protected virtual void OnEnter(Transform target) { }
        protected virtual void onExit(Transform target) { }
        protected virtual void OnUp(Transform target) { }
        protected virtual void onDrag(Transform target) { }
        protected virtual void onBeginDrag(Transform target) { }
        protected virtual void onEndDrag(Transform target) { }
        protected virtual void onRightClick(Transform target) { }
        protected virtual void onMiddleClick(Transform target) { }
        protected virtual void onDoubleClick(Transform target) { }
        #endregion

        #region 初始化
        /// <summary>
        /// 初始化皮肤前
        /// </summary>
        protected virtual void OnInitSkinFront()
        {

        }

        /// <summary>
        /// 初始化皮肤
        /// </summary>
        protected virtual void OnInitSkin()
        {
            if( mainSkinPath != null )
                _skin = LoadSrc(mainSkinPath);
            else
                _skin = new GameObject("Skin");
            skin.transform.SetParent(transform);
            skin.transform.localEulerAngles(0).localScale(1);
            OnDynamicLoad();
        }

        /// <summary>
        /// 初始化前
        /// </summary>
        protected virtual void OnInitFront()
        {
            transList.Clear();
            m_Canvas = GameObject.Find("Canvas");
        }

        protected virtual void OnDynamicLoad()
        {
            try
            {
                var props = this.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                foreach( var prop in props )
                {
                    var attribute = prop.GetCustomAttribute<DynamicLoad>();
                    if( attribute == null )
                        continue;

                    var name = attribute.goName;
                    if( string.IsNullOrEmpty(name) )
                        name = prop.Name;

                    if( prop.PropertyType.IsSubclassOf(typeof(Component)) )
                    {
                        var com = transform.SeachTrs(name, prop.PropertyType);
                        prop.SetValue(this, com);
                    }
                    else
                    {
                        var trans = transform.SeachTrs<Transform>(name);
                        prop.SetValue(this, trans.gameObject);
                    }
                }

                var fields = this.GetType().GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                foreach( var field in fields )
                {
                    var attribute = field.GetCustomAttribute<DynamicLoad>();
                    if( attribute == null )
                        continue;

                    var name = attribute.goName;
                    if( string.IsNullOrEmpty(name) )
                        name = field.Name;

                    if( field.FieldType.IsSubclassOf(typeof(Component)) )
                    {
                        var com = transform.SeachTrs(name, field.FieldType);
                        field.SetValue(this, com);
                    }
                    else
                    {
                        var trans = transform.SeachTrs<Transform>(name);
                        field.SetValue(this, trans.gameObject);
                    }
                }
            }
            catch
            {
                DebugUtil.LogError("查找物体失败", gameObject.name);
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public virtual void Init()
        {
            if( !isInit )
            {
                OnInitFront();
                OnInitSkinFront();
                OnInitSkin();
                Transform[] transforms = this.GetComponentsInChildren<Transform>(true);
                for( int i = 0, max = transforms.Length; i < max; i++ )
                {
                    Transform transform = transforms[i];
                    //如果点按钮没有就是没有初始化 Init()
                    if( transform.name.StartsWith("Btn_") )//以"Btn"开头命名的按钮才会触发OnClick 
                    {
                        if( transform.GetComponent<Button>() )
                        {
                            Button listener = transform.GetComponent<Button>();
                            listener.onClick.AddListener(() => { OnClick(listener.transform); });
                        }
                        else
                        {
                            ButtonEx listener = transform.GetOrAddComponent<ButtonEx>();
                            listener.onLeftClick = (go) => { OnClick(go); };
                            listener.onRightClick = (go) => { onRightClick(go); };
                            listener.onDoubleClick = (go) => { onDoubleClick(go); };
                            listener.onMiddleClick = (go) => { onMiddleClick(go); };
                            listener.onEnter = (go) => { OnEnter(go); };
                            listener.onExit = (go) => { onExit(go); };
                            listener.onUp = (go) => { OnUp(go); };
                            listener.onDown = (go) => { OnDown(go); };
                            listener.onDrag = (go) => { onDrag(go); };
                            listener.onBeginDrag = (go) => { onBeginDrag(go); };
                            listener.onEndDrag = (go) => { onEndDrag(go); };
                        }
                    }
                    transList.Add(transform);
                }
            }
            isInit = true;
        }
        #endregion

        #region 展示
        /// <summary>
        /// 开始显示
        /// </summary>
        public virtual void OnShowing()
        {
        }

        /// <summary>
        /// 显示面板后
        /// </summary>
        public virtual void OnShowed()
        {
        }
        #endregion

        #region 隐藏
        /// <summary>
        /// 开始隐藏
        /// </summary>
        public virtual void OnHideFront()
        {
        }

        /// <summary>
        /// 隐藏完成
        /// </summary>
        public virtual void OnHideDone()
        {
        }
        #endregion

        /// <summary>
        /// 设置主skin
        /// </summary>
        /// <param name="path"></param>
        protected virtual void SetMainSkinPath(string path)
        {
            mainSkinPath = path;
        }

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        protected virtual GameObject LoadSrc(string path)
        {
            return ResourceMgr.GetInstance.CreateGameObject(path, false);
        }
    }
}