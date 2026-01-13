/// Copyright 2026, Antonin Boureau, All rights reserved.
/// Version 20260113

using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Devloader.Events
{
    [Serializable]
    public class DelayedAction
    {
        [SerializeField] private UnityEvent _action = new UnityEvent();

        [Space]
        [SerializeField] private float _delay = 1;
        [SerializeField] private bool _useFixedUpdate;

        private CancellationTokenSource _cancellationTokenSource;

        /// <summary>
        /// Annule l'action différée en cours
        /// </summary>
        public void Cancel()
        {
            if (_cancellationTokenSource != null)
            {
                _cancellationTokenSource.Cancel();
                _cancellationTokenSource.Dispose();
                _cancellationTokenSource = null;
            }
        }

        /// <summary>
        /// Annule toute action en cours d'attente et lance une nouvelle action différée avec les paramètres tels que renseignés dans l'inspecteur
        /// </summary>
        public async void Invoke()
        {
            Cancel();
            _cancellationTokenSource = new CancellationTokenSource();

            try
            {
                await InvokeAsync(_delay, () => _action.Invoke(), _cancellationTokenSource.Token);
            }
            catch (OperationCanceledException)
            { }
        }

        /// <summary>
        /// Annule toute action en cours d'attente et lance une nouvelle action différée avec le délai indiqué
        /// </summary>
        public async void Invoke(float delay)
        {
            Cancel();
            _cancellationTokenSource = new CancellationTokenSource();

            try
            {
                await InvokeAsync(delay, () => _action.Invoke(), _cancellationTokenSource.Token);
            }
            catch (OperationCanceledException)
            { }
        }

        /// <summary>
        /// Annule toute action en cours d'attente et lance une nouvelle action différée avec le délai et l'action à effectuer indiqué
        /// </summary>
        public async void Invoke(float delay, UnityAction action)
        {
            Cancel();
            _cancellationTokenSource = new CancellationTokenSource();

            try
            {
                await InvokeAsync(delay, action, _cancellationTokenSource.Token);
            }
            catch (OperationCanceledException)
            { }
        }

        private async Task InvokeAsync(float delay, UnityAction action, CancellationToken cancellationToken)
        {
            if (delay <= 0 || action == null)
                return;

            float elapsed = 0;

            while (elapsed < delay)
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (_useFixedUpdate)
                {
                    await Task.Yield();
                    elapsed += Time.fixedDeltaTime;
                }
                else
                {
                    await Task.Yield();
                    elapsed += Time.deltaTime;
                }
            }

            cancellationToken.ThrowIfCancellationRequested();
            action.Invoke();
        }

        public int count => _action.GetPersistentEventCount();
    }
}