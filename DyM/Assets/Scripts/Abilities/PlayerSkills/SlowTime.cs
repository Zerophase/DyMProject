using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.StatusEffects;
using Assets.Scripts.Utilities.Messaging.Interfaces;
using ModestTree.Zenject;

namespace Assets.Scripts.Abilities.PlayerSkills
{
    public class SlowTime : AbilityBase
    {
        [Inject]
        public SlowTime(IMessageDispatcher messageDispatcher)
            : base(messageDispatcher)
        {
            statusEffect = StatusEffect.SLOW_TIME;
        }
    }
}
