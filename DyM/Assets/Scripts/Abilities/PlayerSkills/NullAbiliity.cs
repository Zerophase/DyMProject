using Assets.Scripts.Abilities.Interfaces;
using Assets.Scripts.StatusEffects;
using Assets.Scripts.Utilities.Messaging.Interfaces;

namespace Assets.Scripts.Abilities.PlayerSkills
{
	public class NullAbiliity : AbilityBase
	{
		public NullAbiliity(IMessageDispatcher messageDispatcher) 
			: base(messageDispatcher, AbilityTypes.NONE)
		{
			statusEffect = StatusEffect.NONE;
		}

		public override bool CoolDown()
		{
			calculateCooldown();
			return false;
		}
	}
}