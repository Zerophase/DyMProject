using System.Runtime.Remoting.Messaging;
using Assets.Scripts.Abilities.Interfaces;
using Assets.Scripts.Utilities.Messaging;
using Assets.Scripts.Utilities.Messaging.Interfaces;
using Assets.Scripts.Character.Interfaces;

namespace Assets.Scripts.Abilities
{
	public abstract class AbilityBase : IAbility
	{
		private IMessageDispatcher messageDispatcher;

		public AbilityBase(IMessageDispatcher messageDispatcher)
		{
			this.messageDispatcher = messageDispatcher;
		}

		public IAbility Activate()
		{
			throw new System.NotImplementedException();
		}

		public void PickUp(ICharacter character)
		{
			messageDispatcher.DispatchMessage(new Telegram(character, this));
		}
	}
}
