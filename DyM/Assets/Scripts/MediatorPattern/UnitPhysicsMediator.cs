using Assets.Scripts.Character.Interfaces;
using ModestTree.Zenject;

namespace Assets.Scripts.MediatorPattern
{
	public abstract class UnitPhysicsMediator : MovablePhysicsMediator
	{
		[Inject]
		private ICharacter character;
		public ICharacter Character { get { return character; } }

		public abstract void TakeDamage(int healthLost);

		public int Health { get { return character.Health; } }

		public bool Die()
		{
			if (Health <= 0)
				return true;
			return false;
		}
	}
}