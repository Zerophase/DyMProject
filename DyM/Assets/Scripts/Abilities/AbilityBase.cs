using System.Runtime.Remoting.Messaging;
using Assets.Scripts.Abilities.Interfaces;
using Assets.Scripts.Character;
using Assets.Scripts.StatusEffects;
using Assets.Scripts.Utilities.Messaging;
using Assets.Scripts.Utilities.Messaging.Interfaces;
using Assets.Scripts.Character.Interfaces;
using UnityEngine;
using Assets.Scripts.UI;
using ModestTree.Zenject;

namespace Assets.Scripts.Abilities
{
	public abstract class AbilityBase : IAbility
	{
		private IMessageDispatcher messageDispatcher;
		protected IMessageDispatcher MessageDispatcher { get { return messageDispatcher; } }
		protected StatusEffect statusEffect;

		protected EnemyCharacter enemyCharacter = new EnemyCharacter();
		private ICharacter playerCharacter;
		public ICharacter PlayerCharacter { set {playerCharacter = value;}}

		private AbilityTypes abilityType;
		public AbilityTypes AbilityType { get { return abilityType; } }

		private GameObject playerGameobject;
		public GameObject PlayerGameobject {set {playerGameobject = value;}}
		
		private static float cooldown = 5f;
		private static float cooldownLeft = 5f;

		private float timeLimit = 2f;
		private float timeLimitLeft = 2f;

		[Inject]
		private IEntityManager entityManager;

		public AbilityBase(IMessageDispatcher messageDispatcher, AbilityTypes abilityTypes)
		{
			this.messageDispatcher = messageDispatcher;
			this.abilityType = abilityTypes;
		}

		public void Activate(ICharacter character)
		{
			cooldownLeft = 0f;
			timeLimitLeft = 0f;
			SendOutAbilityEffect();
		}

		public virtual void SendOutAbilityEffect()
		{
			messageDispatcher.DispatchMessage(new Telegram(enemyCharacter, statusEffect, true));
		}

		public virtual bool CoolDown()
		{
			if (cooldown <= cooldownLeft)
			{
				return true;
			}
			else if(cooldownLeft < cooldown)
			{
				calculateCooldown();
			}

			return false;
		}

		protected void calculateCooldown()
		{
			cooldownLeft += Time.deltaTime;
			AbilityMessage message = new AbilityMessage(cooldownLeft);
			messageDispatcher.DispatchMessage(new Telegram(entityManager.GetEntityFromID(Entities.HUD, 1), message));
		}

		public void TimeLimit()
		{
			if (timeLimit <= timeLimitLeft)
			{
				
			}
			else if(timeLimitLeft < timeLimit)
			{
				timeLimitLeft += Time.deltaTime;
			}
		}
	}
}
