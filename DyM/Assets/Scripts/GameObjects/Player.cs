using System;
using Assets.Scripts.Character.Interfaces;
using Assets.Scripts.ObjectManipulation;
using Assets.Scripts.ObjectManipulation.Interfaces;
using Assets.Scripts.Projectiles;
using Assets.Scripts.Projectiles.Interfaces;
using ModestTree.Zenject;
using UnityEngine;
using System.Collections;

//TODO add dependencyInjection

namespace Assets.Scripts.GameObjects
{
	public class Player : MonoBehaviour
	{
		[Inject]
		public IMovement movement;
		[Inject]
		public ICharacter character;

		[Inject]
		public IPooledGameObjects PooledBUlletGameObjects;
		public static GameObject GunModel;
		private GameObject bullet;
		void Start()
		{
			GunModel = GameObject.Find("Gun");
			PooledBUlletGameObjects.Initialize();
		}
		// Update is called once per frame
		void FixedUpdate ()
		{
			//Debug.Log("Left thumbstick position: " + Input.GetAxisRaw("Horizontal"));
			if(Input.GetButtonDown("PlaneShiftDown"))
				transform.Translate(movement.ShiftPlane(KeyCode.Joystick1Button4, 
					transform.position));
			else if(Input.GetButtonDown("PlaneShiftUp"))
				transform.Translate(movement.ShiftPlane(KeyCode.Joystick1Button5,
					transform.position));

			transform.Translate(movement.Dodge(transform.position, dodgeKeysToCheck(), Time.deltaTime));

			transform.Translate(movement.Jump(Input.GetAxis("Vertical"), transform.position.y));
			transform.Translate(movement.Move(Input.GetAxis("Horizontal"), Time.deltaTime));
		
		}

		void Update()
		{
			if (Input.GetAxis("Fire1") > 0f && character.EquippedWeapon())
			{
				PooledBUlletGameObjects.GetPooledBullet().GetComponent<Bullet>().Projectile = 
					character.Weapon.Fire();
			}

			if (Input.GetButtonDown("ActivateAbility") && character.EquippedAbility())
			{
				character.Ability.Activate(character);
				Debug.Log("StatusEffect is: " + character.StatusEffect);
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
