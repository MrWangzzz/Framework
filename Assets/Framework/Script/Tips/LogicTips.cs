using System.Collections.Generic;

using UnityEngine;
namespace FrameWork
{
    public class LogicTips : LogicBase
    {
        protected override void Awake()
        {
            mTipsQueue = new List<string>();
        }

        protected override void OnDestroy()
        {
            mTipsQueue.Clear();
            mTipsQueue = null;
        }
        private List<string> mTipsQueue;

        private GameObject LastTips;

        public void AddTips(string content)
        {
            GameObject tipsObj = ResourceMgr.GetInstance.CreateGameObject("Prefab/Tips", true);
            LayerMgr.GetInstance.SetLayer(tipsObj, LayerType.Tips);
            Vector3 originPos = new Vector3(0, 350, 0);
            if (LastTips != null)
            {
                float uiHigh = LastTips.SeachTrs<RectTransform>("TipSprite").sizeDelta.y;
                if (LastTips.transform.localPosition.y < uiHigh)
                    originPos = LastTips.transform.localPosition - new Vector3(0, uiHigh * 1.2f, 0);
            }
            tipsObj.transform.localPosition(originPos).localScale(1).localEulerAngles(Vector3.zero);
            TweenPosition tp = tipsObj.GetComponent<TweenPosition>();
            tp.from = originPos;
            TipsView tv = tipsObj.GetComponent<TipsView>();
            tv.StartTips(content);
            LastTips = tipsObj;
        }
    }
}