using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.GameObjects;
using Assets.Scripts.Utilities.Messaging.Interfaces;
using Assets.Scripts.Utilities;
using Assets.Scripts.Weapons;
using UnityEngine;

namespace Assets.Scripts.MediatorPattern
{
	public class PhysicsDirector : Director
	{
		private List<PhysicsMediator> physicsMediators = new List<PhysicsMediator>();
		private List<Ground> grounds = new List<Ground>();
		private List<WeaponPickUpGameObject> weaponPickUpGameObjects = 
			new List<WeaponPickUpGameObject>();
		private List<AbilityPickUp> abilityPickUps =
 			new List<AbilityPickUp>();

		private bool assignGravity;

		private Vector3 gravity = new Vector3(0f, -1.0f, 0f);

		void Update()
		{
			gravityAssignment();

			GroundCollision();

			WeaponPickUpCollision();
			AbilityPickUpCollision();
		}

		private void AbilityPickUpCollision()
		{
			for (int i = 0; i < abilityPickUps.Count; i++)
			{
				if (abilityPickUps[i].gameObject.CheckBounds(physicsMediators[0].gameObject))
				{
					abilityPickUps[i].PickUp(physicsMediators[0].gameObject);
					abilityPickUps.RemoveAt(i);
				}
			}
		}

		private void WeaponPickUpCollision()
		{
			for(int i = 0; i < weaponPickUpGameObjects.Count; i++)
			{
				if (weaponPickUpGameObjects[i].gameObject.CheckBounds(physicsMediators[0].gameObject))
				{
					weaponPickUpGameObjects[i].PickUp(physicsMediators[0].gameObject);
					weaponPickUpGameObjects.RemoveAt(i);
				}
			}
		}

		private void GroundCollision()
		{
			if (grounds != null)
			{
				foreach (var physicsMed in physicsMediators)
				{
					foreach (var ground in grounds)
					{
						if (ground.gameObject.CheckBounds(physicsMed.gameObject))
						{
							physicsMed.Gravity = Vector3.zero;
							// if gameobject is colliding with a ground stop checking for other grounds.
							break;
						}

						else if (!ground.gameObject.CheckBounds(physicsMed.gameObject))
						{
							physicsMed.Gravity = gravity;
						}
					}
				}
			}
		}

		private void gravityAssignment()
		{
			if (assignGravity)
			{
				foreach (PhysicsMediator physicsMed in physicsMediators)
				{
					physicsMed.Gravity = gravity;
				}

				assignGravity = false;
			}
		}

		public override void Receive(ITelegram telegram)
		{
			if (telegram.Receiver == this)
			{
				if(telegram.Message is PhysicsMediator)
				{
					physicsMediators.Add((PhysicsMediator)telegram.Message);
					assignGravity = true;
				}
				else if(telegram.Message is Ground)
					grounds.Add((Ground)telegram.Message);
				else if(telegram.Message is WeaponPickUpGameObject)
					weaponPickUpGameObjects.Add((WeaponPickUpGameObject)telegram.Message);
				else if(telegram.Message is AbilityPickUp)
					abilityPickUps.Add((AbilityPickUp)telegram.Message);
			}
		}
	}
}
