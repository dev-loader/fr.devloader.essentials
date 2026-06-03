/// Copyright 2026, Antonin Boureau, All rights reserved.
/// Version 20260603

using Devloader.Extensions;
using Devloader.Maths;
using UnityEngine;

namespace Devloader.Effects
{
    [AddComponentMenu("Devloader/Effects/Fade ParticleSystem SimulationSpeed")]
    public class FadeParticleSystemSimulationSpeed : AbstractEffect
    {
        [SerializeField] ParticleSystem[] _particleSystems = new ParticleSystem[0];
        [SerializeField] RangedFloat _simulationSpeedInterval = new RangedFloat(.1f, 1f);

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            if (_particleSystems.Length == 0)
                _particleSystems = this.FindComponents<ParticleSystem>();
        }
#endif

        private void Awake() => ProcessAction = value =>
        {
            foreach (ParticleSystem particleSystem in _particleSystems)
            {
                ParticleSystem.MainModule module = particleSystem.main;
                module.simulationSpeed = Mathf.Lerp(_simulationSpeedInterval.a, _simulationSpeedInterval.b, value);
            }
        };
    }
}