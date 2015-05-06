﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Assets.Scripts.Abilities;
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
		CharacterTypes CharacterType { get; set; }
		void PostConstruction();
		IRangeWeapon RangeWeapon { get; }
		IMeleeWeapon MeleeWeapon { get; }
		IAbility Ability { get; }
		StatusEffect StatusEffect { get; }
		void GainStatusEffect(StatusEffect statusEffect);
		Vector3 Position { get; set; }
		int Health { get; }
		bool EquippedRangeWeapon();
		bool EquippedMeleeWeapon();
		bool EquippedAbility(AbilityTypes abilityType);
		void Equip(IRangeWeapon rangeweapon);
		void Equip(IMeleeWeapon meleeWeapon);
		void Equip(IAbility ability);
		void SwitchWeapon();
		void AddWeapon(RangeWeaponBase weapon);
		void TakeDamage(int healthLost);
		void Heal(int healthGained);
		void AddScore(int scoreValue);
		void SendOutStats();
		void RemoveStatusEffect();
	}
}
