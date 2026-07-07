/// Copyright 2025, Antonin Boureau, All rights reserved.
/// Version 20250206

using Devloader.Extensions;

using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace Devloader.AINavigation
{
    public class NavMeshAgentStateEvents : MonoBehaviour
    {
        [SerializeField] NavMeshAgent agent;
        [Space]

        bool moving;
        [SerializeField] UnityEvent OnAgentStartMoving = new UnityEvent();
        [SerializeField] UnityEvent OnAgentStopMoving = new UnityEvent();

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (!agent)
                agent = this.ValidateComponent<NavMeshAgent>();
        }
#endif

        private void FixedUpdate()
        {
            if (agent.velocity == Vector3.zero && moving)
            {
                OnAgentStopMoving.Invoke();
                moving = false;
            }
            else if (agent.velocity != Vector3.zero && !moving)
            {
                OnAgentStartMoving.Invoke();
                moving = true;
            }
        }
    }
}