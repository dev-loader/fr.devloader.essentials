/// Copyright 2025, Antonin Boureau, All rights reserved.
/// Version 20250206

using Devloader.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Devloader.Startup.CustomSteps
{
    [System.Serializable]
    public class SpawnableItem
    {
        public GameObject prefab;
        public int weight = 1;
        public float minDistance = 1;

        public float minScale = .5f;
        public float maxScale = 1;
    }

    public class SpawnArea : StartupStep
    {
        [SerializeField] List<SpawnableItem> items = new List<SpawnableItem>();
        [SerializeField] int maxItems = 1;

        [SerializeField] Vector3 minAreaLimit = -Vector3.one;
        [SerializeField] Vector3 maxAreaLimit = Vector3.one;

        [SerializeField] List<GameObject> spawnedItems = new List<GameObject>();
        [SerializeField] List<SpawnableItem> distributedItems = new List<SpawnableItem>();

        IEnumerator Start()
        {
            spawnedItems.Clear();

            items.ForEach(item =>
            {
                for (int i = 0; i < item.weight; i++)
                    distributedItems.Add(item);
            });

            while (spawnedItems.Count < maxItems)
            {
                Vector3 randomPos = new Vector3(
                    Random.Range(minAreaLimit.x, maxAreaLimit.x),
                    Random.Range(minAreaLimit.y, maxAreaLimit.y),
                    Random.Range(minAreaLimit.z, maxAreaLimit.z)
                );

                int itemId = Random.Range(0, distributedItems.Count);

                GameObject item = Instantiate(items[itemId].prefab, transform);
                item.transform.RandomLocalPosition(minAreaLimit, maxAreaLimit);
                item.transform.RandomUniformScale(items[itemId].minScale, items[itemId].maxScale);

                spawnedItems.Add(item);

                yield return null;
            }

            yield break;
        }

        private void OnDestroy()
        {
            spawnedItems.ForEach(item => Destroy(item));
            spawnedItems.Clear();
        }
    }
}