using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Character;
using Assets.Scripts.Character.Interfaces;
using Assets.Scripts.Utilities.Messaging.Interfaces;
using Assets.Scripts.StatusEffects;
using Assets.Scripts.Utilities.Messaging;

namespace Assets.Scripts.Abilities.PlayerSkills
{
    public class BoostTime : AbilityBase
    {
	    private PlayerCharacter playerCharacter = new PlayerCharacter();

        public BoostTime(IMessageDispatcher messageDispatcher)
            :base(messageDispatcher, AbilityTypes.SPEED_UP_TIME)
        {
            statusEffect = StatusEffect.BOOST_TIME;
        }

	    public override void SendOutAbilityEffect()
	    {
		   MessageDispatcher.DispatchMessage
				(
					new Telegram(playerCharacter, statusEffect, true)
				);
			base.SendOutAbilityEffect();
	    }
    }
}
