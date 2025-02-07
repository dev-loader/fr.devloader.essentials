/// Copyright 2023, Antonin Boureau, All rights reserved.
/// Version 20230626

using UnityEngine;

namespace Devloader.Utils
{
    [AddComponentMenu("Devloader/Utils/DontDestroyOnLoad")]
    public class DontDestroyOnLoad : MonoBehaviour
    {
        public enum Method
        {
            DontDestroyOnLoad,
            OnceObjectWithTag,
            OnceObjectWithComponent
        }

        public Method method;

        [Header("Si la méthode est OnceObjectWithComponent")]
        public MonoBehaviour component;

#if UNITY_EDITOR
        private void OnValidate()
        {
            switch (method)
            {
                case Method.OnceObjectWithTag:
                    GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(tag);

                    if (gameObjects.Length > 1)
                        Debug.LogWarning("Attention, au moins un autre objet avec le tag " + tag + " est présent dans la scène");

                    break;

                case Method.OnceObjectWithComponent:
                    if(component)
                    {
                        Object[] objects = FindObjectsOfType(component.GetType());

                        if (objects.Length > 1)
                            Debug.LogWarning("Attention, au moins un autre objet avec un composant " + component.GetType() + " est présent dans la scène");
                    }
                    break;
            }
        }
#endif

        private void Awake()
        {
            switch(method)
            {
                case Method.DontDestroyOnLoad:
                    DontDestroyOnLoad(gameObject);
                    break;

                case Method.OnceObjectWithTag:
                    GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(tag);

                    if (gameObjects.Length > 1)
                        Destroy(gameObject);
                    else
                        DontDestroyOnLoad(gameObject);

                    break;

                case Method.OnceObjectWithComponent:
                    if (component)
                    {
                        Object[] objects = FindObjectsOfType(component.GetType());

                        if (objects.Length > 1)
                            Destroy(gameObject);
                        else
                            DontDestroyOnLoad(gameObject);
                    }
                    break;
            }
        }
    }
}