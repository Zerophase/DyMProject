using Assets.Scripts.Character.Interfaces;
using Assets.Scripts.GameObjects;
using UnityEngine;

namespace Assets.Scripts.Abilities.Interfaces
{
	public interface IAbility
	{
		void Activate(ICharacter character);
		bool CoolDown();
		void TimeLimit();
		ICharacter PlayerCharacter {set;}
		GameObject PlayerGameobject {set;}
	}
}
