using System.Collections.Generic;
using UnityEngine;

namespace Devloader.Effects
{
    public class EffectBeviourDispatcher : MonoBehaviour
    {
        [SerializeField] List<AbstractEffect> _behaviours = new List<AbstractEffect>();

        public virtual void Run() => _behaviours.ForEach(b => b.SetToBegin().Run());
        public virtual void Pause() => _behaviours.ForEach(b => b.SetToBegin(AbstractEffect.EffectDirection.Pause).Run());

        public virtual void LoopMethod(AbstractEffect.EffectLoopMethod method) => _behaviours.ForEach(b => b.LoopMethod = method);
        public virtual void LoopMethod(int method) => _behaviours.ForEach(b => b.LoopMethod = (AbstractEffect.EffectLoopMethod) method);
    }
}