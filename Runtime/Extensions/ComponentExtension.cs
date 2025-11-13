/// Copyright 2025, Antonin Boureau, All rights reserved.
/// Version 20251113

using System.Collections.Generic;
using UnityEngine;

namespace Devloader.Extensions
{
    public static class ComponentExtension
    {
#if UNITY_6000_2_OR_NEWER
        /// <summary>
        /// Search the specified Component in the scene.
        /// If not exists and componentToCreate is greater than 0, new objects with the Component is created as a new child of parent.
        /// 
        /// You can modify gameObjects parameters (like name or SetActive(bool)) using the returned components.
        /// </summary>
        public static T[] FindAll<T>(bool inactiveToo = true, bool sortByInstanceId = false, uint componentToCreate = 0, Transform parent = null) where T : Component
        {
            T[] components = Object.FindObjectsByType<T>(inactiveToo ? FindObjectsInactive.Include : FindObjectsInactive.Exclude, sortByInstanceId ? FindObjectsSortMode.InstanceID : FindObjectsSortMode.None);

            if (components.Length <= 0 && componentToCreate > 0)
            {
                List<T> createdComponents = new List<T>((int)componentToCreate);

                for (uint i = 0; i < componentToCreate; i++)
                {
                    GameObject gameObject = new GameObject(typeof(T).Name);

                    if (parent)
                    {
                        gameObject.transform.SetParent(parent);

                        gameObject.transform.localPosition = Vector3.zero;
                        gameObject.transform.localRotation = Quaternion.identity;
                        gameObject.transform.localScale = Vector3.one;
                    }

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
        /// Search the specified Component type in the scene.
        /// If not exists and componentToCreate is greater than 0, new objects with the Component is created as a new child of parent.
        /// 
        /// You can modify gameObjects parameters (like name or SetActive(bool)) using the returned components.
        /// </summary>
        public static Object[] FindAll(System.Type type, bool inactiveToo = true, bool sortByInstanceId = false, uint componentToCreate = 0, Transform parent = null)
        {
            Object[] components = Object.FindObjectsByType(type, inactiveToo ? FindObjectsInactive.Include : FindObjectsInactive.Exclude, sortByInstanceId ? FindObjectsSortMode.InstanceID : FindObjectsSortMode.None);

            if (components.Length <= 0 && componentToCreate > 0)
            {
                List<Object> createdComponents = new List<Object>((int)componentToCreate);

                for (uint i = 0; i < componentToCreate; i++)
                {
                    GameObject gameObject = new GameObject(type.Name);

                    if (parent)
                    {
                        gameObject.transform.SetParent(parent);

                        gameObject.transform.localPosition = Vector3.zero;
                        gameObject.transform.localRotation = Quaternion.identity;
                        gameObject.transform.localScale = Vector3.one;
                    }

                    createdComponents.Add(gameObject.AddComponent(type));

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
        /// Search a specified Component in the scene.
        /// If not exists, a new object with the Component is created as a new child of parent, if createIfNotExist is true.
        /// 
        /// You can modify gameObject parameters (like name or SetActive(bool)) using the returned component.gameObject.
        /// </summary>
        public static T FindAny<T>(bool inactiveToo = true, bool createIfNotExist = false, Transform parent = null) where T : Component
        {
            T component = Object.FindAnyObjectByType<T>(inactiveToo ? FindObjectsInactive.Include : FindObjectsInactive.Exclude);

            if (!component && createIfNotExist)
            {
                GameObject gameObject = new GameObject(typeof(T).Name);

                if (parent)
                {
                    gameObject.transform.SetParent(parent);

                    gameObject.transform.localPosition = Vector3.zero;
                    gameObject.transform.localRotation = Quaternion.identity;
                    gameObject.transform.localScale = Vector3.one;
                }

#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(gameObject);
#endif

                return gameObject.AddComponent<T>();
            }
            else
                return component;
        }

        /// <summary>
        /// Search a specified Component type in the scene.
        /// If not exists, a new object with the Component is created as a new child of parent, if createIfNotExist is true.
        /// 
        /// You can modify gameObject parameters (like name or SetActive(bool)) using the returned component.gameObject.
        /// </summary>
        public static Object FindAny(System.Type type, bool inactiveToo = true, bool createIfNotExist = false, Transform parent = null)
        {
            Object component = Object.FindAnyObjectByType(type, inactiveToo ? FindObjectsInactive.Include : FindObjectsInactive.Exclude);

            if (!component && createIfNotExist)
            {
                GameObject gameObject = new GameObject(type.Name);

                if (parent)
                {
                    gameObject.transform.SetParent(parent);

                    gameObject.transform.localPosition = Vector3.zero;
                    gameObject.transform.localRotation = Quaternion.identity;
                    gameObject.transform.localScale = Vector3.one;
                }

#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(gameObject);
#endif

                return gameObject.AddComponent(type);
            }
            else
                return component;
        }

        /// <summary>
        /// Search the first specified Component in the scene.
        /// If not exists, a new object with the Component is created as a new child of parent, if createIfNotExist is true.
        /// 
        /// You can modify gameObject parameters (like name or SetActive(bool)) using the returned component.gameObject.
        /// </summary>
        public static T FindFirst<T>(bool inactiveToo = true, bool createIfNotExist = false, Transform parent = null) where T : Component
        {
            T component = Object.FindFirstObjectByType<T>(inactiveToo ? FindObjectsInactive.Include : FindObjectsInactive.Exclude);

            if (!component && createIfNotExist)
            {
                GameObject gameObject = new GameObject(typeof(T).Name);

                if (parent)
                {
                    gameObject.transform.SetParent(parent);

                    gameObject.transform.localPosition = Vector3.zero;
                    gameObject.transform.localRotation = Quaternion.identity;
                    gameObject.transform.localScale = Vector3.one;
                }

#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(gameObject);
#endif

                return gameObject.AddComponent<T>();
            }
            else
                return component;
        }

        /// <summary>
        /// Search the first specified Component type in the scene.
        /// If not exists, a new object with the Component is created as a new child of parent, if createIfNotExist is true.
        /// 
        /// You can modify gameObject parameters (like name or SetActive(bool)) using the returned component.gameObject.
        /// </summary>
        public static Object FindFirst(System.Type type, bool inactiveToo = true, bool createIfNotExist = false, Transform parent = null)
        {
            Object component = Object.FindFirstObjectByType(type, inactiveToo ? FindObjectsInactive.Include : FindObjectsInactive.Exclude);

            if (!component && createIfNotExist)
            {
                GameObject gameObject = new GameObject(type.Name);

                if (parent)
                {
                    gameObject.transform.SetParent(parent);

                    gameObject.transform.localPosition = Vector3.zero;
                    gameObject.transform.localRotation = Quaternion.identity;
                    gameObject.transform.localScale = Vector3.one;
                }

#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(gameObject);
#endif

                return gameObject.AddComponent(type);
            }
            else
                return component;
        }
#endif

        /// <summary>
        /// Search the specified Component in the scene
        /// If not exists, a new object with the Component is created as a new child of parent, if createIfNotExist is true
        /// 
        /// You can modify gameObject parameters (like name or SetActive(bool)) using the returned component.gameObject
        /// </summary>
#if UNITY_6000_2_OR_NEWER
        [System.Obsolete("Use FindFirst or FindAny instead")]
#endif
        public static T FindObject<T>(bool inactiveToo = true, bool createIfNotExist = false, Transform parent = null) where T : Component
        {
            T component = Object.FindObjectOfType<T>(inactiveToo);

            if (!component && createIfNotExist)
            {
                GameObject gameObject = new GameObject(typeof(T).Name);

                if (parent)
                {
                    gameObject.transform.SetParent(parent);

                    gameObject.transform.localPosition = Vector3.zero;
                    gameObject.transform.localRotation = Quaternion.identity;
                    gameObject.transform.localScale = Vector3.one;
                }

#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(gameObject);
#endif

                return gameObject.AddComponent<T>();
            }
            else
                return component;
        }

        /// <summary>
        /// Search the specified Component type in the scene
        /// If not exists, a new object with the Component is created as a new child of parent, if createIfNotExist is true
        /// 
        /// You can modify gameObject parameters (like name or SetActive(bool)) using the returned component.gameObject
        /// </summary>
#if UNITY_6000_2_OR_NEWER
        [System.Obsolete("Use FindFirst or FindAny instead")]
#endif
        public static Object FindObject(System.Type type,  bool inactiveToo = true, bool createIfNotExist = false, Transform parent = null)
        {
            Object component = Object.FindObjectOfType(type, inactiveToo);

            if (!component && createIfNotExist)
            {
                GameObject gameObject = new GameObject(type.Name);

                if (parent)
                {
                    gameObject.transform.SetParent(parent);

                    gameObject.transform.localPosition = Vector3.zero;
                    gameObject.transform.localRotation = Quaternion.identity;
                    gameObject.transform.localScale = Vector3.one;
                }

#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(gameObject);
#endif

                return gameObject.AddComponent(type);
            }
            else
                return component;
        }

        /// <summary>
        /// Search the specified Component in the scene
        /// If not exists, new objects with the Component is created as a new child of parent, if componentToCreate is greater than 0
        /// 
        /// You can modify gameObjects parameters (like name or SetActive(bool)) using the returned components
        /// </summary>
#if UNITY_6000_2_OR_NEWER
        [System.Obsolete("Use FindAll instead")]
#endif
        public static T[] FindObjects<T>(bool inactiveToo = true, uint componentToCreate = 0, Transform parent = null) where T : Component
        {
            T[] components = Object.FindObjectsOfType<T>(inactiveToo);

            if (components.Length <= 0 && componentToCreate > 0)
            {
                List<T> createdComponents = new List<T>((int)componentToCreate);

                for (uint i = 0; i < componentToCreate; i++)
                {
                    GameObject gameObject = new GameObject(typeof(T).Name);

                    if (parent)
                    {
                        gameObject.transform.SetParent(parent);

                        gameObject.transform.localPosition = Vector3.zero;
                        gameObject.transform.localRotation = Quaternion.identity;
                        gameObject.transform.localScale = Vector3.one;
                    }

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
        /// Search the specified Component type in the scene
        /// If not exists, new objects with the Component is created as a new child of parent, if componentToCreate is greater than 0
        /// 
        /// You can modify gameObjects parameters (like name or SetActive(bool)) using the returned components
        /// </summary>
#if UNITY_6000_2_OR_NEWER
        [System.Obsolete("Use FindAll instead")]
#endif
        public static Object[] FindObjects(System.Type type, bool inactiveToo = true, uint componentToCreate = 0, Transform parent = null)
        {
            Object[] components = Object.FindObjectsOfType(type, inactiveToo);

            if (components.Length <= 0 && componentToCreate > 0)
            {
                List<Object> createdComponents = new List<Object>((int)componentToCreate);

                for (uint i = 0; i < componentToCreate; i++)
                {
                    GameObject gameObject = new GameObject(type.Name);

                    if (parent)
                    {
                        gameObject.transform.SetParent(parent);

                        gameObject.transform.localPosition = Vector3.zero;
                        gameObject.transform.localRotation = Quaternion.identity;
                        gameObject.transform.localScale = Vector3.one;
                    }

                    createdComponents.Add(gameObject.AddComponent(type));

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
        /// Search the components with the same Component type in the scene.
        /// You can modify gameObjects parameters (like name or SetActive(bool)) using the returned components.
        /// </summary>
#if UNITY_6000_2_OR_NEWER
        public static Object[] FindSimilar(this Component component, bool inactiveToo = true, bool sortByInstanceId = false) => FindAll(component.GetType(), inactiveToo, sortByInstanceId);
#else
        public static Object[] FindSimilar(this Component component, bool inactiveToo = true, bool sortByInstanceId = false) => FindObjects(component.GetType(), inactiveToo);
#endif

        /// <summary>
        /// Search the specified Component in the parent Transform children
        /// If not exists, a new object with the Component is created as a new child of parent, if createIfNotExist is true
        /// 
        /// You can modify gameObject parameters (like name or SetActive(bool)) using the returned component.gameObject
        /// </summary>
        public static T FindComponent<T>(this Component parent, bool includeInactive = true, bool createIfNotExist = false) where T : Component
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

        /// <summary>
        /// Search the specified Component in the parent Transform children
        /// If not exists, new objects with the Component is created as a new child of parent, if componentToCreate is greater than 0
        /// 
        /// You can modify gameObjects parameters (like name or SetActive(bool)) using the returned components
        /// </summary>
        public static T[] FindComponents<T>(this Component parent, bool includeInactive = true, uint componentToCreate = 0) where T : Component
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
        /// Search the specified Component in the Current Component GameObject and its parents
        /// </summary>
        public static T FindComponentInParent<T>(this Component current, bool includeInactive = true) where T : Component
        {
            T component = current.GetComponent<T>();

            if (!component)
                component = current.GetComponentInParent<T>(includeInactive);

            return component;
        }

        /// <summary>
        /// Search all the occurence of the specified Component in the Current Component GameObject and its parents
        /// </summary>
        public static T[] FindComponentsInParent<T>(this Component current, bool includeInactive = true) where T : Component
        {
            List<T> components = new List<T>(current.GetComponents<T>());
            components.AddRange(current.GetComponentsInParent<T>(includeInactive));

            return components.ToArray();
        }

        /// <summary>
        /// Create a new GameObject or Instantiate a prefab and attach a component of type T
        /// 
        /// You can modify gameObject parameters (like name or SetActive(bool)) using the returned component.gameObject
        /// </summary>
        public static T InstantiateObject<T>(Transform parent = null, GameObject prefab = null) where T : Component
        {
            GameObject gameObject = prefab ? Object.Instantiate(prefab, parent) : new GameObject(typeof(T).ToString());

            if (parent && !prefab)
            {
                gameObject.transform.SetParent(parent);

                gameObject.transform.localPosition = Vector3.zero;
                gameObject.transform.localRotation = Quaternion.identity;
                gameObject.transform.localScale = Vector3.one;
            }

#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(gameObject);
#endif

            return gameObject.AddComponent<T>();
        }

        /// <summary>
        /// If the gameObject where the component is attached has several components, only this component will be destroyed.
        /// Otherwise, the gameObject is destroyed with the component
        /// </summary>
        /// <param name="component"></param>
        /// <param name="destroyGameObjectToo">Forces the destruction of the gameObject if set on true (default is false). That can be useful to remove components depending of this component</param>
        public static void HardDestroy(this Component component, bool destroyGameObjectToo = false)
        {
            if (!destroyGameObjectToo && component.gameObject.GetComponents<Component>().Length > 1)
                Object.Destroy(component);
            else
                Object.Destroy(component.gameObject);
        }

        /// <summary>
        /// If the gameObject where the component is attached has several components, only this component will be destroyed.
        /// Otherwise, the gameObject is destroyed with the component
        /// </summary>
        /// <param name="component"></param>
        /// <param name="keepGameObjectUltimately">Prevents the destruction of the gameObject if set on true (default behaviour). That can be useful to keep components depending of this component</param>
        /// <returns>Return true if the component or the gameObject was destroyed, false otherwise</returns>
        public static bool SoftDestroy(this Component component, bool keepGameObjectUltimately = true)
        {
            bool destroyed = true;

            if (component.gameObject.GetComponents<Component>().Length > 1)
                Object.Destroy(component);
            else if (!keepGameObjectUltimately)
                Object.Destroy(component.gameObject);
            else
                destroyed = false;

            return destroyed;
        }

        /// <summary>
        /// Try to find a component in the children. Return true if a component is found and set the value in the out parameter, otherwise return false and set the out parameter to null.
        /// </summary>
        /// <param name="component"></param>
        /// <param name="foundComponent">The component found. If several components exists, the first one will be returned.</param>
        /// <returns>Return true if a component was found.</returns>
        public static bool TryFindComponent<T>(this Component component, out T foundComponent, bool includeInactive = true, bool createIfNotExists = false) where T : Component
        {
            foundComponent = component.FindComponent<T>(includeInactive, createIfNotExists);
            return foundComponent is not null;
        }

        /// <summary>
        /// Try to find components in the children. Return true if at least one component is found and set all the components in the out parameter, otherwise return false and set the out parameter to null.
        /// </summary>
        /// <param name="component"></param>
        /// <param name="foundComponents">The components found.</param>
        /// <returns>Return true if atleast one component was found.</returns>
        public static bool TryFindComponents<T>(this Component component, out T[] foundComponents, bool includeInactive = true, uint componentToCreate = 0) where T : Component
        {
            foundComponents = component.FindComponents<T>(includeInactive, componentToCreate);
            return foundComponents.Length > 0;
        }

        /// <summary>
        /// Try to find a component in the children. Return true if a component is found and set the value in the out parameter, otherwise return false and set the out parameter to null.
        /// </summary>
        /// <param name="component"></param>
        /// <param name="foundComponent">The component found. If several components exists, the first one will be returned.</param>
        /// <returns>Return true if a component was found.</returns>
        public static bool TryFindComponentInParent<T>(this Component component, out T foundComponent, bool includeInactive = true, bool createIfNotExists = false) where T : Component
        {
            foundComponent = component.FindComponentInParent<T>(includeInactive);
            return foundComponent is not null;
        }

        /// <summary>
        /// Try to find components in the children. Return true if at least one component is found and set all the components in the out parameter, otherwise return false and set the out parameter to null.
        /// </summary>
        /// <param name="component"></param>
        /// <param name="foundComponents">The components found.</param>
        /// <returns>Return true if atleast one component was found.</returns>
        public static bool TryFindComponentsInParent<T>(this Component component, out T[] foundComponents, bool includeInactive = true, uint componentToCreate = 0) where T : Component
        {
            foundComponents = component.FindComponentsInParent<T>(includeInactive);
            return foundComponents.Length > 0;
        }

        /// <summary>
        ///	Get the specified Component on the GameObject
        ///	If not exists, a new Component is created on it
        ///	
        ///	If you don't need to create a component, just use gameObject.TryGetComponent<T>(out T component) instead
        /// </summary>
        public static T ValidateComponent<T>(this Component component) where T : Component
        {
            T t;

            if (component.TryGetComponent(out t))
                return t;
            else
            {
                t = component.gameObject.AddComponent<T>();
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(component);
#endif
                return t;
            }
        }

        /// <summary>
        ///	Get the specified Component on the GameObject
        ///	If not exists, a new Component is created on it
        ///	
        ///	If you don't need to create a component, just use gameObject.TryGetComponent<T>(out T component) instead
        /// </summary>
        /// <param name="component"></param>
        /// <param name="foundComponent">The component found. If several components exists, the first one will be returned.</param>
        /// <returns>Return true if the component was found, false if a component was created.</returns>
        public static bool ValidateComponent<T>(this Component component, out T foundComponent) where T : Component
        {
            if (!component.TryGetComponent(out foundComponent))
            {
                foundComponent = component.gameObject.AddComponent<T>();
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(component);
#endif

                return false;
            }

            return true;
        }

        /// <summary>
        ///	Try to get the specified Component on the GameObject
        ///	If not exists, a new Component is created on it
        ///	
        ///	If you don't need to create a component, just use gameObject.TryGetComponent<T>(out T component) instead
        /// </summary>
        /*public static T ValidateComponent<T>(this GameObject gameObject) where T : Component
        {
            if (gameObject.TryGetComponent(out T component))
                return component;
            else
                return gameObject.gameObject.AddComponent<T>();
        }*/
    }
}
