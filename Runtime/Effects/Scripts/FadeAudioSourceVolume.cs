/// Copyright 2023, Antonin Boureau, All rights reserved.
/// Version 20230626

using UnityEngine;
using Devloader.Extensions;

namespace Devloader.Effects
{
    [AddComponentMenu("Devloader/Effects/FadeAudioSourceVolume")]
    public class FadeAudioSourceVolume : AbstractEffect
    {
        public AudioSource audioSource;

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            if (!audioSource)
                audioSource = this.ValidateComponent<AudioSource>();

            base.OnValidate();
        }
#endif

        private void Awake()
        {
            if (!audioSource)
                audioSource = this.ValidateComponent<AudioSource>();

            processAction = delegate (float value)
            { audioSource.volume = value; };
        }
    }
}

/// <summary>
/// Version 20230209
/// </summary>