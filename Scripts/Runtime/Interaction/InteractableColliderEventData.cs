using System.Collections;
using UnityEngine;

namespace Devloader.Interaction
{
    public struct InteractableColliderEventData
    {
        public bool isInCollider;

        public Collider other;
        public Vector3 closestPoint;

        public InteractableColliderEventData(bool isInCollider, Collider other, Vector3 closestPoint)
        {
            this.isInCollider = isInCollider;
            this.other = other;
            this.closestPoint = closestPoint;
        }
    }
}