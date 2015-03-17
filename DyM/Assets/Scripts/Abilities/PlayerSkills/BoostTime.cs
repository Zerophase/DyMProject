using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Utilities.Messaging.Interfaces;
using Assets.Scripts.StatusEffects;

namespace Assets.Scripts.Abilities.PlayerSkills
{
    public class BoostTime : AbilityBase
    {
        public BoostTime(IMessageDispatcher messageDispatcher)
            :base(messageDispatcher)
        {
            statusEffect = StatusEffect.BOOST_TIME;
        }
    }
}
