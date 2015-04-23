using System;
using System.Collections.Generic;
using Assets.Scripts.Character.Interfaces;
using Assets.Scripts.CustomInputManager;
using Assets.Scripts.DependencyInjection;
using Assets.Scripts.MediatorPattern;
using Assets.Scripts.ObjectManipulation;
using Assets.Scripts.ObjectManipulation.Interfaces;
using Assets.Scripts.Projectiles;
using Assets.Scripts.Projectiles.Interfaces;
using Assets.Scripts.Utilities;
using ModestTree.Zenject;
using UnityEngine;
using System.Collections;
using Assets.Scripts.StatusEffects;
using Assets.Scripts.Utilities.Messaging;
using Assets.Scripts.Utilities.Messaging.Interfaces;

namespace Assets.Scripts.GameObjects
{
	public partial class Player : MovablePhysicsMediator
	{
		private IPlaneShift planeShift;
		[Inject]
		private PlaneShiftFactory factory;

		[Inject]
		public ICharacter character;

		[Inject]
		public IPooledGameObjects PooledBulletGameObjects;

		public static GameObject GunModel;

        private Vector3 acceleration = new Vector3(20f,0f,0f);

		private Animator animator;

		private bool idle;

		private float weaponSwitchTimer = 0.0f;

		public int Health { get { return character.Health; } }

		public int TouchGroundFrameCount;

		private Gun gun;

		private List<AudioSource> audioSources = new List<AudioSource>();

		private ParticleSystem particleSystem;

		protected override void Start()
		{
			planeShift = factory.Create(transform.position);
			GunModel = GameObject.FindGameObjectWithTag("EquippedGun");
			gun = GameObject.FindWithTag("GunRotator").GetComponent<Gun>();

			PooledBulletGameObjects.Initialize();

			animator = GetComponent<Animator>();

			var aSources = GetComponents<AudioSource>();
			for (int i = 0; i < aSources.Length; i++)
			{
				audioSources.Add(aSources[i]);
			}

			character.PostConstruction();
			character.SendOutStats();

			particleSystem = GameObject.Find("PlaneShiftParticle").GetComponent<ParticleSystem>();

			base.Start();
		}

		protected void Update()
		{
			endScreen();
			
			switchPlane();

			move();
			
			flip(gun.Direction.checkDirection());

			rangeAttack();

			weakAttack();

			activateAbility();

			switchWeapon();
		}

        // TODO move out of player
		private void endScreen()
		{
			if (transform.position.y < -40)
			{
				Application.LoadLevel("GameOver");
			}
		}

		private Vector3 previousPlane;
		private void switchPlane()
		{
			if (InputManager.PlaneShiftDown())
			{
				var changePlane = planeShift.ShiftPlane(KeyCode.Joystick1Button4,
					transform.position);
				previousPlane = transform.position;
				UpdatePlane(changePlane);

				if (transform.position != previousPlane)
				{
					particleSystem.transform.position = transform.position;
					particleSystem.Play();
				}
			}
			else if (InputManager.PlaneShiftUp())
			{
				var changePlane = planeShift.ShiftPlane(KeyCode.Joystick1Button5,
					transform.position);
				previousPlane = transform.position;
				UpdatePlane(changePlane);
				if (transform.position != previousPlane)
				{
					particleSystem.transform.position = transform.position;
					particleSystem.Play();
				}
			}

			// TODO put back in when we have a reason for a dodge.
			//UpdatePlane(planeShift.Dodge(transform.position, InputManager.CheckDodgeKeys(), Time.deltaTime));
		}

		private bool jumped;
		private float previousYPosition;
		private void move()
		{
			if (Input.GetAxis("LockPosition") == 0f)
			{
				speed = InputManager.MovementHorizontal();
			}
				

			if (Util.compareEachFloat(speed, 0.0f))
			{
				idle = true;
			}
			else
				idle = false;

			animator.SetBool("Idle", idle);
            animator.SetFloat("Speed", speed);
            
			if (!audioSources[0].isPlaying && (speed > 0.1f || speed < -0.1f))
				audioSources[0].Play();
			if(!audioSources[1].isPlaying && InputManager.Jump())
            {
                
                audioSources[1].Play();
            }
				
			if (!audioSources[2].isPlaying && InputManager.Jumping() && !Util.compareEachFloat(transform.position.y, previousYPosition))
			{
				audioSources[2].Play();
				jumped = audioSources[2].isPlaying;
			}
			else if (audioSources[2].isPlaying && Util.compareEachFloat(transform.position.y, previousYPosition))
			{
				audioSources[2].Stop();
				if (!audioSources[3].isPlaying && jumped && Util.compareEachFloat(transform.position.y, previousYPosition))
				{
					jumped = false;
					audioSources[3].Play();
				}
			}

            animator.SetBool("Jumped", jumped);

			previousYPosition = transform.position.y;
			statusEffect();
            UpdateVelocity(cardinalMovement.CalculateTotalMovement(speed,
                acceleration * movementMultiplier, InputManager.Jumping(), transform.localPosition));
		}

		private void statusEffect()
		{
			var statusEffect = character.StatusEffect;
			if (statusEffect == StatusEffect.NONE)
				movementMultiplier = 1.0f;
			else if ((statusEffect & StatusEffect.BOOST_TIME) ==
				StatusEffect.BOOST_TIME)
			{
				movementMultiplier = 1.5f;
			}

			if (character.StatusEffect != StatusEffect.NONE)
			{
				character.RemoveStatusEffect();
			}
		}

        private bool shot;
		private void rangeAttack()
		{
            if (character.EquippedRangeWeapon() && InputManager.Fire())
            {
                shot = character.RangeWeapon.FireRate(Time.deltaTime);
                animator.SetBool("Shot", shot);
                if (shot)
                {
                    IProjectile bullet = character.RangeWeapon.Fire();
                    bullet.ShotDirection = -GunModel.transform.right;
	                var bulletInstance = PooledBulletGameObjects.GetPooledBullet().GetComponent<Bullet>();
	                bulletInstance.Projectile = bullet;
					bulletInstance.Initialize();
					messageDispatcher.DispatchMessage(new Telegram(bulletInstance, GunModel.transform));
                }
            }
            
		}

		private void weakAttack()
		{
			if (InputManager.WeakAttack() && character.EquippedRangeWeapon())
			{
				character.MeleeWeapon.Attack();
			}
		}

		private void activateAbility()
		{
            
			if (character.EquippedAbility(InputManager.ActivateAbility()) && 
				character.Ability.CoolDown())
			{
				character.Ability.Activate(character);
			}
		}

		private void switchWeapon()
		{
			if (InputManager.SwitchWeapon() &&
			    character.EquippedRangeWeapon())
			{
				character.SwitchWeapon();
			}
		}

		public void TakeDamage(int healthLost)
		{
			character.TakeDamage(healthLost);
		}

		public void Heal(int healthGained)
		{
			character.Heal(healthGained);
		}

		public void AddScore(int scoreValue)
		{
			character.AddScore(scoreValue);
		}

		void LateUpdate()
		{
			gun.Rotate();
		}
	}
}
