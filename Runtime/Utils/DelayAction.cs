using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Devloader.Utils
{
    public class DelayAction : MonoBehaviour
    {
        [SerializeField] private float delay;
        [Space]

        [SerializeField] private UnityEvent action = new UnityEvent();

        public void RunDelay()
        {
            StopAllCoroutines();
            StartCoroutine(DelayCoroutine(delay));
        }

        public void RunDelay(float delay)
        {
            StopAllCoroutines();
            StartCoroutine(DelayCoroutine(delay));
        }

        public IEnumerator DelayCoroutine(float delay)
        {
            if (delay <= 0)
                yield break;

            float t = 0;

            while(t < delay)
            {
                t += Time.deltaTime;
                yield return null;
            }

            action.Invoke();
            yield break;
        }
    }
}