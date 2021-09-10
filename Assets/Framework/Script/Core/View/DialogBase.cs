using System.Security.Policy;

using UnityEngine;

namespace FrameWork
{
    public class DialogBase : ViewBase
    {

        private bool _setSize = true;
        /// <summary>
        /// 跟随Camvas大小
        /// </summary>
        public virtual bool setSize
        {
            get => _setSize;
            protected set => _setSize = value;
        }

        private bool _isClockwise = true;
        /// <summary> 
        /// 旋转打开是否是顺时针 
        /// </summary>
        public virtual bool isClockwise
        {
            get => _isClockwise;
            protected set => _isClockwise = value;
        }

        private bool _cache = false;
        /// <summary>
        /// 缓存标识---如为false,则在关闭时destroy。
        /// </summary>
        public virtual bool cache
        {
            get => _cache;
            protected set => _cache = value;
        }

        private float _alpha = 0.8f;
        /// <summary>
        /// 遮罩透明度
        /// </summary>
        public virtual float alpha
        {
            get => _alpha;
            protected set => _alpha = value;
        }

        private float _openDuration = 0.5f;
        /// <summary> 
        /// 面板打开时间 
        /// </summary>
        public virtual float OpenDuration
        {
            get => _openDuration;
            protected set => _openDuration = value;
        }

        private DialogShowStyle _showStyle = DialogShowStyle.Normal;
        /// <summary>
        /// 面板显示方式
        /// </summary>
        public virtual DialogShowStyle showStyle
        {
            get => _showStyle;
            protected set => _showStyle = value;
        }

        private DialogMaskSytle _maskStyle = DialogMaskSytle.None;
        /// <summary> 
        /// 面板遮罩方式
        /// </summary>
        public virtual DialogMaskSytle maskStyle
        {
            get => _maskStyle;
            protected set => _maskStyle = value;
        }

        private object[] _dialogArgs;
        /// <summary>
        /// 记录面板init时参数
        /// </summary>
        public virtual object[] dialogArgs
        {
            get => _dialogArgs;
            protected set => _dialogArgs = value;
        }

        private DialogShowStyle[] DialogStyles;
        private System.Random random;

        public virtual DialogShowStyle GetShowStyle()
        {
            DialogStyles = System.Enum.GetValues(typeof(DialogShowStyle)) as DialogShowStyle[];
            random = new System.Random();
            return DialogStyles[random.Next(0, DialogStyles.Length)];
        }

        public virtual void OnCustomShow(bool isOpen) { }

        protected override void OnInitSkinFront()
        {
            base.OnInitSkinFront();
            SetMainSkinPath($"Dialog/{name}");
        }

        /// <summary>
        /// 初始化面板
        /// </summary>
        /// <param name="_dialogArgs">面板参数</param>
        public virtual void OnInit(params object[] _dialogArgs)
        {
            dialogArgs = _dialogArgs;
            Init();
            if( _setSize )
                skinTrs.GetComponent<RectTransform>().sizeDelta(M_Canvas.sizeDelta);
        }


        /// <summary>
        /// 重值数据
        /// </summary>
        /// <param name="_dialogArgs"></param>
        public virtual void OnResetArgs(params object[] _dialogArgs)
        {
            dialogArgs = _dialogArgs;
        }


        /// <summary>
        /// 发起关闭
        /// </summary>
        protected virtual void Close()
        {
            DialogMgr.Instance.HideDialog(name);
        }

        /// <summary>
        /// 发起关闭
        /// </summary>
        protected virtual void Close(string dialog)
        {
            DialogMgr.Instance.HideDialog(dialog);
        }

        /// <summary>
        /// 立刻关闭
        /// </summary>
        protected virtual void CloseImmediate()
        {
            DialogMgr.Instance.DestroyDialog(name);
        }

        /// <summary>
        /// 开始隐藏
        /// </summary>
        public override void OnHideFront()
        {
            base.OnHideFront();
            _cache = false;
        }

    }
}




