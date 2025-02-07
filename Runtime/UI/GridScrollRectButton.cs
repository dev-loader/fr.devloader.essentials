/// Copyright 2024, Antonin Boureau, All rights reserved.
/// Version 20240522

using System.Collections;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using Devloader.Extensions;

namespace Devloader.UI
{
    [AddComponentMenu("Devloader/UI/GridScrollRectButton")]
    public class GridScrollRectButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
    {
        public enum ScrollOn
        {
            PointerClick,
            PointerEnter
        }

        #region Inspector

        [Header("Scroll Rect")]
        [SerializeField] ScrollRect scrollRect;
        [Space]

        [Header("Scroll Settings")]
        [SerializeField] float scrollRepeatDelay = 1.0f;
        [SerializeField, Range(-1, 1)] int scrollDirection = 1;
        [SerializeField] float scrollSpeed = 2f;
        [SerializeField] float scrollStep = 1f;
        [Space]
        [SerializeField] ScrollOn scrollOn = ScrollOn.PointerEnter;
        [Space]

        [Header("Button Customization")]
        [SerializeField] ColorBlock colors = ColorBlock.defaultColorBlock;

        #endregion

        Graphic graphic;
        bool underPointer = false;
        static Coroutine scrollCoroutine;

        private RectTransform GridTransform { get => scrollRect.content; }

        public bool HasReachLeftLimit { get => GridTransform.localPosition.x >= 0; }

        public bool HasReachRightLimit { get => -GridTransform.localPosition.x >= GridTransform.sizeDelta.x; }

        public bool Interactable {
            get
            {
                if (scrollDirection < 0)
                    return !HasReachLeftLimit;
                else if (scrollDirection > 0)
                    return !HasReachRightLimit;
                else
                    return false;
            }
        }

#if UNITY_EDITOR

        private void OnValidate()
        {
            if (!scrollRect)
                scrollRect = GetComponentInParent<ScrollRect>();
        }

        private void Reset()
        {
            if (!scrollRect)
                scrollRect = GetComponentInParent<ScrollRect>();
        }

#endif

        private void Awake()
        {
            try
            {
                graphic = this.ValidateComponent<Graphic>();
            }
            catch (System.Exception)
            {
                graphic = this.ValidateComponent<Image>();
            }

            if (!scrollRect)
                scrollRect = GetComponentInParent<ScrollRect>();

            scrollRect.onValueChanged.AddListener(OnScrollRectValueChanged);
        }

        private void OnEnable() => UpdateColorFade();

        private void Start()
        {
            if (scrollDirection <= 0)
                graphic.CrossFadeColor(colors.disabledColor, colors.fadeDuration, true, true);
            else
                graphic.CrossFadeColor(colors.normalColor, colors.fadeDuration, true, true);

        }

        private void OnDisable() => graphic.CrossFadeColor(colors.disabledColor, colors.fadeDuration, true, true);

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            if (!Interactable)
                return;

            if (scrollOn != ScrollOn.PointerClick)
                return;

            if (scrollCoroutine != null)
                return;

            graphic.CrossFadeColor(colors.pressedColor, colors.fadeDuration, true, true);
            scrollCoroutine = StartCoroutine(ScrollCoroutine());
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            underPointer = true;

            if (!Interactable)
                return;

            if (scrollCoroutine != null)
                return;

            //if (gridTransform.localPosition.x == 0 && scrollDirection < 0 || -gridTransform.localPosition.x == gridTransform.sizeDelta.x && scrollDirection > 0)

            UpdateColorFade();

            if (scrollOn != ScrollOn.PointerEnter)
                return;

            scrollCoroutine = StartCoroutine(ScrollCoroutine());
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            underPointer = false;
            UpdateColorFade();
        }

        void OnScrollRectValueChanged(Vector2 value) => UpdateColorFade();

        IEnumerator ScrollCoroutine()
        {
            switch (scrollOn)
            {
                case ScrollOn.PointerClick:
                    if (underPointer)
                        yield return ScrollActionCoroutine(scrollRect.content);
                        UpdateColorFade();
                    break;

                case ScrollOn.PointerEnter:
                    while (underPointer)
                    {
                        yield return ScrollActionCoroutine(scrollRect.content);

                        float t = 0;
                        while (underPointer && t < scrollRepeatDelay)
                        {
                            t += Time.deltaTime;
                            yield return null;
                        }
                    }
                    break;
            }

            scrollCoroutine = null;
            yield break;
        }

        IEnumerator ScrollActionCoroutine(RectTransform scrollRectContent)
        {
            GridLayoutGroup gridLayout = scrollRect.content.ValidateComponent<GridLayoutGroup>();

            float delta = (gridLayout.cellSize.x + gridLayout.spacing.x) * scrollStep;
            Vector3 wantedPosition = scrollRectContent.localPosition + Vector3.left * delta * scrollDirection;

            if (-wantedPosition.x > scrollRectContent.sizeDelta.x || -wantedPosition.x < 0)
                wantedPosition.x = Mathf.Clamp(wantedPosition.x, -scrollRectContent.sizeDelta.x, 0);

            while (scrollRectContent.localPosition != wantedPosition)
            {
                scrollRectContent.localPosition = Vector3.MoveTowards(scrollRectContent.localPosition, wantedPosition, delta * Time.deltaTime * scrollSpeed);
                yield return null;
            }

            yield break;
        }

        void UpdateColorFade()
        {
            if (Interactable)
                graphic.CrossFadeColor(underPointer ? colors.highlightedColor : colors.normalColor, colors.fadeDuration, true, true);
            else
                graphic.CrossFadeColor(colors.disabledColor, colors.fadeDuration, true, true);
        }
    }
}