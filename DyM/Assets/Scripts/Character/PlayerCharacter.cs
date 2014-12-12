using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Abilities;
using Assets.Scripts.Abilities.Interfaces;
using Assets.Scripts.Character.Interfaces;
using Assets.Scripts.Projectiles.Interfaces;
using Assets.Scripts.StatusEffects;
using Assets.Scripts.Utilities.Messaging;
using Assets.Scripts.Utilities.Messaging.Interfaces;
using Assets.Scripts.Weapons;
using Assets.Scripts.Weapons.Interfaces;
using Assets.Scripts.Weapons.Bases;
using ModestTree.Zenject;
using UnityEngine;

namespace Assets.Scripts.Character
{
	public class PlayerCharacter : ICharacter
	{
        private IEntityManager entityManager;

		private List<RangeWeaponBase> rangeWeapons = new List<RangeWeaponBase>();

		[Inject]
		private IBulletPool bulletPool;

		private IRangeWeapon rangeWeapon;
		public IRangeWeapon RangeWeapon
		{
			get { return rangeWeapon; }
		}

		private IMeleeWeapon meleeWeapon;

		public IMeleeWeapon MeleeWeapon
		{
			get { return meleeWeapon; }
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

        private IIds id;

		private int health = 300;
		public int Health { get { return health; } }

		public Vector3 Position { get; set; }

		[Inject]
		private IMessageDispatcher messageDispatcher;

		public PlayerCharacter()
		{
			
		}

		[Inject]
		public PlayerCharacter(IReceiver receiver, IIds id, IEntityManager entityManager)
		{
            this.id = id;
            this.entityManager = entityManager;
            this.receiver = receiver;
			this.receiver.Owner = this;


            id.CreateId();
            entityManager.Add(Entities.CHARACTER, id.ObjectId, this);
			receiver.SubScribe();
		}

		public bool EquippedRangeWeapon()
		{
			return rangeWeapon != null;
		}

		public bool EquippedMeleeWeapon()
		{
			return meleeWeapon != null;
		}

		public bool EquippedAbility()
		{
			return ability != null;
		}

		public void Equip(IRangeWeapon weapon)
		{
			bulletPool.ChangeBullet(weapon);
			this.rangeWeapon = weapon;
		}

		public void Equip(IMeleeWeapon meleeWeapon)
		{
			this.meleeWeapon = meleeWeapon;
		}

		public void Equip(IAbility ability)
		{
			this.ability = ability;
		}

		public void Receive(ITelegram telegram)
		{
			if(telegram.Message is RangeWeaponBase)
				AddWeapon((RangeWeaponBase)telegram.Message);
			else if(telegram.Message is MeleeWeaponBase)
				Equip((TestMeleeWeapon)telegram.Message);
			else if(telegram.Message is AbilityBase)
				Equip((AbilityBase)telegram.Message);
			else if(telegram.Message is StatusEffect)
				GainStatusEffect((StatusEffect)telegram.Message);
		}

		public void SwitchWeapon()
		{
			rangeWeapons.Sort();
			if(rangeWeapons.Count > 1)
				Equip(rangeWeapons.Find(x => x != rangeWeapon));
		}

		public void AddWeapon(RangeWeaponBase weapon)
		{
			rangeWeapons.Add(weapon);
			if(rangeWeapons.Count < 2)
				Equip(weapon);
		}

		public void GainStatusEffect(StatusEffect statusEffect)
		{
			this.statusEffect |= statusEffect;
		}

		public void TakeDamage(int healthLost)
		{
			health -= healthLost;
			SendOutStats();
		}

		public void SendOutStats()
		{
			messageDispatcher.DispatchMessage(new Telegram(entityManager.GetEntityFromID(Entities.HUD, 1), health));
		}

		public void RemoveStatusEffect()
		{
			throw new NotImplementedException();
		}
	}
}
