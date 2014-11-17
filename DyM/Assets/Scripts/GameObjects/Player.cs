using System;
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

		protected override void Start()
		{
			planeShift = factory.Create(transform.position);
			GunModel = GameObject.FindGameObjectWithTag("EquippedGun");
			gun = GameObject.FindWithTag("GunRotator").GetComponent<Gun>();

			PooledBUlletGameObjects.Initialize();

			animator = GetComponent<Animator>();

			base.Start();
		}

		protected override void Update()
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

			speed = InputManager.MovementHorizontal();

			if(Util.compareEachFloat(speed, 0.0f))
			{
				idle = true;

			}
			else
				idle = false;

			animator.SetBool("Idle", idle);
			animator.SetFloat("Speed", speed);

            transform.Translate(cardinalMovement.CalculateTotalMovement(speed,
				acceleration,InputManager.Jump(), 0f/*stand in for total distance jumped*/));
            //transform.Translate(cardinalMovement.Move(Input.GetAxis("Horizontal"), acceleration, Time.deltaTime));
            //transform.Translate(cardinalMovement.Jump(Input.GetButton("Jump"), 0f));

			flip(speed);

			if (InputManager.Fire() && 
				character.EquippedRangeWeapon() && character.RangeWeapon.FireRate(Time.deltaTime))
			{
				IProjectile bullet = character.RangeWeapon.Fire();
				PooledBUlletGameObjects.GetPooledBullet().GetComponent<Bullet>().Projectile = bullet;
			}

			if (InputManager.WeakAttack() && character.EquippedRangeWeapon())
			{
				character.MeleeWeapon.Attack();
			}

			if (InputManager.ActivateAbility() && character.EquippedAbility())
			{
				character.Ability.Activate(character);
//				Debug.Log("StatusEffect is: " + character.StatusEffect);
			}

			if (InputManager.SwitchWeapon() &&
			    character.EquippedRangeWeapon())
			{
				character.SwitchWeapon();
			}
				

			base.Update();
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
