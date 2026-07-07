/// Copyright 2023, Antonin Boureau, All rights reserved.
/// Version 20230626

using UnityEngine;
using UnityEngine.SceneManagement;

namespace Devloader.Utils
{
    [AddComponentMenu("Devloader/Utils/SceneLoaderBasis")]
    public class SceneLoaderBasis : MonoBehaviour
    {
        public string sceneName;

        public static void LoadScene(string sceneName, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
        { SceneManager.LoadSceneAsync(sceneName, loadSceneMode); }

        public virtual void OpenScene(string sceneName)
        { SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single); }

        public virtual void OpenScene()
        { SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single); }

        public virtual void AddScene(string sceneName)
        { SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive); }

        public virtual void AddScene()
        { SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive); }
    }
}