using System;
using Assets.Scripts.Character.Interfaces;
using Assets.Scripts.ObjectManipulation;
using Assets.Scripts.ObjectManipulation.Interfaces;
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

		public static GameObject GunModel;
		private GameObject bullet;
		void Start()
		{
			GunModel = GameObject.Find("Gun");
			bullet = Resources.Load<GameObject>("Prefabs/Bullet/Bullet");
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
			if (Input.GetAxis("Fire1") > 0f && character.Equipped())
			{
				GameObject clone = Instantiate(bullet, GunModel.transform.position, Quaternion.identity) as GameObject;
				clone.GetComponent<Bullet>().Projectile = character.Weapon.Fire();
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
