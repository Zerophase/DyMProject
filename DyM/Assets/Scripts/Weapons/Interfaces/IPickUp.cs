using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Character.Interfaces;

namespace Assets.Scripts.Weapons.Interfaces
{
	public interface IPickUp
	{
		void PickUp(ICharacter character);
	}
}
