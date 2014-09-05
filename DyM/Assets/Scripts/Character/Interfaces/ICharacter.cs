using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Assets.Scripts.Weapons;
using Assets.Scripts.Weapons.Interfaces;

namespace Assets.Scripts.Character.Interfaces
{
	public interface ICharacter
	{
		IWeapon Weapon { get; }
		void Equip(IWeapon weapon);
		void SwitchWeapon();
		void AddWeapon(BaseWeapon weapon);
	}
}
