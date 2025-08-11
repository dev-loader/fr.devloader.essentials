/// Copyright 2025, Antonin Boureau, All rights reserved.
/// Version 20250811

using System.Collections;

using UnityEngine;
using UnityEngine.Events;

using Devloader.Extensions;
using Devloader.Lifecycle;

namespace Devloader.Startup.CustomSteps
{
    public class PrefabSpawner : StartupStep
    {
        [Header("Prefab settings")]
        [SerializeField] bool _instanceOnStart = false;
        [SerializeField] GameObject _prefab;
        [SerializeField, Tooltip("If null, current gameObject will be used")] Transform _prefabParent;
        [Space]
        [SerializeField] UnityEvent<GameObject> _onSpawn = new UnityEvent<GameObject>();

        [Header("Instance settings")]
        [SerializeField] bool _destroyOnDelay = true;
        [SerializeField] float _destroyDelay = 1;
        [Space]

        [SerializeField] UnityEvent _onDestroy = new UnityEvent();


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
            _onSpawn.Invoke(prefabInstance);

            prefabInstance.ValidateComponent<OnDestroyHandler>().onDestroy.AddListener(() => _onDestroy.Invoke());

            if (_destroyOnDelay)
                Destroy(prefabInstance, _destroyDelay);
        }
    }
}
