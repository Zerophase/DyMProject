using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Collision;
using Assets.Scripts.Collision.IntersectionTests;
using Assets.Scripts.Collision.SweepTests;
using Assets.Scripts.CollisionBoxes.ThreeD;
using Assets.Scripts.GameObjects;
using Assets.Scripts.Utilities.Messaging.Interfaces;
using Assets.Scripts.Utilities;
using Assets.Scripts.Weapons;
using UnityEngine;

namespace Assets.Scripts.MediatorPattern
{
	public class PhysicsDirector : Director
	{
		private List<MovablePhysicsMediator> movablePhysicsMediators = new List<MovablePhysicsMediator>();
		private List<Ground> grounds = new List<Ground>();
		private List<WeaponPickUpGameObject> weaponPickUpGameObjects = 
			new List<WeaponPickUpGameObject>();
		private List<AbilityPickUpGameObject> abilityPickUps =
 			new List<AbilityPickUpGameObject>();
		private List<PhysicsMediator> bullets = new List<PhysicsMediator>();  

		private bool assignGravity;

		private Vector3 gravity = new Vector3(0f, -10f, 0f);

		private PhysicsMediator player;

		private AABBIntersection aabbIntersection = new AABBIntersection();

		void Update()
		{
			if(Input.GetKey(KeyCode.Escape))
				Application.Quit();
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
			for (int i = 0; i < movablePhysicsMediators.Count; i++)
			{
				if(movablePhysicsMediators[i] is Player)
					continue;
				if (aabbIntersection.Intersect(player.BoundingBox, movablePhysicsMediators[i].BoundingBox))
				{
					((Player)player).TakeDamage(((Slug) movablePhysicsMediators[i]).DealDamage());

					if (((Player)player).Health <= 0)
					{
						PhysicsMediator removeMe = movablePhysicsMediators.Find(p => p == player);
						movablePhysicsMediators.RemoveAt(i);
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
				for (int j = 0; j < movablePhysicsMediators.Count; j++)
				{
					if (movablePhysicsMediators[j] is Player)
						continue;
					if (aabbIntersection.Intersect(bullets[i].BoundingBox, movablePhysicsMediators[j].BoundingBox))
					{
						((Slug)movablePhysicsMediators[j]).TakeDamge(((Bullet)bullets[i]).DealDamage());

						if (((Slug) movablePhysicsMediators[j]).Health <= 0)
						{
							PhysicsMediator removeMe = movablePhysicsMediators[j];
							movablePhysicsMediators.RemoveAt(j);
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
				if (aabbIntersection.Intersect(abilityPickUps[i].BoundingBox, player.BoundingBox))
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
				if (aabbIntersection.Intersect(weaponPickUpGameObjects[i].BoundingBox, player.BoundingBox))
				{
					weaponPickUpGameObjects[i].PickUp(player.gameObject);
					weaponPickUpGameObjects.RemoveAt(i);
				}
			}
		}

		// TODO move into Seperate class
		private AABB3D playerBoundingBox;
		private AABB3D groundBoundingBox;
		private Vector3 velocity = new Vector3(0.0f, 0.0f, 0.0f);

		private Sweep sweep = new Sweep();

		private void GroundCollision()
		{
			if (grounds != null)
			{
				float hitTime = 0f;
				//sweep.ResetRectangles();
				for (int k = 0; k < movablePhysicsMediators.Count; k++)
				{
					movablePhysicsMediators[k].UpdateVelocity(gravity);
					for (int i = 0; i < grounds.Count; i++)
					{
						playerBoundingBox = movablePhysicsMediators[k].BoundingBox;
						groundBoundingBox = grounds[i].BoundingBox;
						if (sweep.TestMovingAABB(playerBoundingBox,
							playerBoundingBox.Velocity * Time.deltaTime, 0f, 1f,
							groundBoundingBox, ref hitTime))
						{
							float actualHittime = 1.0f - hitTime;
							if (playerBoundingBox.NormalCollision[0].x > 0.0f)
							{
								velocity.x = playerBoundingBox.Velocity.x;
								velocity.y = 0.0f;
								velocity.z = 0.0f;
								movablePhysicsMediators[k].UpdateVelocity(-velocity * actualHittime);
							}
							else if (playerBoundingBox.NormalCollision[0].x < 0.0f)
							{
								velocity.x = playerBoundingBox.Velocity.x;
								velocity.y = 0.0f;
								velocity.z = 0.0f;
								movablePhysicsMediators[k].UpdateVelocity(-velocity * actualHittime);
							}
																				 // TODO find out if better way for avoiding getting caught in platforms
							if (playerBoundingBox.NormalCollision[1].y < 0.0f && playerBoundingBox.Velocity.y < 0.0f)
							{
								velocity.x = 0.0f;
								velocity.y = playerBoundingBox.Velocity.y;
								velocity.z = 0.0f;
								movablePhysicsMediators[k].UpdateVelocity(-velocity * actualHittime);
							}

						}
					}

					movablePhysicsMediators[k].UpdatePosition();
					movablePhysicsMediators[k].ResetVelocity();
				}
			}
		}

		private void gravityAssignment()
		{
			if (assignGravity)
			{
				foreach (MovablePhysicsMediator physicsMed in movablePhysicsMediators)
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
					movablePhysicsMediators.Add((MovablePhysicsMediator)telegram.Message);
					if (telegram.Message is Player)
						player = movablePhysicsMediators.Find(p => p is Player);
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
