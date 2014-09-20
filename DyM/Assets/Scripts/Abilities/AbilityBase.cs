using System.Runtime.Remoting.Messaging;
using Assets.Scripts.Abilities.Interfaces;
using Assets.Scripts.StatusEffects;
using Assets.Scripts.Utilities.Messaging;
using Assets.Scripts.Utilities.Messaging.Interfaces;
using Assets.Scripts.Character.Interfaces;

namespace Assets.Scripts.Abilities
{
	public abstract class AbilityBase : IAbility
	{
		private IMessageDispatcher messageDispatcher;
		protected StatusEffect statusEffect;

		public AbilityBase(IMessageDispatcher messageDispatcher)
		{
			this.messageDispatcher = messageDispatcher;
		}

		public void Activate(ICharacter character)
		{
			messageDispatcher.DispatchMessage(new Telegram(character, statusEffect));
		}

		public void PickUp(ICharacter character)
		{
			messageDispatcher.DispatchMessage(new Telegram(character, this));
		}
	}
}
