﻿using System;
using System.Diagnostics;
using Assets.Scripts.Character.Interfaces;
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
	public class Player : PhysicsMediator
	{
		private IPlaneShift planeShift;
		[Inject]
		private PlaneShiftFactory factory;

		
		[Inject]
		public ICharacter character;

		[Inject]
		public IPooledGameObjects PooledBUlletGameObjects;
		public static GameObject GunModel;
		private GameObject bullet;

		private bool planeShiftUp = false;
		private bool planeShiftdown = false;
        private Vector3 acceleration = new Vector3(10f,0f,0f);

		
		
		void Start()
		{
			planeShift = factory.Create(transform.position);
			GunModel = GameObject.Find("Gun");
			PooledBUlletGameObjects.Initialize();
			
			base.Start();
            
		}

		void Update()
		{
			if (Input.GetButtonDown("PlaneShiftDown"))
			{
				transform.Translate(planeShift.ShiftPlane(KeyCode.Joystick1Button4,
					transform.position));
			}
			else if (Input.GetButtonDown("PlaneShiftUp"))
			{
				transform.Translate(planeShift.ShiftPlane(KeyCode.Joystick1Button5,
					transform.position));
			}

			transform.Translate(planeShift.Dodge(transform.position, dodgeKeysToCheck(), Time.deltaTime));
            transform.Translate((cardinalMovement.CalculateTotalMovement(Input.GetAxis("Horizontal"),acceleration,Input.GetButton("Jump"))));
            //transform.Translate(cardinalMovement.Move(Input.GetAxis("Horizontal"), acceleration, Time.deltaTime));
            //transform.Translate(cardinalMovement.Jump(Input.GetButton("Jump"), 0f));

			if (Input.GetAxis("Fire1") > 0 && character.EquippedRangeWeapon())
			{
				PooledBUlletGameObjects.GetPooledBullet().GetComponent<Bullet>().Projectile = 
					character.RangeWeapon.Fire();
			}

			if (Input.GetButtonDown("WeakAttack") && character.EquippedRangeWeapon())
			{
				character.MeleeWeapon.Attack();
			}

			if (Input.GetButtonDown("ActivateAbility") && character.EquippedAbility())
			{
				character.Ability.Activate(character);
//				Debug.Log("StatusEffect is: " + character.StatusEffect);
			}
				
		}

		private bool dodgeKeysToCheck()
		{
			if (Input.GetButton("PlaneShiftDown") || Input.GetButton("PlaneShiftUp"))
				return true;
			else
			{
				return false;
			}
		}
	} 
}
