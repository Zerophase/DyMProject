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
		private List<AbilityPickUpGameObject> abilityPickUps =
 			new List<AbilityPickUpGameObject>();
		private List<PhysicsMediator> bullets = new List<PhysicsMediator>();  

		private bool assignGravity;

		private Vector3 gravity = new Vector3(0f, -1f, 0f);

		private PhysicsMediator player;

		void Update()
		{
			if(player == null)
			{
				Application.LoadLevel("GameOver");
			}
			gravityAssignment();

			GroundCollision();

			WeaponPickUpCollision();
			AbilityPickUpCollision();

			bulletCollision();
			slugCollision();
		}

		private int damage;
		private void slugCollision()
		{
			for (int i = 0; i < physicsMediators.Count; i++)
			{
				if(physicsMediators[i] is Player)
					continue;
				if (physicsMediators[i].CheckBounds(player))
				{
					((Player)player).TakeDamage(((Slug) physicsMediators[i]).DealDamage());

					if (((Player)player).Health <= 0)
					{
						PhysicsMediator removeMe = physicsMediators.Find(p => p == player);
						physicsMediators.RemoveAt(i);
						Destroy(removeMe.gameObject);
					}
				}
			}	
		}

		private void bulletCollision()
		{
			if(!bullets.Any(aBullet => aBullet.enabled))
				return;
			for (int i = 0; i < bullets.Count; i++)
			{
				for (int j = 0; j < physicsMediators.Count; j++)
				{
					if (physicsMediators[j] is Player)
						continue;
					if (bullets[i].CheckBounds(physicsMediators[j]))
					{
						((Slug)physicsMediators[j]).TakeDamge(((Bullet)bullets[i]).DealDamage());

						if (((Slug) physicsMediators[j]).Health <= 0)
						{
							PhysicsMediator removeMe = physicsMediators[j];
							physicsMediators.RemoveAt(j);
							Destroy(removeMe.gameObject);
						}
					}
				}
			}
		}

		private void AbilityPickUpCollision()
		{
			for (int i = 0; i < abilityPickUps.Count; i++)
			{
				if (abilityPickUps[i].CheckBounds(player))
				{
					abilityPickUps[i].PickUp(player.gameObject);
					abilityPickUps.RemoveAt(i);
				}
			}
		}

		private void WeaponPickUpCollision()
		{
			for(int i = 0; i < weaponPickUpGameObjects.Count; i++)
			{
				if (weaponPickUpGameObjects[i].CheckBounds(player))
				{
					weaponPickUpGameObjects[i].PickUp(player.gameObject);
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
						if (physicsMed.CheckBounds(ground))
						{
							//if (Util.compareEachFloat(physicsMed.GetBox3D.yVelocity, 0f))
							{
								physicsMed.Gravity = Vector3.zero;
								physicsMed.HasJumped = false;
							}
							// if gameobject is colliding with a ground stop checking for other grounds.
							break;
						}
						else if (!physicsMed.CheckBounds(ground))
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
					if (telegram.Message is Player)
						player = physicsMediators.Find(p => p is Player);
					assignGravity = true;
				}
				else if(telegram.Message is Ground)
					grounds.Add((Ground)telegram.Message);
				else if(telegram.Message is WeaponPickUpGameObject)
					weaponPickUpGameObjects.Add((WeaponPickUpGameObject)telegram.Message);
				else if(telegram.Message is AbilityPickUpGameObject)
					abilityPickUps.Add((AbilityPickUpGameObject)telegram.Message);
				else if(telegram.Message is Bullet)
					bullets.Add((Bullet)telegram.Message);

			}
		}
	}
}
