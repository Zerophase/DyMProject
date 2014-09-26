using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Abilities;
using Assets.Scripts.Abilities.Interfaces;
using Assets.Scripts.Character.Interfaces;
using Assets.Scripts.StatusEffects;
using Assets.Scripts.Utilities.Messaging;
using Assets.Scripts.Utilities.Messaging.Interfaces;
using Assets.Scripts.Weapons;
using Assets.Scripts.Weapons.Interfaces;
using ModestTree.Zenject;
using UnityEngine;

namespace Assets.Scripts.Character
{
	public class TestCharacter : ICharacter
	{
		private List<BaseWeapon> weapons = new List<BaseWeapon>(); 

		private IRangeWeapon weapon;
		public IRangeWeapon Weapon
		{
			get { return weapon; }
		}

		private IAbility ability;

		public IAbility Ability
		{
			get { return ability; }
		}

		private StatusEffect statusEffect;
		public StatusEffect StatusEffect { get { return statusEffect; } }
		private IReceiver receiver;

		public IReceiver Receiver
		{
			set { receiver = value; }
		}
		public Vector3 Position { get; set; }

		public TestCharacter()
		{
			
		}

		[Inject]
		public TestCharacter(IReceiver receiver)
		{
			this.receiver = receiver;
			this.receiver.Owner = this;
			receiver.SubScribe();
		}

		public bool EquippedWeapon()
		{
			return weapon != null;
		}

		public bool EquippedAbility()
		{
			return ability != null;
		}

		public void Equip(IWeapon weapon)
		{
			this.weapon = weapon;
		}

		public void Equip(IAbility ability)
		{
			this.ability = ability;
		}

		public void Receive(ITelegram telegram)
		{
			if(telegram.Message is BaseWeapon)
				AddWeapon((TestWeapon)telegram.Message);
			else if(telegram.Message is AbilityBase)
				Equip((Ability)telegram.Message);
			else if(telegram.Message is StatusEffect)
				GainStatusEffect((StatusEffect)telegram.Message);
		}

		public void SwitchWeapon()
		{
			weapons.Sort();
			Equip(weapons.Find(x => x != weapon));
		}

		public void AddWeapon(BaseWeapon weapon)
		{
			weapons.Add(weapon);
			if(weapons.Count < 2)
				Equip(weapon);
		}

		public void GainStatusEffect(StatusEffect statusEffect)
		{
			this.statusEffect |= statusEffect;
		}
	}
}
