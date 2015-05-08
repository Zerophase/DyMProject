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
using Assets.Scripts.GameObjects;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Assets.Scripts.Abilities.PlayerSkills;
using Assets.Scripts.Utilities;

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

		[Inject]
		public IAbility NullAbility;
		private List<IAbility> abilities = new List<IAbility>();
        private IAbility ability;
		public IAbility Ability
		{
            get { return abilities.First(); }
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
		public int Health { get { return health; } set { health = value; } }

		private int score = 0;
		public int Score { get {return score; } }

		public Vector3 Position { get; set; }

		[Inject]
		private IMessageDispatcher messageDispatcher;

		public PlayerCharacter()
		{
			statusEffect = StatusEffect.NONE;
		}

		[Inject]
		public PlayerCharacter(IReceiver receiver, IIds id, IEntityManager entityManager)
		{
            this.id = id;
            this.entityManager = entityManager;
            this.receiver = receiver;
			this.receiver.Owner = this;
			statusEffect = StatusEffect.NONE;

            id.CreateId();
            entityManager.Add(Entities.CHARACTER, id.ObjectId, this);
			receiver.SubScribe();
		}

		public void PostConstruction()
		{
			Equip(NullAbility);
		}

		public bool EquippedRangeWeapon()
		{
			return rangeWeapon != null;
		}

		public bool EquippedMeleeWeapon()
		{
			return meleeWeapon != null;
		}

		public bool EquippedAbility(AbilityTypes abilityType)
		{
			if (abilities.All(a => a == NullAbility))
				return false;
            bool activeAbility = abilities.Any(x => x.AbilityType == abilityType);
            abilities.Swap(0, 
				abilities.FindIndex(a => a.AbilityType == abilityType));
            return activeAbility;
		}

		// TODO Make Extension method
		
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
            if(abilities.All(x => x != ability))
                abilities.Add(ability);
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
            weapon.Character = this;
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

		public void Heal(int healthGain)
		{
			health += healthGain;
			SendOutStats();
		}

		public void AddScore(int scoreValue)
		{
			score += scoreValue;
			Debug.Log ("Score: " + score);
			SendOutStats();
		}

		public void SendOutStats()
		{
			HealthMessage healthMessage = new HealthMessage(health);
			messageDispatcher.DispatchMessage(new Telegram(entityManager.GetEntityFromID(Entities.HUD, 1), healthMessage));
		}

		public void RemoveStatusEffect()
		{
			if (timeLimit())
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

		private CharacterTypes characterType;
		public CharacterTypes CharacterType
		{
			get { return characterType; }
			set { characterType = value; }
		}
	}
}
