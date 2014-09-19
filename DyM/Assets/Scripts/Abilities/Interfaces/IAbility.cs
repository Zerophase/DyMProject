using Assets.Scripts.Character.Interfaces;

namespace Assets.Scripts.Abilities.Interfaces
{
	public interface IAbility
	{
		IAbility Activate();
		void PickUp(ICharacter character);
	}
}
