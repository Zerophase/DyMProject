using Assets.Scripts.Character.Interfaces;

namespace Assets.Scripts.Abilities.Interfaces
{
	public interface IAbility
	{
		void Activate(ICharacter character);
		void PickUp(ICharacter character);
	}
}
