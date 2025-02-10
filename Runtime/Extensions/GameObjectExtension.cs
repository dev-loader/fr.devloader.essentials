/// Copyright 2025, Antonin Boureau, All rights reserved.
/// Version 20250210

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

        /// <summary>
        /// Try to find a component in the children. Return true if a component is found and set the value in the out parameter, otherwise return false and set the out parameter to null.
        /// </summary>
        /// <param name="component"></param>
        /// <param name="foundComponent">The component found. If several components exists, the first one will be returned.</param>
        /// <returns>Return true if a component was found.</returns>
        public static bool TryFindComponent<T>(this GameObject gameObject, out T foundComponent, bool includeInactive = true, bool createIfNotExists = false) where T : Component
        {
            foundComponent = gameObject.FindComponent<T>(includeInactive, createIfNotExists);
            return foundComponent is not null;
        }

        /// <summary>
        /// Try to find components in the children. Return true if at least one component is found and set all the components in the out parameter, otherwise return false and set the out parameter to null.
        /// </summary>
        /// <param name="component"></param>
        /// <param name="foundComponents">The components found.</param>
        /// <returns>Return true if atleast one component was found.</returns>
        public static bool TryFindComponents<T>(this GameObject gameObject, out T[] foundComponents, bool includeInactive = true, uint componentToCreate = 0) where T : Component
        {
            foundComponents = gameObject.FindComponents<T>(includeInactive, componentToCreate);
            return foundComponents.Length > 0;
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