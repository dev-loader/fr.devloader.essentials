using UnityEngine;

namespace Devloader.Utils
{
    public class BasicSpawner : MonoBehaviour
    {
        [Header("Behaviour settings")]
        [SerializeField] private bool _onStart;

        [Header("Prefab settings")]
        [SerializeField] private GameObject _prefab;
        [SerializeField] private Transform _parent;

        [Space]
        [SerializeField] bool _overrideTransform;
        [SerializeField] Vector3 _position = Vector3.zero;
        [SerializeField] Vector3 _eulerAngles = Vector3.zero;

        [Space]
        [SerializeField, Tooltip("Use 0 or a negative value to deactivate autodestruction")] float _autoDestroyAfter = -1;

        private void Start()
        {
            if (_onStart)
                InstantiatePrefab();
        }

        public virtual void InstantiatePrefab()
        {
            if (!_prefab)
                return;

            GameObject instance;

            if (_overrideTransform)
                instance = Instantiate(_prefab, _position, Quaternion.Euler(_eulerAngles), _parent);
            else
                instance = Instantiate(_prefab, _parent);

            if(_autoDestroyAfter > 0)
                Destroy(instance, _autoDestroyAfter);
        }
    }
}