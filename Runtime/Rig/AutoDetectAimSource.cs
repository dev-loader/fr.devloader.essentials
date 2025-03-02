/// Copyright 2025, Antonin Boureau, All rights reserved.
/// Version 20250219

using Devloader.ColliderManagement;
using Devloader.Extensions;
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
        [SerializeField] float weightTransitionDuration = 1f;

        [Header("Collider settings")]
        [SerializeField] SphereCollider detectionCollider;
        [SerializeField] float detectionRadius = 0;

        [Header("For debug purposes")]
        [SerializeField] WeightedTransform currentSourceObject;

        UnityAction<Collider> onTriggerEnter;
        UnityAction<Collider> onTriggerExit;

        RigBuilder rigBuilder;

        bool increaseWeight;

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

        private void Update()
        {
            if (increaseWeight)
                UpdateSourceWeight(Time.deltaTime / weightTransitionDuration);
            else
                UpdateSourceWeight(Time.deltaTime / - weightTransitionDuration);
        }

        private void OnEnable()
        {
            ColliderEventHandler eventHandler = this.ValidateComponent<ColliderEventHandler>();
            eventHandler.triggerEnterEvent.AddListener(onTriggerEnter);
            eventHandler.triggerExitEvent.AddListener(onTriggerExit);

            if (aimConstraint)
                rigBuilder = aimConstraint.FindComponentInParent<RigBuilder>();
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
            Camera camera;
            switch (eventType)
            {
                case TriggerEventType.Enter:
                    if (collider.TryFindComponent(out camera))
                    {
                        currentSourceObject.transform = camera.transform;
                        increaseWeight = true;

                        if (aimConstraint && rigBuilder)
                        {
                            WeightedTransformArray sourceObjects = aimConstraint.data.sourceObjects;

                            bool found = false;

                            for (int i = 0; i < sourceObjects.Count && !found; i++)
                                if (sourceObjects[i].transform.name == currentSourceObject.transform.name)
                                {
                                    sourceObjects.SetWeight(i, currentSourceObject.weight);
                                    aimConstraint.data.sourceObjects = sourceObjects;

                                    found = true;
                                }

                            if(!found)
                            {
                                sourceObjects.Add(currentSourceObject);
                                aimConstraint.data.sourceObjects = sourceObjects;
                            }

                            rigBuilder.Build();
                        }
                    }

                    break;

                case TriggerEventType.Exit:
                    if (collider.TryFindComponent(out camera) && camera.transform == currentSourceObject.transform)
                        increaseWeight = false;

                    break;
            }
        }

        void UpdateSourceWeight(float delta)
        {
            currentSourceObject.weight = Mathf.Clamp01(currentSourceObject.weight + delta);

            if (aimConstraint && rigBuilder)
            {
                WeightedTransformArray sourceObjects = aimConstraint.data.sourceObjects;

                for (int i = 0; i < sourceObjects.Count; i++)
                    if (sourceObjects[i].transform.name == currentSourceObject.transform.name)
                    {
                        sourceObjects.SetWeight(i, currentSourceObject.weight);

                        aimConstraint.data.sourceObjects = sourceObjects;
                        rigBuilder.Build();

                        break;
                    }
            }
        }
    }
}