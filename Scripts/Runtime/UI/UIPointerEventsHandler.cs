/// Copyright 2025, Antonin Boureau, All rights reserved.
/// Version 20250302

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Devloader.UI
{
    public class UIPointerEventsHandler : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler, IPointerUpHandler
    {
        [SerializeField] bool enableOnClick = true;
        [SerializeField] UnityEvent<PointerEventData> onPointerClick = new UnityEvent<PointerEventData>();
        [Space]

        [SerializeField] bool enableOnDown = true;
        [SerializeField] UnityEvent<PointerEventData> onPointerDown = new UnityEvent<PointerEventData>();
        [Space]

        [SerializeField] bool enableOnEnter = true;
        [SerializeField] UnityEvent<PointerEventData> onPointerEnter = new UnityEvent<PointerEventData>();
        [Space]

        [SerializeField] bool enableOnExit = true;
        [SerializeField] UnityEvent<PointerEventData> onPointerExit = new UnityEvent<PointerEventData>();
        [Space]

        [SerializeField] bool enableOnMove = true;
        [SerializeField] UnityEvent<PointerEventData> onPointerMove = new UnityEvent<PointerEventData>();
        [Space]

        [SerializeField] bool enableOnUp = true;
        [SerializeField] UnityEvent<PointerEventData> onPointerUp = new UnityEvent<PointerEventData>();

        public void OnPointerClick(PointerEventData eventData)
        {
            if(enableOnClick)
                onPointerClick.Invoke(eventData);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (enableOnDown)
                onPointerDown.Invoke(eventData);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (enableOnEnter)
                onPointerEnter.Invoke(eventData);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (enableOnExit)
                onPointerExit.Invoke(eventData);
        }

        void IPointerMoveHandler.OnPointerMove(PointerEventData eventData)
        {
            if (enableOnMove)
                onPointerMove.Invoke(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (enableOnUp)
                onPointerUp.Invoke(eventData);
        }
    }
}