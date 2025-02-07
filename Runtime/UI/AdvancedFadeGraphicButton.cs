/// Copyright 2024, Antonin Boureau, All rights reserved.
/// Version 20240408

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Devloader.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Devloader.UI
{
    public class AdvancedFadeGraphicButton : Button, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        private Texture[] normalTextures = {};
        public bool disableOnClick = true;

        public new bool interactable
        {
            get { return base.interactable; }
            set
            {
                base.interactable = value;
                UpdateGraphics();
            }
        }

        public enum ButtonState
        {
            Normal,
            Hovered,
            Pressed,
            Disabled
        }

        public UnityEvent<ButtonState> onStateChanged = new UnityEvent<ButtonState>();

        public UnityEvent onDisabled = new UnityEvent();
        public UnityEvent onHighlighted = new UnityEvent();
        public UnityEvent onPressed = new UnityEvent();

        private UnityAction<ButtonState> stateDispatcher;

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            UpdateGraphics();
        }
#endif

        protected override void Awake()
        {
            GameObject parent = (targetGraphic) ? targetGraphic.gameObject : gameObject;
            Graphic[] graphics = parent.GetComponentsInChildren<Graphic>();

            normalTextures = new Texture[graphics.Length];

            for (int i = 0; i < graphics.Length; i++)
                normalTextures[i] = graphics[i].mainTexture;

            stateDispatcher = state =>
            {
                switch (state)
                {
                    case ButtonState.Hovered:
                        onHighlighted.Invoke();
                        break;

                    case ButtonState.Pressed:
                        onPressed.Invoke();
                        break;

                    case ButtonState.Disabled:
                        onDisabled.Invoke();
                        break;
                }
            };
        }

        protected override void OnEnable() => onStateChanged.AddListener(stateDispatcher);

        protected override void OnDisable()
        {
            if(onStateChanged != null && stateDispatcher != null)
                onStateChanged.RemoveListener(stateDispatcher);
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            UpdateGraphics();
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            UpdateGraphics();
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            UpdateGraphics();
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);

            if(interactable)
                onClick.Invoke();

            if (disableOnClick)
                interactable = false;

            UpdateGraphics();
        }

        public void UpdateGraphics()
        {
            GameObject parent = (targetGraphic) ? targetGraphic.gameObject : gameObject;

            Graphic[] graphics = parent.GetComponentsInChildren<Graphic>(true);

            if (transition == Transition.ColorTint)
            {
                Color colorToApply;

                switch (currentSelectionState)
                {
                    case SelectionState.Disabled:
                        colorToApply = colors.disabledColor;
                        onStateChanged.Invoke(ButtonState.Disabled);
                        break;

                    case SelectionState.Highlighted:
                        colorToApply = colors.highlightedColor;
                        onStateChanged.Invoke(ButtonState.Hovered);
                        break;

                    case SelectionState.Pressed:
                        colorToApply = colors.pressedColor;
                        onStateChanged.Invoke(ButtonState.Pressed);
                        break;

                    case SelectionState.Selected:
                        colorToApply = colors.selectedColor;
                        onStateChanged.Invoke(ButtonState.Hovered);
                        break;

                    default:
                        colorToApply = colors.normalColor;
                        onStateChanged.Invoke(ButtonState.Normal);
                        break;
                }

                for (int i = 0; i < graphics.Length; i++)
                    graphics[i].CrossFadeColor(colorToApply, colors.fadeDuration, true, true, true);
            }
            else if (transition == Transition.SpriteSwap)
            {
                switch (currentSelectionState)
                {
                    case SelectionState.Disabled:
                        if(spriteState.disabledSprite)
                            for (int i = 0; i < graphics.Length; i++)
                                graphics[i].material.mainTexture = spriteState.disabledSprite.texture;

                        onStateChanged.Invoke(ButtonState.Disabled);
                        break;

                    case SelectionState.Highlighted:
                        if (spriteState.highlightedSprite)
                            for (int i = 0; i < graphics.Length; i++)
                                graphics[i].material.mainTexture = spriteState.highlightedSprite.texture;

                        onStateChanged.Invoke(ButtonState.Hovered);
                        break;

                    case SelectionState.Pressed:
                        if (spriteState.pressedSprite)
                            for (int i = 0; i < graphics.Length; i++)
                                graphics[i].material.mainTexture = spriteState.pressedSprite.texture;

                        onStateChanged.Invoke(ButtonState.Pressed | ButtonState.Hovered);
                        break;

                    case SelectionState.Selected:
                        if (spriteState.selectedSprite)
                            for (int i = 0; i < graphics.Length; i++)
                                graphics[i].material.mainTexture = spriteState.selectedSprite.texture;

                        onStateChanged.Invoke(ButtonState.Hovered);
                        break;

                    default:
                        for (int i = 0; i < normalTextures.Length; i++)
                            graphics[i].material.mainTexture = normalTextures[i];

                        onStateChanged.Invoke(ButtonState.Normal);
                        break;
                }
            }
            else if (transition == Transition.Animation)
                Debug.Log("[AdvancedFadeGraphicButton] Animation transition not implemented yet");
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(AdvancedFadeGraphicButton))]
public class AdvancedFadeGraphicButtonEditor : Editor
{
    SerializedProperty interactable;
    SerializedProperty transition;
    SerializedProperty targetGraphic;
    SerializedProperty colors;
    SerializedProperty spriteState;
    SerializedProperty navigation;
    SerializedProperty onClick;

    SerializedProperty onStateChanged;

    SerializedProperty onHighlighted;
    SerializedProperty onPressed;
    SerializedProperty onDisabled;

    SerializedProperty disableOnClick;

    private void OnEnable()
    {
        interactable = serializedObject.FindProperty("m_Interactable");
        transition = serializedObject.FindProperty("m_Transition");
        targetGraphic = serializedObject.FindProperty("m_TargetGraphic");
        colors = serializedObject.FindProperty("m_Colors");
        spriteState = serializedObject.FindProperty("m_SpriteState");
        navigation = serializedObject.FindProperty("m_Navigation");
        onClick = serializedObject.FindProperty("m_OnClick");

        onStateChanged = serializedObject.FindProperty("onStateChanged");

        onHighlighted = serializedObject.FindProperty("onHighlighted");
        onPressed = serializedObject.FindProperty("onPressed");
        onDisabled = serializedObject.FindProperty("onDisabled");

        disableOnClick = serializedObject.FindProperty("disableOnClick");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        AdvancedFadeGraphicButton myScript = target as AdvancedFadeGraphicButton;

        EditorGUILayout.LabelField("Button basics", EditorStyles.boldLabel);

        EditorGUILayout.PropertyField(interactable);
        EditorGUILayout.PropertyField(transition);

        EditorGUI.indentLevel++;

        EditorGUILayout.PropertyField(targetGraphic);
        switch (myScript.transition)
        {
            case Selectable.Transition.ColorTint:
                EditorGUILayout.PropertyField(colors);
                break;

            case Selectable.Transition.SpriteSwap:
                EditorGUILayout.PropertyField(spriteState);
                break;

            case Selectable.Transition.Animation:
                EditorGUILayout.LabelField("Non pris en charge");
                break;
        }

        EditorGUI.indentLevel--;
        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(navigation);
        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(onClick);
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Advanced", EditorStyles.boldLabel);

        EditorGUILayout.PropertyField(onStateChanged);
        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(onHighlighted);
        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(onPressed);
        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(onDisabled);
        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(disableOnClick);

        serializedObject.ApplyModifiedProperties();
    }
}
#endif