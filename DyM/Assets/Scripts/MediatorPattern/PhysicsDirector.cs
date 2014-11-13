using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Collision;
using Assets.Scripts.GameObjects;
using Assets.Scripts.Utilities.Messaging.Interfaces;
using Assets.Scripts.Utilities;
using Assets.Scripts.Weapons;
using UnityEngine;

namespace Assets.Scripts.MediatorPattern
{
	public class PhysicsDirector : Director
	{
		private List<MovablePhysicsMediator> physicsMediators = new List<MovablePhysicsMediator>();
		private List<Ground> grounds = new List<Ground>();
		private List<WeaponPickUpGameObject> weaponPickUpGameObjects = 
			new List<WeaponPickUpGameObject>();
		private List<AbilityPickUp> abilityPickUps =
 			new List<AbilityPickUp>();

		private bool assignGravity;

		private Vector3 gravity = new Vector3(0f, -1f, 0f);

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
				if (abilityPickUps[i].CheckBounds(physicsMediators[0]))
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
				if (weaponPickUpGameObjects[i].CheckBounds(physicsMediators[0]))
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
						if (GJK.Instance.TestCollision(physicsMed.GetBox3D, ground.GetBox3D))
						{
							//if (Util.compareEachFloat(physicsMed.GetBox3D.yVelocity, 0f))
							{
								physicsMed.Gravity = Vector3.zero;
								physicsMed.HasJumped = false;
							}
							// if gameobject is colliding with a ground stop checking for other grounds.
							break;
						}
						else if (!GJK.Instance.TestCollision(physicsMed.GetBox3D, ground.GetBox3D))
						{
							physicsMed.Gravity = gravity;
							physicsMed.HasJumped = true;
						}
					}
				}
			}
		}

		private void gravityAssignment()
		{
			if (assignGravity)
			{
				foreach (MovablePhysicsMediator physicsMed in physicsMediators)
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
				if(telegram.Message is MovablePhysicsMediator)
				{
					physicsMediators.Add((MovablePhysicsMediator)telegram.Message);
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
