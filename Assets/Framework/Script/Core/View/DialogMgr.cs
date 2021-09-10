using System;
using System.Collections.Generic;

using DG.Tweening;

using UnityEngine;
using UnityEngine.UI;

using Random = UnityEngine.Random;

namespace FrameWork
{
    /// <summary>
    /// 面板遮罩
    /// </summary>
    public enum DialogMaskSytle
    {
        /// <summary>
        /// 周围不可关闭
        /// </summary>
        None,
        /// <summary>
        /// 周围点击可关闭
        /// </summary>
        Open,
    }

    #region DialogShowStyle 窗口打开方式
    /// <summary>
    /// 窗口打开方式
    /// </summary>
    public enum DialogShowStyle
    {
        /// <summary>
        /// 正常打开
        /// </summary>
        Normal,

        /// <summary>
        /// 正常打开透明
        /// </summary>
        NormalAlpha,

        /// <summary>
        /// 从中间放大
        /// </summary>
        CenterToBig,

        /// <summary>
        /// 从中间放大透明
        /// </summary>
        CenterToBigAlpha,

        /// <summary>
        /// 从某点放大
        /// </summary>
        SomeplaceToBig,

        /// <summary>
        /// 从某点放大透明
        /// </summary>
        SomeplaceToBigAlpha,

        /// <summary>
        /// 从中间旋转放大
        /// </summary>
        CenterToBigRotate,

        /// <summary>
        /// 从中间旋转放大透明
        /// </summary>
        CenterToBigRotateAlpha,

        /// <summary>
        /// 从上往下
        /// </summary>
        FromTop,

        /// <summary>
        /// 从上往下透明
        /// </summary>
        FromTopAlpha,

        /// <summary>
        /// 从下往上
        /// </summary>
        FromDown,

        /// <summary>
        /// 从下往上透明
        /// </summary>
        FromDownAlpha,

        /// <summary>
        /// 从左向右
        /// </summary>
        FromLeft,

        /// <summary>
        /// 从左向右透明
        /// </summary>
        FromLeftAlpha,

        /// <summary>
        /// 从右向左
        /// </summary>
        FromRight,

        /// <summary>
        /// 从右向左透明
        /// </summary>
        FromRightAlpha,

        /// <summary>
        /// 从上往下旋转
        /// </summary>
        FromTopRotate,

        /// <summary>
        /// 从上往下旋转透明
        /// </summary>
        FromTopRotateAlpha,

        /// <summary>
        /// 从下往上旋转
        /// </summary>
        FromDownRotate,

        /// <summary>
        /// 从下往上旋转透明
        /// </summary>
        FromDownRotateAlpha,

        /// <summary>
        /// 从左向右旋转
        /// </summary>
        FromLeftRotate,

        /// <summary>
        /// 从左向右旋转透明
        /// </summary>
        FromLeftRotateAlpha,

        /// <summary>
        /// 从右向左旋转
        /// </summary>
        FromRightRotate,

        /// <summary>
        /// 从右向左旋转透明
        /// </summary>
        FromRightRotateAlpha,

        /// <summary>
        /// 自定义个性化
        /// </summary>
        Custom,
    }
    #endregion


    /// <summary>
    /// 面板管理
    /// </summary>
    public class DialogMgr : Singleton<DialogMgr>
    {
        #region 初始化
        public static bool cache { get; set; } = false;

        public class CacheClass
        {
            public string dialogName;
            public object[] objectValue;

            public CacheClass(string _dialogName, object[] _object)
            {
                dialogName = _dialogName;
                objectValue = _object;
            }
            private CacheClass() { }
        }

        public bool isShowDialog(string dialogName)
        {
            return dialogs.ContainsKey(dialogName);
        }

        #endregion

        #region 数据定义

        /// <summary>
        /// 存储当前已经实例化的面板
        /// </summary>
        public Dictionary<string, DialogBase> dialogs = new Dictionary<string, DialogBase>();

        /// <summary>
        /// 存储当前已经实例化的面板
        /// </summary>
        public List<CacheClass> cacheDialog = new List<CacheClass>();

        /// <summary> 深度列表 </summary>
        public List<DialogBase> dialogsDethList = new List<DialogBase>();
        #endregion


        /// <summary>
        /// 当前面板
        /// </summary>
        public DialogBase current;

        public void Destroy()
        {

        }

        /// <summary>
        /// 打开指定弹框
        /// </summary>
        /// <param name="dialogName"></param>
        /// <param name="dialogArgs">场景参数</param>
        public void ShowDialog(string dialogName, params object[] dialogArgs)
        {
            if( cache && dialogs.Count > 0 )
            {
                if( !dialogs.ContainsKey(dialogName) )
                {
                    for( int i = 0; i < cacheDialog.Count; i++ )
                        if( cacheDialog[i].dialogName == dialogName )
                            return;
                    cacheDialog.Add(new CacheClass(dialogName, dialogArgs));
                }
            }
            else
            {
                if( dialogs.ContainsKey(dialogName) )
                {
                    DebugUtil.LogError("面板已打开", dialogName);
                    current = dialogs[dialogName];
                    current.gameObject.SetActive(false);
                    current.OnInit(dialogArgs);
                    current.OnShowing();
                    LayerMgr.GetInstance.SetLayer(current.gameObject, LayerType.Dialog);
                }
                else
                {
                    GameObject go = new GameObject(dialogName.ToString());

                    Type mType = Type.GetType("FrameWork." + dialogName.ToString());
                    DialogBase pb = go.AddComponent(mType) as DialogBase;
                    pb.OnInit(dialogArgs);
                    MaskStyle(pb);
                    current = pb;
                    dialogs.Add(pb.name, pb);
                    dialogsDethList.Add(pb);
                    ChangeDialogDeth();
                    pb.OnShowing();
                    pb.skinTrs.SetAsLastSibling();
                    LayerMgr.GetInstance.SetLayer(go.gameObject, LayerType.Dialog);
                    go.transform.localPosition(0).localScale(1).localEulerAngles(0);
                }
                if( current.showStyle == DialogShowStyle.Custom )
                    current.OnCustomShow(true);
                else
                    StartShowDialog(current, current.showStyle, true);
            }
        }

        /// <summary> 关闭所有面板 </summary>
        public void CloseAllDialog()
        {
            Dictionary<string, DialogBase>.ValueCollection vs = dialogs.Values;
            foreach( DialogBase item in vs )
            {
                if( current.showStyle == DialogShowStyle.Custom )
                    current.OnCustomShow(false);
                else
                    StartShowDialog(item, item.showStyle, false);
            }
            dialogsDethList.Clear();
            LayerMgr.GetInstance.ClearLayer();
        }

        /// <summary> 打开关闭面板效果 </summary>
        private void StartShowDialog(DialogBase go, DialogShowStyle showStyle, bool isOpen)
        {
            switch( showStyle )
            {
                case DialogShowStyle.Normal:
                    ShowNomal(go, isOpen, false);
                    break;
                case DialogShowStyle.NormalAlpha:
                    ShowNomal(go, isOpen, true);
                    break;
                case DialogShowStyle.CenterToBig:
                    CenterScaleBigNomal(go, isOpen, false);
                    break;
                case DialogShowStyle.CenterToBigAlpha:
                    CenterScaleBigNomal(go, isOpen, true);
                    break;
                case DialogShowStyle.SomeplaceToBig:
                    SomeplaceToSlide(go, isOpen, false);
                    break;
                case DialogShowStyle.SomeplaceToBigAlpha:
                    SomeplaceToSlide(go, isOpen, true);
                    break;
                case DialogShowStyle.FromTop:
                    TopAndDownToSlide(go, true, isOpen, false);
                    break;
                case DialogShowStyle.FromTopAlpha:
                    TopAndDownToSlide(go, true, isOpen, true);
                    break;
                case DialogShowStyle.FromDown:
                    TopAndDownToSlide(go, false, isOpen, false);
                    break;
                case DialogShowStyle.FromDownAlpha:
                    TopAndDownToSlide(go, false, isOpen, true);
                    break;
                case DialogShowStyle.FromLeft:
                    LeftAndRightToSlide(go, false, isOpen, false);
                    break;
                case DialogShowStyle.FromLeftAlpha:
                    LeftAndRightToSlide(go, false, isOpen, true);
                    break;
                case DialogShowStyle.FromRight:
                    LeftAndRightToSlide(go, true, isOpen, false);
                    break;
                case DialogShowStyle.FromRightAlpha:
                    LeftAndRightToSlide(go, true, isOpen, true);
                    break;
                case DialogShowStyle.CenterToBigRotate:
                    CenterScaleBigRotateNomal(go, isOpen, go.isClockwise, false);
                    break;
                case DialogShowStyle.CenterToBigRotateAlpha:
                    CenterScaleBigRotateNomal(go, isOpen, go.isClockwise, true);
                    break;
                case DialogShowStyle.FromTopRotate:
                    TopAndDownRotateToSlide(go, true, go.isClockwise, isOpen, true);
                    break;
                case DialogShowStyle.FromTopRotateAlpha:
                    TopAndDownRotateToSlide(go, true, go.isClockwise, isOpen, true);
                    break;
                case DialogShowStyle.FromDownRotate:
                    TopAndDownRotateToSlide(go, false, go.isClockwise, isOpen, true);
                    break;
                case DialogShowStyle.FromDownRotateAlpha:
                    TopAndDownRotateToSlide(go, false, go.isClockwise, isOpen, true);
                    break;
                case DialogShowStyle.FromLeftRotate:
                    LeftAndRightRotateToSlide(go, false, go.isClockwise, isOpen, false);
                    break;
                case DialogShowStyle.FromLeftRotateAlpha:
                    LeftAndRightRotateToSlide(go, false, go.isClockwise, isOpen, true);
                    break;
                case DialogShowStyle.FromRightRotate:
                    LeftAndRightRotateToSlide(go, true, go.isClockwise, isOpen, false);
                    break;
                case DialogShowStyle.FromRightRotateAlpha:
                    LeftAndRightRotateToSlide(go, true, go.isClockwise, isOpen, true);
                    break;
                case DialogShowStyle.Custom:
                    break;
            }
        }

        #region 显示方式

        #region 默认显示
        /// <summary>
        /// 默认显示
        /// </summary>
        /// <param name="go"></param>
        /// <param name="isOpen"></param>
        private void ShowNomal(DialogBase go, bool isOpen, bool isAlpha)
        {

            Action action = () =>
            {
                if( isOpen )
                {
                    current.gameObject.SetActive(true);
                    current.OnShowed();
                }
                else
                    DestroyDialog(go.name);
            };
            if( !isAlpha )
                action?.Invoke();
            else
                OpenAlpha(go, isOpen, action);
        }
        #endregion

        #region 中间变大
        /// <summary>
        /// 中间变大
        /// </summary>
        /// <param name="go"></param>
        /// <param name="isOpen"></param>
        private void CenterScaleBigNomal(DialogBase go, bool isOpen, bool isAlpha)
        {

            Action action = () =>
            {
                if( isOpen )
                    go.OnShowed();
                else
                    DestroyDialog(go.name);
            };

            OpenScale(go, isOpen, action);
            if( isAlpha ) OpenAlpha(go, isOpen);
        }
        #endregion

        #region 左右往中
        /// <summary>
        /// 左右往中
        /// </summary>
        /// <param name="go"></param>
        /// <param name="isRight"></param>
        /// <param name="isOpen"></param>
        private void LeftAndRightToSlide(DialogBase go, bool isRight, bool isOpen, bool isAlpha)
        {
            Vector3 fromPos = isRight ? new Vector3(1000, 0, 0) : new Vector3(-1000, 0, 0);
            OpenPosition(go, isOpen, fromPos);
            if( isAlpha ) OpenAlpha(go, isOpen);
        }
        #endregion

        #region 上下往中
        /// <summary>
        /// 上下往中
        /// </summary>
        /// <param name="go"></param>
        /// <param name="isTop"></param>
        /// <param name="isOpen"></param>
        private void TopAndDownToSlide(DialogBase go, bool isTop, bool isOpen, bool isAlpha)
        {
            Vector3 fromPos = isTop ? new Vector3(0, 1000, 0) : new Vector3(0, -1000, 0);
            OpenPosition(go, isOpen, fromPos);
            if( isAlpha ) OpenAlpha(go, isOpen);
        }
        #endregion

        #region 中间变大旋转
        /// <summary>
        /// 中间变大旋转
        /// </summary>
        /// <param name="go"></param>
        /// <param name="isOpen"></param>
        private void CenterScaleBigRotateNomal(DialogBase go, bool isOpen, bool isClockwise, bool isAlpha)
        {

            Action action = () =>
            {
                if( isOpen )
                    go.OnShowed();
                else
                    DestroyDialog(go.name);
            };

            OpenScale(go, isOpen, action);
            OpenRotate(go, isOpen, isClockwise);
            if( isAlpha ) OpenAlpha(go, isOpen);
        }
        #endregion

        #region 左右往中旋转
        /// <summary>
        /// 左右往中旋转
        /// </summary>
        /// <param name="go"></param>
        /// <param name="isTop"></param>
        /// <param name="isOpen"></param>
        private void LeftAndRightRotateToSlide(DialogBase go, bool isRight, bool isClockwise, bool isOpen, bool isAlpha)
        {
            Vector3 fromPos = isRight ? new Vector3(1000, 0, 0) : new Vector3(-1000, 0, 0);
            OpenPosition(go, isOpen, fromPos);
            OpenRotate(go, isOpen, isClockwise);
            if( isAlpha ) OpenAlpha(go, isOpen);
        }
        #endregion

        #region 上下往中旋转
        /// <summary>
        /// 上下往中旋转
        /// </summary>
        /// <param name="go"></param>
        /// <param name="isTop"></param>
        /// <param name="isOpen"></param>
        private void TopAndDownRotateToSlide(DialogBase go, bool isTop, bool isClockwise, bool isOpen, bool isAlpha)
        {
            Vector3 fromPos = isTop ? new Vector3(0, 1000, 0) : new Vector3(0, -1000, 0);
            OpenPosition(go, isOpen, fromPos);
            OpenRotate(go, isOpen, isClockwise);
            if( isAlpha ) OpenAlpha(go, isOpen);
        }
        #endregion

        #region 从某个点往中
        /// <summary>
        /// 从某个点往中
        /// </summary>
        /// <param name="go"></param>
        /// <param name="isOpen"></param>
        private void SomeplaceToSlide(DialogBase go, bool isOpen, bool isAlpha)
        {
            Transform skin = go.transform.Find(go.name);

            float x = Random.Range(0, skin.sizeDelta().x);
            float y = Random.Range(0, skin.sizeDelta().y);

            Vector3 fromPos = new Vector3(x, y, 0);
            OpenPosition(go, isOpen, fromPos);
            OpenScale(go, isOpen);
            if( isAlpha ) OpenAlpha(go, isOpen);



        }
        #endregion

        private void OpenPosition(DialogBase go, bool isOpen, Vector3 fromPos)
        {
            TweenPosition tp = go.transform.Find(go.name).GetOrAddComponent<TweenPosition>();
            if( isOpen ) { tp.Reset(); }
            tp.from = fromPos;
            tp.to = Vector3.zero;
            tp.duration = go.OpenDuration;
            tp.ease = Ease.InOutSine;
            tp.onFinished = () =>
            {
                if( isOpen )
                    go.OnShowed();
                else
                    DestroyDialog(go.name);
            };
            tp.Play(isOpen);
        }

        private void OpenRotate(DialogBase go, bool isOpen, bool isClockwise, Action action = null)
        {
            TweenRotation tr = go.transform.Find(go.name).GetOrAddComponent<TweenRotation>();
            if( isOpen ) tr.Reset();

            tr.from = Vector3.zero;
            tr.to = new Vector3(0, 0, 359);
            tr.isClockwise = isClockwise;
            tr.duration = go.OpenDuration;
            tr.ease = Ease.InOutSine;
            tr.onFinished = () => { action?.Invoke(); };
            tr.Play(isOpen);
        }

        private void OpenScale(DialogBase go, bool isOpen, Action action = null)
        {
            TweenScale ts = go.transform.Find(go.name).GetOrAddComponent<TweenScale>();
            if( isOpen ) ts.Reset();
            ts.from = Vector3.zero;
            ts.to = Vector3.one;
            ts.duration = go.OpenDuration;
            ts.ease = Ease.Linear;
            ts.onFinished = () => { action?.Invoke(); };
            ts.Play(isOpen);
        }

        private void OpenAlpha(DialogBase go, bool isOpen, Action action = null)
        {
            TweenAlpha ta = go.transform.Find(go.name).GetOrAddComponent<TweenAlpha>();
            if( isOpen )
                ta.Reset();
            ta.from = 0;
            ta.to = 1;
            ta.duration = go.OpenDuration;
            ta.onFinished = () => { action?.Invoke(); };
            ta.Play(isOpen);
        }

        #endregion

        #region 遮罩方式

        private void MaskStyle(DialogBase go)
        {
            Transform mask = ResourceMgr.GetInstance.CreateTransform("Prefab/DialogMask", true);
            mask.sizeDelta(go.M_Canvas.sizeDelta);
            switch( go.maskStyle )
            {
                case DialogMaskSytle.None: break;
                case DialogMaskSytle.Open:
                    mask.GetOrAddComponent<ButtonEx>().onLeftClick = g => { HideDialog(mask.transform.parent.name); };
                    break;
            }
            mask.GetComponent<Image>().color = new Color(0, 0, 0, go.alpha);
            mask.SetParent(go.gameObject.transform);
            mask.localPosition(0).localEulerAngles(0).localScale(1);

        }

        #endregion

        /// <summary>
        /// 发起关闭
        /// </summary>
        public void HideDialog(string type)
        {
            if( dialogs.ContainsKey(type) )
            {
                DialogBase pb = dialogs[type];
                StartShowDialog(pb, pb.showStyle, false);
                dialogsDethList.Remove(pb);
            }
            else
            {
                DebugUtil.LogError("Tip", "关闭的 " + type + " 面板不存在!");
            }
        }



        // <summary> 改变面板的深度 </summary>
        public void ChangeDialogDeth()
        {
            LayerMgr.GetInstance.SetDialogsLayer(dialogsDethList);
        }


        /// <summary>
        /// 强制摧毁面板
        /// </summary>
        /// <param name="type"></param>
        public void DestroyDialog(string type)
        {
            if( dialogs.ContainsKey(type) )
            {
                DialogBase pb = dialogs[type];

                if( !pb.cache )
                {
                    dialogs.Remove(type);
                    dialogsDethList.Remove(pb);
                    if( dialogs.Count <= 0 )
                        LayerMgr.GetInstance.ClearLayer();
                    else
                        LayerMgr.GetInstance.ClearLayer(isSet: false);
                    pb.OnHideDone();
                    GameObject.Destroy(pb.gameObject);
                }
                else
                {
                    if( dialogs.Count <= 0 )
                        LayerMgr.GetInstance.ClearLayer();
                    else
                        LayerMgr.GetInstance.ClearLayer(isSet: false);
                    pb.OnHideDone();
                    pb.gameObject.SetActive(false);
                }
            }
            if( cacheDialog.Count > 0 )
            {
                for( int i = 0; i < cacheDialog.Count; i++ )
                {
                    ShowDialog(cacheDialog[i].dialogName, cacheDialog[i].objectValue);
                    cacheDialog.Remove(cacheDialog[i]);
                }
            }
        }

    }
}