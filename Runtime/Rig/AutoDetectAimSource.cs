/// Copyright 2025, Antonin Boureau, All rights reserved.
/// Version 20250206

using Devloader.ColliderManagement;
using Devloader.Extensions;
using Devloader.Utils;

using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Events;

namespace Devloader.Rig
{
    public class AutoDetectAimSource : MonoBehaviour
    {
        enum TriggerEventType
        {
            Enter,
            Exit,
            Stay
        }

        [Header("Aim Settings")]
        [SerializeField] MultiAimConstraint aimConstraint;
        [SerializeField] Transform constrainedObject;

        [Header("Collider settings")]
        [SerializeField] SphereCollider detectionCollider;
        [SerializeField] float detectionRadius = 0;

        WeightedTransform currentSourceObject;

        UnityAction<Collider> onTriggerEnter;
        UnityAction<Collider> onTriggerExit;

        private void OnValidate()
        {
            CheckAimConstraint();
            CheckColliderSettings();
        }

        private void Awake()
        {
            onTriggerEnter = collider => CheckColliderTrigger(collider, TriggerEventType.Enter);
            onTriggerExit = collider => CheckColliderTrigger(collider, TriggerEventType.Exit);
        }

        private void OnEnable()
        {
            ColliderEventHandler eventHandler = this.ValidateComponent<ColliderEventHandler>();
            eventHandler.triggerEnterEvent.AddListener(onTriggerEnter);
            eventHandler.triggerExitEvent.AddListener(onTriggerExit);
        }

        private void OnDisable()
        {
            ColliderEventHandler eventHandler = this.ValidateComponent<ColliderEventHandler>();
            eventHandler.triggerEnterEvent.RemoveListener(onTriggerEnter);
            eventHandler.triggerExitEvent.RemoveListener(onTriggerExit);
        }

        void CheckAimConstraint()
        {
            if (!aimConstraint && TryGetComponent(out aimConstraint) || aimConstraint && constrainedObject != aimConstraint.data.constrainedObject)
                constrainedObject = aimConstraint.data.constrainedObject;
        }

        void CheckColliderSettings()
        {
            if (!detectionCollider && TryGetComponent(out detectionCollider))
                detectionRadius = detectionCollider.radius;
            else if (detectionCollider && detectionCollider.radius != detectionRadius)
            {
                detectionCollider.radius = detectionRadius;

                if (constrainedObject)
                    detectionCollider.center = transform.InverseTransformPoint(constrainedObject.position);
            }
        }

        void CheckColliderTrigger(Collider collider, TriggerEventType eventType)
        {
            switch (eventType)
            {
                case TriggerEventType.Enter:
                    if (collider.TryGetComponent(out CharacterController characterController))
                    {
                        currentSourceObject.transform = CameraUtils.Active.transform;
                        currentSourceObject.weight = 1;

                        if (aimConstraint && !aimConstraint.data.sourceObjects.Contains(currentSourceObject))
                        {
                            WeightedTransformArray sourceObjects = aimConstraint.data.sourceObjects;
                            sourceObjects.Add(currentSourceObject);

                            aimConstraint.data.sourceObjects = sourceObjects;
                            aimConstraint.FindComponentInParent<RigBuilder>()?.Build();
                        }
                    }

                    break;

                case TriggerEventType.Exit:
                    if (collider.transform == currentSourceObject.transform)
                    {
                        currentSourceObject.weight = 0;

                        if (aimConstraint.data.sourceObjects.Count > 0)
                            aimConstraint.data.sourceObjects.SetWeight(0, currentSourceObject.weight);
                    }

                    break;
            }
        }
    }
}