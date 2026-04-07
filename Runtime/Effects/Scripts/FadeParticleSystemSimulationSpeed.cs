using Devloader.Effects;
using UnityEngine;

public class FadeParticleSystemSimulationSpeed : AbstractEffect
{
    [SerializeField] ParticleSystem[] _particleSystems = new ParticleSystem[0];

    [Space]
    [SerializeField] float _initialValue = .1f;
    [SerializeField] float _finalValue = 1f;

    private void Awake() => processAction = value =>
    {
        foreach(ParticleSystem particleSystem in _particleSystems)
        {
            ParticleSystem.MainModule module = particleSystem.main;
            module.simulationSpeed = Mathf.Lerp(_initialValue, _finalValue, value);
        }
    };
}
