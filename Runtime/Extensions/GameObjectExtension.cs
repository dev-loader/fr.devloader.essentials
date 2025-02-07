/// Copyright 2025, Antonin Boureau, All rights reserved.
/// Version 20250206

using System.Collections.Generic;
using UnityEngine;

namespace Devloader.Extensions
{
    public static class GameObjectExtension
    {
        public static T FindComponent<T>(this GameObject parent, bool includeInactive = true, bool createIfNotExist = false) where T : Component
        {
            T component = parent.GetComponentInChildren<T>(includeInactive);

            if (!component && createIfNotExist)
            {
                GameObject gameObject = new GameObject(typeof(T).Name);
                gameObject.transform.SetParent(parent.transform);

                gameObject.transform.localPosition = Vector3.zero;
                gameObject.transform.localRotation = Quaternion.identity;
                gameObject.transform.localScale = Vector3.one;

#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(gameObject);
#endif

                return gameObject.AddComponent<T>();
            }
            else
                return component;
        }

        public static T[] FindComponents<T>(this GameObject parent, bool includeInactive = true, uint componentToCreate = 0) where T : Component
        {
            if (!parent)
                return new T[0];

            T[] components = parent.GetComponentsInChildren<T>(includeInactive);

            if (components.Length <= 0 && componentToCreate > 0)
            {
                List<T> createdComponents = new List<T>((int)componentToCreate);

                for (uint i = 0; i < componentToCreate; i++)
                {
                    GameObject gameObject = new GameObject(typeof(T).Name + " " + i);
                    gameObject.transform.SetParent(parent.transform);

                    gameObject.transform.localPosition = Vector3.zero;
                    gameObject.transform.localRotation = Quaternion.identity;
                    gameObject.transform.localScale = Vector3.one;

                    createdComponents.Add(gameObject.AddComponent<T>());
#if UNITY_EDITOR
                    UnityEditor.EditorUtility.SetDirty(gameObject);
#endif
                }

                return createdComponents.ToArray();
            }
            else
                return components;
        }

        /// <summary>
        /// Search the specified Component in the Current GameObject and its parents
        /// </summary>
        public static T FindComponentInParent<T>(this GameObject current, bool includeInactive = true) where T : Component
        {
            T component = current.GetComponent<T>();

            if (!component)
                component = current.GetComponentInParent<T>(includeInactive);

            return component;
        }

        /// <summary>
        /// Instantiate a prefab at the parent origin
        /// </summary>
        public static GameObject InstantiatePrefab(this GameObject current, GameObject prefab, Transform parent = null)
        {
            GameObject gameObject = Object.Instantiate(prefab, parent);

            if (parent)
            {
                gameObject.transform.localPosition = Vector3.zero;
                gameObject.transform.localRotation = Quaternion.identity;
                gameObject.transform.localScale = Vector3.one;
            }

#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(gameObject);
#endif

            return gameObject;
        }

        public static T ValidateComponent<T>(this GameObject gameObject) where T : Component
        {
            T t;

            if (gameObject.TryGetComponent(out t))
                return t;
            else
            {
                t = gameObject.gameObject.AddComponent<T>();
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(gameObject);
#endif
                return t;
            }
        }
    }
}