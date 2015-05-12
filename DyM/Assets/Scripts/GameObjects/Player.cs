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
using Assets.Scripts.Weapons.Guns;
using Assets.Scripts.Utilities;
using ModestTree.Zenject;
using UnityEngine;
using System.Collections;
using Assets.Scripts.Character;
using Assets.Scripts.StatusEffects;
using Assets.Scripts.Utilities.Messaging;
using Assets.Scripts.Utilities.Messaging.Interfaces;
using Assets.Scripts.Weapons;

namespace Assets.Scripts.GameObjects
{
	public partial class Player : UnitPhysicsMediator
	{
		private IPlaneShift planeShift;
		[Inject]
		private PlaneShiftFactory factory;

		[Inject]
		public IPooledGameObjects PooledBulletGameObjects;

		public static GameObject GunModel;

        private Vector3 acceleration = new Vector3(20f,0f,0f);

		private Animator animator;

		private bool idle;

		private float weaponSwitchTimer = 0.0f;

		public int Health { get { return Character.Health; } }

		public int TouchGroundFrameCount;

		private Gun gun;

		private List<AudioSource> audioSources = new List<AudioSource>();

		private ParticleSystem particleSystem;

		[Inject]
		private RangeWeaponFactory rangeWeaponFactory;

		protected override void Start()
		{
			planeShift = factory.Create(transform.position);
			GunModel = GameObject.FindGameObjectWithTag("PlayerEquippedGun");
			gun = GameObject.FindWithTag("GunRotator").GetComponent<Gun>();

			// TODO remove once a better way of getting the gun info to the game is found
			var test = rangeWeaponFactory.Create(WeaponTypes.MACHINE_GUN);
			test.Character = Character;
			messageDispatcher.DispatchMessage(new Telegram(test, null, true));
			PooledBulletGameObjects.Initialize(Character);

			animator = GetComponent<Animator>();

			var aSources = GetComponents<AudioSource>();
			for (int i = 0; i < aSources.Length; i++)
			{
				audioSources.Add(aSources[i]);
			}

			Character.PostConstruction();
			Character.SendOutStats();

			particleSystem = GameObject.Find("PlaneShiftParticle").GetComponent<ParticleSystem>();

			Character.CharacterType = CharacterTypes.PLAYER;

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
				//Application.LoadLevel("GameOver");
				AutoFade.LoadLevel(4, 2, 1, Color.black);
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
			var statusEffect = Character.StatusEffect;
			if (statusEffect == StatusEffect.NONE)
				movementMultiplier = 1.0f;
			else if ((statusEffect & StatusEffect.BOOST_TIME) ==
				StatusEffect.BOOST_TIME)
			{
				movementMultiplier = 1.5f;
			}

			if (Character.StatusEffect != StatusEffect.NONE)
			{
				Character.RemoveStatusEffect();
			}
		}

        private bool shot;
		private void rangeAttack()
		{
			if (Character.EquippedRangeWeapon() && InputManager.Fire())
			{
				shot = Character.RangeWeapon.FireRate(Time.deltaTime);
				animator.SetBool("Shot", shot);
				if (shot)
				{
					IProjectile bullet = Character.RangeWeapon.Fire();
					bullet.ShotDirection = -GunModel.transform.right;
					bullet.CharacterType = CharacterTypes.ENEMY;
					var bulletInstance = PooledBulletGameObjects.GetPooledBullet(Character).GetComponent<Bullet>();
					bulletInstance.Projectile = bullet;
					messageDispatcher.DispatchMessage(new Telegram(bulletInstance, GunModel.transform));
				}
			}
            
		}

		private void weakAttack()
		{
			if (InputManager.WeakAttack() && Character.EquippedRangeWeapon())
			{
				Character.MeleeWeapon.Attack();
			}
		}

		private void activateAbility()
		{

			if (Character.EquippedAbility(InputManager.ActivateAbility()) &&
				Character.Ability.CoolDown())
			{
				Character.Ability.Activate(Character);
			}
		}

		private void switchWeapon()
		{
			if (InputManager.SwitchWeapon() &&
				Character.EquippedRangeWeapon())
			{
				Character.SwitchWeapon();
			}
		}

		public override void TakeDamage(int healthLost)
		{
			Character.TakeDamage(healthLost);
		}

		public void Heal(int healthGained)
		{
			Character.Heal(healthGained);
		}

		public void AddScore(int scoreValue)
		{
			Character.AddScore(scoreValue);
		}

		void LateUpdate()
		{
			gun.Rotate();
		}
	}
}
