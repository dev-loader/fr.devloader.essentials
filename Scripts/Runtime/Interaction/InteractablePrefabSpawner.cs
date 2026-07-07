/// Copyright 2025, Antonin Boureau, All rights reserved.
/// Version 20250206

using Devloader.Extensions;
using Devloader.Startup.CustomSteps;

using UnityEngine;

namespace Devloader.Interaction
{
    public class InteractablePrefabSpawner : PrefabSpawner
    {

        public void SpawnAt(InteractableColliderEventData eventData)
        {
            GameObject prefabInstance = gameObject.InstantiatePrefab(prefab, prefabParent);
            prefabInstance.transform.position = eventData.closestPoint;

            if (destroyOnDelay)
                Destroy(prefabInstance, destroyDelay);
        }
    }
}