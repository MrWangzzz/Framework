using UnityEngine;
namespace FrameWork
{
    public class PanelBase : ViewBase
    {
        /// <summary>如果缓存为true，面板在发起关闭的时候为隐藏而不是直接删除</summary>
        public bool cache = false;

        protected object[] _panelArgs;
        /// <summary>
        /// 记录场景init时参数
        /// </summary>
        public object[] panelArgs
        {
            get
            {
                return _panelArgs;
            }
        }

        protected override void OnInitSkin()
        {
            base.OnInitSkin();
            skin.GetComponent<RectTransform>().sizeDelta = M_Canvas.sizeDelta;
        }

        protected override void OnInitSkinFront()
        {
            base.OnInitSkinFront();
            SetMainSkinPath($"Panel/{name}");
        }

        /// <summary>
        /// 重值数据
        /// </summary>
        /// <param name="panelArgs"></param>
        public virtual void OnResetArgs(params object[] panelArgs)
        {
            _panelArgs = panelArgs;
        }

        /// <summary>
        /// 初始化场景
        /// </summary>
        /// <param name="panelArgs">场景参数</param>
        public virtual void OnInit(params object[] panelArgs)
        {
            _panelArgs = panelArgs;
            Init();
        }

        /// <summary>
        /// 开始隐藏
        /// </summary>
        public override void OnHideFront()
        {
            base.OnHideFront();
            gameObject.SetActive(false);
        }

    }

}

