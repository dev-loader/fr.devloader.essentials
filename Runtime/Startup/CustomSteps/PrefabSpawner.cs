/// Copyright 2025, Antonin Boureau, All rights reserved.
/// Version 20250206

using Devloader.Extensions;
using System.Collections;

using UnityEngine;

namespace Devloader.Startup.CustomSteps
{
    public class PrefabSpawner : StartupStep
    {
        [Space]
        [SerializeField] GameObject _prefab;
        [SerializeField, Tooltip("If null, current gameObject will be used")] Transform _prefabParent;

        [Space]
        [SerializeField] bool _instanceOnStart = false;
        [SerializeField] bool _destroyOnDelay = true;

        [Space]
        [SerializeField] float _destroyDelay = 1;

        public GameObject prefab => _prefab;
        public Transform prefabParent => _prefabParent;

        public bool destroyOnDelay => _destroyOnDelay;
        public float destroyDelay => _destroyDelay;


        private IEnumerator Start()
        {
            if (_instanceOnStart)
                Spawn();

            onStartupReady.Invoke();
            yield break;
        }

        public void Spawn()
        {
            GameObject prefabInstance = gameObject.InstantiatePrefab(_prefab, _prefabParent);

            if (_destroyOnDelay)
                Destroy(prefabInstance, _destroyDelay);
        }
    }
}
