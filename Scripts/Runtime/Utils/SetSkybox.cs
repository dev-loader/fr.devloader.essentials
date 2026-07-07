/// Copyright 2025, Antonin Boureau, All rights reserved.
/// Version 20250206

using UnityEngine;

namespace Devloader.Utils
{
    public class SetSkybox : MonoBehaviour
    {
        Skybox skyboxComponent;

        Camera sceneCamera { get => Camera.main ?? Camera.current ?? null; }

        private void Awake() => sceneCamera?.TryGetComponent(out skyboxComponent);

        public void Apply(Material skyboxMaterial)
        {
            if (!sceneCamera)
                return;

            if (!skyboxComponent && sceneCamera.TryGetComponent(out Skybox skybox))
                skybox.material = skyboxMaterial;
            else
                sceneCamera.gameObject.AddComponent<Skybox>().material = skyboxMaterial;
        }

        public void Remove()
        {
            if (!sceneCamera)
                return;

            if (!skyboxComponent && sceneCamera.TryGetComponent(out Skybox skybox))
                Destroy(skybox);
        }
    }
}