/// Copyright 2023, Antonin Boureau, All rights reserved.
/// Version 20230626

using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace Devloader.Startup
{
    [AddComponentMenu("Devloader/Startup/StartupSequence")]
    public class StartupSequence : MonoBehaviour
    {
        private static StartupSequence instance;

        public List<StartupStep> steps = new List<StartupStep>();
        int index = 0;

        [Header("Next Scene Settings")]
        public bool loadSceneAfterLastStep = true;
        public string nextSceneToLoad;

#if UNITY_EDITOR
        private void OnValidate()
        {
            for (int i = 0; i < steps.Count; i++)
                if (steps[i] && steps[i].gameObject.activeSelf)
                    steps[i].gameObject.SetActive(false);
        }
#endif

        private void Awake()
        {
            if (!instance)
                instance = this;
            else
                Destroy(gameObject);
        }

        private void Start()
        {
            if (steps.Count == index && loadSceneAfterLastStep)
            {
                LoadScene();
                return;
            }

            steps[index].onStartupReady.AddListener(Next);
            steps[index].gameObject.SetActive(true);
        }

        public void Next()
        {
            steps[index++].onStartupReady.RemoveListener(Next);

            if (index < steps.Count)
            {
                if (steps[index] != null)
                {
                    steps[index].onStartupReady.AddListener(Next);
                    steps[index].gameObject.SetActive(true);
                }
                else
                    Next();
            }
            else if (loadSceneAfterLastStep)
                LoadScene();
        }

        private void LoadScene()
        {
            if (!string.IsNullOrEmpty(nextSceneToLoad.Trim()))
                SceneManager.LoadScene(nextSceneToLoad);
            else if (gameObject.scene.buildIndex + 1 < SceneManager.sceneCountInBuildSettings)
                SceneManager.LoadScene(gameObject.scene.buildIndex + 1);
        }

        public static void RemoveStep(StartupStep step)
        {
            if (!instance)
                return;

            instance.steps.Remove(step);
        }
    }
}