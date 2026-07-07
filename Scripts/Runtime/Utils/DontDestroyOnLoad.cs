/// Copyright 2025, Antonin Boureau, All rights reserved.
/// Version 20251113

using Devloader.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Devloader.Utils
{
    [AddComponentMenu("Devloader/Utils/DontDestroyOnLoad")]
    public class DontDestroyOnLoad : MonoBehaviour
    {
        public enum Method
        {
            DontDestroyOnLoad,
            [System.Obsolete, HideInInspector]
            OnceObjectWithTag,
            OnlyObjectWithTag,
            [System.Obsolete, HideInInspector]
            OnceObjectWithComponent,
            OnlyObjectWithComponent,
        }

        [Header("Main settings")]
        [SerializeField] Method _method;

        [Header("OnlyObjectWithComponent settings")]
        [SerializeField] MonoBehaviour _component;

        [Header("OnlyObjectWithComponent Settings")]
        [SerializeField] bool _debugOnDestroyInEditor = false;

        private Transform _initialParent;

#if UNITY_EDITOR
        [System.Obsolete]
        private void OnValidate()
        {
            switch (_method)
            {
                case Method.OnceObjectWithTag:
                case Method.OnlyObjectWithTag:
                    GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(tag);

                    if (gameObjects.Length > 1)
                        Debug.LogWarning("Attention, au moins un autre objet avec le tag " + tag + " est présent dans la scène");

                    break;

                case Method.OnceObjectWithComponent:
                case Method.OnlyObjectWithComponent:
                    if (_component)
                    {
                        Object[] objects = _component.FindSimilar();

                        if (objects.Length > 1)
                            Debug.LogWarning("Attention, au moins un autre objet avec un composant " + _component.GetType() + " est présent dans la scène");
                    }
                    break;
            }
        }
#endif

        private void Awake()
        {
            if (transform.parent)
            {
                _initialParent = transform.parent;
                transform.parent = null;
            }

            switch (_method)
            {
                case Method.DontDestroyOnLoad:
                    DontDestroyOnLoad(gameObject);
                    break;

                case Method.OnlyObjectWithTag:
                    GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(tag);

                    if (gameObjects.Length > 1)
                        Destroy(gameObject);
                    else
                        DontDestroyOnLoad(gameObject);

                    break;

                case Method.OnlyObjectWithComponent:
                    if (_component)
                    {
                        Object[] objects = _component.FindSimilar();

                        if (objects.Length > 1)
                            Destroy(gameObject);
                        else
                            DontDestroyOnLoad(gameObject);
                    }
                    break;
            }
        }

        private void OnDestroy()
        {
            if(SceneManager.loadedSceneCount > 0)
            {
                SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());

                if (_initialParent)
                    transform.parent = _initialParent;
            }
#if UNITY_EDITOR
            else if(_debugOnDestroyInEditor)
                Debug.LogWarning("A Don't Destroy On Load element was destroyed because no scene was loaded");
#else
            else
                Debug.LogWarning("A Don't Destroy On Load element was destroyed because no scene was loaded");
#endif
        }

        public Method method { get => _method; set => _method = value; }

        public MonoBehaviour component { get =>  _component; set => _component = value; }
    }
}