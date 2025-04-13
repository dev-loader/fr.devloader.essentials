/// Copyright 2025, Antonin Boureau, All rights reserved.
/// Version 20250413

using Devloader.ColliderManagement;
using Devloader.Extensions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations.Rigging;

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

        RigBuilder rigBuilder;

        Dictionary<string, Coroutine> coroutines = new Dictionary<string, Coroutine>();

        private void OnValidate()
        {
            CheckAimConstraint();
            CheckColliderSettings();
        }

        private void OnEnable()
        {
            ColliderEventHandler eventHandler = this.ValidateComponent<ColliderEventHandler>();
            eventHandler.triggerEnterEvent.AddListener(ColliderTriggerEnterHandler);
            eventHandler.triggerExitEvent.AddListener(ColliderTriggerExitHandler);

            if (aimConstraint)
                rigBuilder = aimConstraint.FindComponentInParent<RigBuilder>();
        }

        private void OnDisable()
        {
            ColliderEventHandler eventHandler = this.ValidateComponent<ColliderEventHandler>();
            eventHandler.triggerEnterEvent.RemoveListener(ColliderTriggerEnterHandler);
            eventHandler.triggerExitEvent.RemoveListener(ColliderTriggerExitHandler);
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

        void ColliderTriggerEnterHandler(Collider collider)
        {
            if (collider.GetComponent<CharacterController>() && collider.TryFindComponent(out Camera camera))
            {
                if (aimConstraint && rigBuilder)
                {
                    int i = FindTransformIndex(camera.name);

                    if (i < 0)
                    {
                        WeightedTransformArray transforms = aimConstraint.data.sourceObjects;
                        transforms.Add(new WeightedTransform(camera.transform, 0));

                        aimConstraint.data.sourceObjects = transforms;
                        rigBuilder.Build();
                    }

                    if(coroutines.ContainsKey(camera.name))
                    {
                        StopCoroutine(coroutines[camera.name]);
                        coroutines[camera.name] = StartCoroutine(IncreaseWeight(camera.name));
                    }
                    else
                        coroutines.Add(camera.name, StartCoroutine(IncreaseWeight(camera.name)));
                }
            }
        }

        void ColliderTriggerExitHandler(Collider collider)
        {
            if (collider.GetComponent<CharacterController>() && collider.TryFindComponent(out Camera camera))
            {
                if (coroutines.ContainsKey(camera.name))
                {
                    StopCoroutine(coroutines[camera.name]);
                    coroutines[camera.name] = StartCoroutine(DecreaseWeight(camera.name));
                }
                else
                    coroutines.Add(camera.name, StartCoroutine(DecreaseWeight(camera.name)));
            }
        }

        int FindTransformIndex(string transformName)
        {
            if (!aimConstraint || !rigBuilder)
                return -1;

            List<WeightedTransform> sourceObjectsList = aimConstraint.data.sourceObjects.ToList();
            return sourceObjectsList.FindIndex(s => s.transform.name == transformName);
        }

        IEnumerator DecreaseWeight(string transformName)
        {
            if (!aimConstraint || !rigBuilder)
                yield break;

            /// Recherche de l'objet
            int i = FindTransformIndex(transformName);

            /// Si objet non trouvé, on arrête la coroutine
            if (i < 0)
                yield break;

            while (aimConstraint.data.sourceObjects.GetWeight(i) > 0)
            {
                WeightedTransformArray transforms = aimConstraint.data.sourceObjects;
                transforms.SetWeight(i, Mathf.Clamp01(aimConstraint.data.sourceObjects.GetWeight(i) - Time.deltaTime / weightTransitionDuration));

                aimConstraint.data.sourceObjects = transforms;
                rigBuilder.Build();

                yield return null;
            }
        }

        IEnumerator IncreaseWeight(string transformName)
        {
            if(!aimConstraint || !rigBuilder)
                yield break;

            /// Recherche de l'objet
            int i = FindTransformIndex(transformName);

            /// Si objet non trouvé, on arrête la coroutine
            if (i < 0)
                yield break;

            while(aimConstraint.data.sourceObjects.GetWeight(i) < 1)
            {
                WeightedTransformArray transforms = aimConstraint.data.sourceObjects;
                transforms.SetWeight(i, Mathf.Clamp01(aimConstraint.data.sourceObjects.GetWeight(i) + Time.deltaTime / weightTransitionDuration));
                
                aimConstraint.data.sourceObjects = transforms;
                rigBuilder.Build();

                yield return null;
            }
        }
    }
}