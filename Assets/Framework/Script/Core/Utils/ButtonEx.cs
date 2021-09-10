using System;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace FrameWork
{
    public class ButtonEx : Button, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        public Action<Transform> onLeftClick { get; set; }
        public Action<Transform> onDoubleClick { get; set; }
        public Action<Transform> onMiddleClick { get; set; }
        public Action<Transform> onRightClick { get; set; }
        public Action<Transform> onEnter { get; set; }
        public Action<Transform> onExit { get; set; }
        public Action<Transform> onUp { get; set; }
        public Action<Transform> onDown { get; set; }
        public Action<Transform> onDrag { get; set; }
        public Action<Transform> onBeginDrag { get; set; }
        public Action<Transform> onEndDrag { get; set; }

        public PointerEventData getPointerEventData;
        public BaseEventData getBaseEventData;
        public void OnDrag(PointerEventData eventData)
        {
            getPointerEventData = eventData;
            onDrag?.Invoke(transform);
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            getPointerEventData = eventData;
            onBeginDrag?.Invoke(transform);
        }
        public void OnEndDrag(PointerEventData eventData)
        {
            getPointerEventData = eventData;
            onEndDrag?.Invoke(transform);
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            getPointerEventData = eventData;
            base.OnPointerClick(eventData);
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                switch (eventData.clickCount)
                {
                    case 1:onLeftClick?.Invoke(transform);break;
                    case 2:onDoubleClick?.Invoke(transform);break;
                }
            }
            else if (eventData.button == PointerEventData.InputButton.Middle)
            {
                onMiddleClick?.Invoke(transform);
            }
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                onRightClick?.Invoke(transform);
            }
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            getPointerEventData = eventData;
            base.OnPointerEnter(eventData);
            onEnter?.Invoke(transform);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            getPointerEventData = eventData;
            base.OnPointerExit(eventData);
            onExit?.Invoke(transform);
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            getPointerEventData = eventData;
            base.OnPointerUp(eventData);
            onUp?.Invoke(transform);
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            getPointerEventData = eventData;
            base.OnPointerDown(eventData);
            onDown?.Invoke(transform);
        }

    }
}