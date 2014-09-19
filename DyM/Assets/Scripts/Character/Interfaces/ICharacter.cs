using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Assets.Scripts.Abilities.Interfaces;
using Assets.Scripts.Utilities.Messaging;
using Assets.Scripts.Utilities.Messaging.Interfaces;
using Assets.Scripts.Weapons;
using Assets.Scripts.Weapons.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Character.Interfaces
{
	public interface ICharacter : IOwner
	{
		IWeapon Weapon { get; }
		IAbility Ability { get; }
		Vector3 Position { get; set; }
		bool Equipped();
		void Equip(IWeapon weapon);
		void Equip(IAbility ability);
		void SwitchWeapon();
		void AddWeapon(BaseWeapon weapon);
	}
}
