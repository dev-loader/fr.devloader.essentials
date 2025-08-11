/// Copyright 2025, Antonin Boureau, All rights reserved.
/// Version 20250518

using Devloader.Extensions;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Devloader.Rig
{
    public class MoveAimSourceOnTrigger : MonoBehaviour
    {
        [Header("Aim Settings")]
        [SerializeField] MultiAimConstraint _aimConstraint;
        [SerializeField] Transform _constrainedObject;

        [Header("Aim Target Settings")]
        [SerializeField] Transform _aimTarget;
        [SerializeField] float _translationDuration = .2f;
        [SerializeField] bool _useCurrentPositionAsDefault;
        [SerializeField] Vector3 _overrideCurrentPosition = Vector3.forward;
        [SerializeField] bool _overrideRelativeToConstrainedObject = true;

        [Header("Collider settings")]
        [SerializeField] SphereCollider _detectionCollider;
        [SerializeField] float _detectionRadius = 0;

        Vector3 _defaultPosition;
        AimTargetSourceData _lastNearestTargetWhoMoved;

        Vector3 groundVectorScale = Vector3.forward + Vector3.right;

        class AimTargetSourceDataCollection
        {
            List<AimTargetSourceData> _datas = new List<AimTargetSourceData>();

            public int Count => _datas.Count;

            public AimTargetSourceData Add(Transform transform)
            {
                AimTargetSourceData sourceData = _datas.Find(d => d._transform == transform);

                if (sourceData is null)
                {
                    sourceData = new AimTargetSourceData(transform);
                    _datas.Add(sourceData);
                }

                return sourceData;
            }

            public void Clear() => _datas.Clear();

            public bool Contains(Transform transform)
            {
                return _datas.Find(d => d._transform == transform) is not null;
            }

            public IEnumerator<AimTargetSourceData> GetEnumerator()
            {
                return _datas.GetEnumerator();
            }

            public void Remove(Transform transform)
            {
                int dataIndex = _datas.FindIndex(d => d._transform == transform);

                if (dataIndex > -1)
                    _datas.RemoveAt(dataIndex);
            }

        }

        class AimTargetSourceData
        {
            public Transform _transform;
            public Vector3 _lastPosition;

            public float distance;
            public float groundedDistance;

            public AimTargetSourceData(Transform transform)
            {
                _transform = transform;
                _lastPosition = transform.position;

                distance = float.MaxValue;
                groundedDistance = float.MaxValue;
            }
        }

        AimTargetSourceDataCollection _targets = new AimTargetSourceDataCollection();

#if UNITY_EDITOR
        private void OnValidate()
        {
            CheckAimConstraint();
            CheckAimTarget();
            CheckColliderSettings();
        }

        private void OnDrawGizmosSelected()
        {
            if(_aimTarget)
            {
                Gizmos.color = Color.white;
                Gizmos.DrawSphere(_aimTarget.position, .01f);

                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(_aimTarget.position, .01f);
            }
        }
#endif

        private void Awake()
        {
            if(_aimTarget)
            {
                if (_useCurrentPositionAsDefault)
                    _defaultPosition = _aimTarget.localPosition;
                else
                {
                    if (_overrideRelativeToConstrainedObject)
                        _defaultPosition    = _constrainedObject.forward * _overrideCurrentPosition.z
                                            + _constrainedObject.right * _overrideCurrentPosition.x
                                            + _constrainedObject.up * _overrideCurrentPosition.y;
                    else
                        _defaultPosition = _overrideCurrentPosition;

                    _aimTarget.localPosition = _defaultPosition;
                }
            }
        }

        private void LateUpdate()
        {
            if (_aimTarget)
            {
                if (_lastNearestTargetWhoMoved is not null)
                    _aimTarget.localPosition = Vector3.MoveTowards(_aimTarget.localPosition, _aimTarget.InverseTransformPoint(_lastNearestTargetWhoMoved._lastPosition), _translationDuration / Time.deltaTime);
                else
                    _aimTarget.localPosition = Vector3.MoveTowards(_aimTarget.localPosition, _defaultPosition, _translationDuration / Time.deltaTime);
            }
        }

        void CheckAimConstraint()
        {
            if (!_aimConstraint && TryGetComponent(out _aimConstraint) || _aimConstraint && _constrainedObject != _aimConstraint.data.constrainedObject)
                _constrainedObject = _aimConstraint.data.constrainedObject;
        }
        void CheckAimTarget()
        {
            if (!_aimTarget && transform.childCount > 0)
                _aimTarget = transform.GetChild(0);

            if (_aimTarget)
            {
                if (_useCurrentPositionAsDefault)
                    _defaultPosition = _aimTarget.localPosition;
                else
                {
                    if (_overrideRelativeToConstrainedObject)
                        _defaultPosition = _constrainedObject.forward * _overrideCurrentPosition.z
                                            + _constrainedObject.right * _overrideCurrentPosition.x
                                            + _constrainedObject.up * _overrideCurrentPosition.y;
                    else
                        _defaultPosition = _overrideCurrentPosition;

                    _aimTarget.localPosition = _defaultPosition;
                }
            }
        }
        void CheckColliderSettings()
        {
            if (!_detectionCollider && !TryGetComponent(out _detectionCollider))
                _detectionCollider = gameObject.AddComponent<SphereCollider>();

            if (_detectionCollider)
            {
                if(_detectionCollider.radius != _detectionRadius)
                    _detectionCollider.radius = _detectionRadius;

                if (_constrainedObject)
                    _detectionCollider.center = transform.InverseTransformPoint(_constrainedObject.position);

                if(!_detectionCollider.isTrigger)
                    _detectionCollider.isTrigger = true;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.TryFindComponent(out Camera camera))
            {
                AimTargetSourceData target = _targets.Add(camera.transform);

                if (_targets.Count == 1)
                    _lastNearestTargetWhoMoved = target;
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.TryFindComponent(out Camera camera))
            {
                if (_targets.Count > 1)
                {
                    foreach (AimTargetSourceData target in _targets)
                    {
                        if (target._transform.position != target._lastPosition)
                        {
                            target._lastPosition = target._transform.position;

                            target.distance = Vector3.Distance(_constrainedObject.transform.position, target._transform.position);
                            target.groundedDistance = Vector3.Distance(Vector3.Scale(_constrainedObject.transform.position, groundVectorScale), Vector3.Scale(target._transform.position, groundVectorScale));

                            if (target.groundedDistance < _lastNearestTargetWhoMoved.groundedDistance)
                                _lastNearestTargetWhoMoved = target;
                        }
                    }
                }
                else if(_targets.Count > 0)
                    _lastNearestTargetWhoMoved._lastPosition = _lastNearestTargetWhoMoved._transform.position;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryFindComponent(out Camera camera))
            {
                _targets.Remove(camera.transform);

                if (_targets.Count == 0)
                    _lastNearestTargetWhoMoved = null;
            }
        }
    }
}