/// Copyright 2025, Antonin Boureau, All rights reserved.
/// Version 20250206

using UnityEngine;
using UnityEngine.Events;

namespace Devloader.Utils
{
    [AddComponentMenu("Devloader/Utils/KeyCatcher")]
    public class KeyCatcher : MonoBehaviour
    {
        [SerializeField] KeyCode key = KeyCode.Space;
        [Space]

        [SerializeField] bool continuously = false;
        [SerializeField] UnityEvent continuouslyEvent = new UnityEvent();
        [Space]

        [SerializeField] bool onDown = false;
        [SerializeField] UnityEvent onDownEvent = new UnityEvent();
        [Space]

        [SerializeField] bool onUp = true;
        [SerializeField] UnityEvent onUpEvent = new UnityEvent();

        private void Update()
        {
            if (continuously && Input.GetKey(key))
                continuouslyEvent.Invoke();

            if (onDown && Input.GetKeyDown(key))
                onDownEvent.Invoke();

            if (onUp && Input.GetKeyUp(key))
                onUpEvent.Invoke();
        }
    }
}