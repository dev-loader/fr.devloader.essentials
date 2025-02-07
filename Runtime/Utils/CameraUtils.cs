using UnityEngine;

namespace Devloader.Utils
{
    public class CameraUtils : MonoBehaviour
    {
        public static Camera Active => Camera.main ?? Camera.current;
    }
}