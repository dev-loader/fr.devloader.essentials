/// Copyright 2026, Antonin Boureau, All rights reserved.
/// Version 20260603

using Devloader.Extensions;
using UnityEngine;

namespace Devloader.Effects
{
    [AddComponentMenu("Devloader/Effects/Fade AudioSource Volume")]
    public class FadeAudioSourceVolume : AbstractEffect
    {
        [SerializeField] AudioSource _audioSource;

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            if (!_audioSource)
                _audioSource = this.ValidateComponent<AudioSource>();
        }
#endif

        private void Awake() => ProcessAction = value => _audioSource.volume = value;
    }
}