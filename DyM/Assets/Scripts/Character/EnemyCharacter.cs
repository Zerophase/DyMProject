﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Abilities;
using Assets.Scripts.Abilities.Interfaces;
using Assets.Scripts.Character.Interfaces;
using Assets.Scripts.StatusEffects;
using Assets.Scripts.Utilities.Messaging.Interfaces;
using Assets.Scripts.Weapons.Bases;
using Assets.Scripts.Weapons.Interfaces;
using ModestTree.Zenject;
using UnityEngine;
using Assets.Scripts.Projectiles.Interfaces;

namespace Assets.Scripts.Character
{
	public class EnemyCharacter : ICharacter
	{
        [Inject]
        private IBulletPool bulletPool;

		public IRangeWeapon RangeWeapon
		{
			get { throw new NotImplementedException(); }
		}

		public IMeleeWeapon MeleeWeapon
		{
			get { throw new NotImplementedException(); }
		}

		public IAbility Ability
		{
			get { throw new NotImplementedException(); }
		}

		private StatusEffect statusEffect;
		public StatusEffect StatusEffect
		{
			get { return statusEffect; }
		}

		private IReceiver receiver;
		public IReceiver Receiver
		{
			set { receiver = value; }
		}

		public Vector3 Position
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public int Health
		{
			get { throw new NotImplementedException(); }
		}

		public EnemyCharacter()
		{
			
		}

		[Inject]
		public EnemyCharacter(IReceiver receiver)
		{
			this.receiver = receiver;
			this.receiver.Owner = this;
			receiver.SubScribe();
		}

		public void PostConstruction()
		{
			throw new NotImplementedException();
		}

		public bool EquippedRangeWeapon()
		{
			throw new NotImplementedException();
		}

		public bool EquippedMeleeWeapon()
		{
			throw new NotImplementedException();
		}

		public bool EquippedAbility(AbilityTypes abilityType)
		{
			throw new NotImplementedException();
		}

		public void Equip(IRangeWeapon rangeweapon)
		{
            bulletPool.ChangeBullet(rangeweapon);
		}

		public void Equip(IMeleeWeapon meleeWeapon)
		{
			throw new NotImplementedException();
		}

		public void Equip(IAbility ability)
		{
			throw new NotImplementedException();
		}

		public void SwitchWeapon()
		{
			throw new NotImplementedException();
		}

		public void AddWeapon(RangeWeaponBase weapon)
		{
            weapon.Character = this;
		}

		public void TakeDamage(int healthLost)
		{
			throw new NotImplementedException();
		}

		public void Receive(ITelegram telegram)
		{
			if(telegram.Message is StatusEffect)
				GainStatusEffect((StatusEffect)telegram.Message);
		}

		public void GainStatusEffect(StatusEffect statusEffect)
		{
			this.statusEffect |= statusEffect;
		}

		public void RemoveStatusEffect()
		{
			if(timeLimit())
				this.statusEffect = StatusEffect.NONE;
		}

		private float timeLimitTotal = 2f;
		private float timeLimitLeft;
		private bool timeLimit()
		{
			if (timeLimitTotal <= timeLimitLeft)
			{
				timeLimitLeft = 0f;
				return true;
			}
			else if (timeLimitLeft < timeLimitTotal)
			{
				timeLimitLeft += Time.deltaTime;
			}

			return false;
		}


		public void SendOutStats()
		{
			throw new NotImplementedException();
		}


		public void Heal(int healthGained)
		{
			throw new NotImplementedException();
		}
		public void AddScore(int scoreValue)
		{
			throw new NotImplementedException();
		}

		private CharacterTypes characterType;
		public CharacterTypes CharacterType
		{
			get { return characterType; }
			set { characterType = value; }
		}
	}
}
