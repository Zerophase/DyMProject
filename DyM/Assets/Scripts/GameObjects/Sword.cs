using System;
using Assets.Scripts.Weapons.Interfaces;
using ModestTree.Zenject;
using UnityEngine;
using System.Collections;

namespace Assets.Scripts.GameObjects
{
	public class Sword : MonoBehaviour
	{
		[Inject]
		private IMeleeWeapon weapon;

		private bool swing = false;

		private Quaternion targetRotation;
		void Update () 
		{
			if (Input.GetButtonDown("WeakAttack"))
			{
				swing = true;
				targetRotation = Quaternion.Euler(0f, Mathf.Atan(0.5f), 0f);
			}

			if (swing)
			{
				transform.rotation = targetRotation;
				swing = false;
			}
		}

		void OnCollisionEnter(UnityEngine.Collision collision)
		{
			if(this.tag == "EquippedSword")
				return;

			if (collision.gameObject.tag == "Player")
			{
				//weapon.PickUp(collision.gameObject.GetComponent<Player>().character);
				Destroy(gameObject);
			}
		}
	} 
}
