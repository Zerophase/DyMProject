using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Assets.Scripts.Abilities.Interfaces;
using Assets.Scripts.StatusEffects;
using Assets.Scripts.Utilities.Messaging;
using Assets.Scripts.Utilities.Messaging.Interfaces;
using Assets.Scripts.Weapons;
using Assets.Scripts.Weapons.Interfaces;
using Assets.Scripts.Weapons.Bases;
using UnityEngine;

namespace Assets.Scripts.Character.Interfaces
{
	public interface ICharacter : IOwner
	{
		IRangeWeapon RangeWeapon { get; }
		IMeleeWeapon MeleeWeapon { get; }
		IAbility Ability { get; }
		StatusEffect StatusEffect { get; }
		Vector3 Position { get; set; }
		int Health { get; }
		bool EquippedRangeWeapon();
		bool EquippedMeleeWeapon();
		bool EquippedAbility();
		void Equip(IRangeWeapon rangeweapon);
		void Equip(IMeleeWeapon meleeWeapon);
		void Equip(IAbility ability);
		void SwitchWeapon();
		void AddWeapon(RangeWeaponBase weapon);
		void TakeDamage(int healthLost);
	}
}
