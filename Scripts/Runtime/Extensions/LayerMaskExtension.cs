using UnityEngine;

namespace Devloader.Extensions
{
    public static class LayerMaskExtension
    {
        public static bool Ignores(this LayerMask layerMask, int layer) => ((1 << layer) & layerMask) == 0;

        public static bool Includes(this LayerMask layerMask, int layer) => ((1 << layer) & layerMask) != 0;
    }
}