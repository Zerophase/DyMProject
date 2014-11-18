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
using UnityEditorInternal;
using UnityEngine;
using System.Collections;

//TODO add dependencyInjection

namespace Assets.Scripts.GameObjects
{
	public class Player : MovablePhysicsMediator
	{
		private IPlaneShift planeShift;
		[Inject]
		private PlaneShiftFactory factory;

		[Inject]
		public ICharacter character;

		[Inject]
		public IPooledGameObjects PooledBUlletGameObjects;
		public static GameObject GunModel;

        private Vector3 acceleration = new Vector3(10f,0f,0f);

		private Animator animator;

		private bool idle;

		private float weaponSwitchTimer = 0.0f;

		public int Health { get { return character.Health; } }

		private Gun gun;

		private List<AudioSource> audioSources = new List<AudioSource>();

		protected override void Start()
		{
			planeShift = factory.Create(transform.position);
			GunModel = GameObject.FindGameObjectWithTag("EquippedGun");
			gun = GameObject.FindWithTag("GunRotator").GetComponent<Gun>();

			PooledBUlletGameObjects.Initialize();

			animator = GetComponent<Animator>();

			var aSources = GetComponents<AudioSource>();
			for (int i = 0; i < aSources.Length; i++)
			{
				audioSources.Add(aSources[i]);
			}

			base.Start();
		}

		protected override void Update()
		{
			death();
			
			switchPlane();

			move();

			flip(speed);

			rangeAttack();

			weakAttack();

			activateAbility();

			switchWeapon();
				

			base.Update();
		}

		private void death()
		{
			if (transform.position.y < -40)
			{
				Application.LoadLevel("main_menu");
			}
		}

		private void switchPlane()
		{
			if (InputManager.PlaneShiftDown())
			{
				transform.Translate(planeShift.ShiftPlane(KeyCode.Joystick1Button4,
					transform.position));
			}
			else if (InputManager.PlaneShiftUp())
			{
				transform.Translate(planeShift.ShiftPlane(KeyCode.Joystick1Button5,
					transform.position));
			}

			transform.Translate(planeShift.Dodge(transform.position, InputManager.CheckDodgeKeys(), Time.deltaTime));
		}

		private bool jumped;
		private float previousYPosition;
		private void move()
		{
			speed = InputManager.MovementHorizontal();

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
				audioSources[1].Play();

			
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

			previousYPosition = transform.position.y;

			transform.Translate(cardinalMovement.CalculateTotalMovement(speed,
				acceleration, InputManager.Jumping(), 0f /*stand in for total distance jumped*/));
		}

		private void rangeAttack()
		{
			if (InputManager.Fire() &&
			    character.EquippedRangeWeapon() && character.RangeWeapon.FireRate(Time.deltaTime))
			{
				IProjectile bullet = character.RangeWeapon.Fire();
				PooledBUlletGameObjects.GetPooledBullet().GetComponent<Bullet>().Projectile = bullet;
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
			if (character.EquippedAbility() && character.Ability.CoolDown() && 
				InputManager.ActivateAbility())
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

		void LateUpdate()
		{
			gun.Rotate();
		}
	} 
}
