using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Devloader.Utils
{
    public class DelayAction : MonoBehaviour
    {
        [SerializeField] private float _delay;
        [SerializeField] private bool _useFixedUpdate;
        [Space]

        [SerializeField] private UnityEvent _action = new UnityEvent();

        public void RunDelay()
        {
            StopAllCoroutines();
            StartCoroutine(DelayCoroutine(_delay, () => _action.Invoke()));
        }

        public void RunDelay(float delay)
        {
            StopAllCoroutines();
            StartCoroutine(DelayCoroutine(delay, () => _action.Invoke()));
        }

        public void RunDelay(float delay, UnityAction action)
        {
            StopAllCoroutines();
            StartCoroutine(DelayCoroutine(delay, action));
        }

        public IEnumerator DelayCoroutine(float delay, UnityAction action = null)
        {
            if (delay <= 0 || action == null)
                yield break;

            float t = 0;

            while(t < delay)
            {
                t += Time.deltaTime;

                if(_useFixedUpdate)
                    yield return new WaitForFixedUpdate();
                else
                    yield return null;
            }

            action.Invoke();
            yield break;
        }
    }
}