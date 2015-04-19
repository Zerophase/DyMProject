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
using Assets.Scripts.Weapons;
using UnityEngine;

namespace Assets.Scripts.MediatorPattern
{
	public class PhysicsDirector : Director
	{
		private List<MovablePhysicsMediator> movablePhysicsMediators = new List<MovablePhysicsMediator>();
		private List<Ground> grounds = new List<Ground>();

		private MovablePhysicsMediator[] movablePhysicsMediatorsArray;
		private Ground[] groundsArray;
		private int groundCount;
		private List<WeaponPickUpGameObject> weaponPickUpGameObjects = 
			new List<WeaponPickUpGameObject>();
		private List<AbilityPickUpGameObject> abilityPickUps =
 			new List<AbilityPickUpGameObject>();
		private List<HealthPack> healthPacks =
 			new List<HealthPack>();
		private List<PhysicsMediator> bullets = new List<PhysicsMediator>();  

		private bool assignGravity;

		private Vector3 gravity = new Vector3(0f, -30f, 0f);

		private PhysicsMediator player;

		private AABBIntersection aabbIntersection = new AABBIntersection();

		public void Initialize()
		{
			groundCount = FindObjectsOfType<Ground>().Length;
		}

		void Update()
		{
			if(Input.GetKey(KeyCode.Escape))
				Application.Quit();
			if(player == null)
			{
				Application.LoadLevel("GameOver");
			}

			GroundCollision();

			WeaponPickUpCollision();
			AbilityPickUpCollision();
			HealthPackCollision();

			bulletCollision();
			slugCollision();
		}

		private int damage;
		private void slugCollision()
		{
            for (int i = 0; i < movablePhysicsMediatorsArray.Length; i++)
			{
                if (movablePhysicsMediatorsArray[i] is Player)
					continue;
                if (aabbIntersection.Intersect(player.BoundingBox, movablePhysicsMediatorsArray[i].BoundingBox))
				{
                    ((Player)player).TakeDamage(((Slug)movablePhysicsMediatorsArray[i]).DealDamage());

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
                for (int j = 0; j < movablePhysicsMediatorsArray.Length; j++)
				{
					if (movablePhysicsMediators[j] is Player)
						continue;
                    if (aabbIntersection.Intersect(bullets[i].BoundingBox, movablePhysicsMediatorsArray[j].BoundingBox))
					{
                        ((Slug)movablePhysicsMediatorsArray[j]).TakeDamge(((Bullet)bullets[i]).DealDamage());

                        if (((Slug)movablePhysicsMediatorsArray[j]).Health <= 0)
						{
                            PhysicsMediator removeMe = movablePhysicsMediatorsArray[j];
                            movablePhysicsMediatorsArray[j] = null;
                            quicksort(movablePhysicsMediatorsArray, 0, movablePhysicsMediatorsArray.Length - 1);
                            Array.Resize<MovablePhysicsMediator>(ref movablePhysicsMediatorsArray, movablePhysicsMediatorsArray.Length - 1);
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

		private void HealthPackCollision()
		{
			for (int i = 0; i < healthPacks.Count; i++)
			{
				if (aabbIntersection.Intersect(healthPacks[i].BoundingBox, player.BoundingBox))
				{
					healthPacks[i].PickUp(player.gameObject);
					healthPacks.RemoveAt(i);
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

		private int compareAABBS(PhysicsMediator left, PhysicsMediator right)
		{
            float leftMin = 0f;
            float rightMin = 0f;
            if(left == null)
            {
                return 1;
            }
            else if( right == null)
            {
                return -1;
            }
            else
            {
                AABB3D a = left.BoundingBox;
                AABB3D b = right.BoundingBox;
                leftMin = a.Center.x - a.HalfWidth;
                rightMin = b.Center.x - b.HalfWidth;
            }
			
			if (leftMin < rightMin)
				return -1;
			if (leftMin > rightMin)
				return 1;
			return 0;
		}

		private void quicksort(PhysicsMediator[] colliders, int first, int last)
		{
			int left = first;
			int right = last;
			int pivot = first;
			first++;

			while (last >= first)
			{
                if (compareAABBS(colliders[first], colliders[pivot]) >= 0 &&
                    compareAABBS(colliders[last], colliders[pivot]) < 0)
                    swap(colliders, first, last);
                else if (compareAABBS(colliders[first], colliders[pivot]) >= 0)
                    last--;
                else if (compareAABBS(colliders[last], colliders[pivot]) < 0)
                    first++;
                else
                {
                    last--;
                    first++;
                }
			}

			swap(colliders, pivot, last);
			pivot = last;
			if(pivot > left)
				quicksort(colliders, left, pivot);
			if(right > pivot + 1)
				quicksort(colliders, pivot + 1, right);
		}

		private void swap(PhysicsMediator[] colliders, int left, int right)
		{
			var temp = colliders[right];
			colliders[right] = colliders[left];
			colliders[left] = temp;
		}

		private bool adjustPosition = true;
		private void GroundCollision()
		{
			quicksort(movablePhysicsMediatorsArray, 0, movablePhysicsMediatorsArray.Length - 1);
			
			float hitTime = 0f;

			for (int k = 0; k < movablePhysicsMediatorsArray.Length; k++)
			{
				movablePhysicsMediatorsArray[k].UpdateVelocity(gravity);

				playerBoundingBox = movablePhysicsMediatorsArray[k].BoundingBox;

				for (int i = 0; i < groundsArray.Length; i++)
				{
					groundBoundingBox = groundsArray[i].BoundingBox;
					var groundMin = groundBoundingBox.Center - new Vector3(groundBoundingBox.HalfWidth, groundBoundingBox.HalfHeight);
					var unitMax = playerBoundingBox.Center + new Vector3(playerBoundingBox.HalfWidth, playerBoundingBox.HalfHeight) + playerBoundingBox.Velocity * Time.deltaTime;
					if (groundMin.x > unitMax.x)
					{
						break;
					}

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
							movablePhysicsMediatorsArray[k].UpdateVelocity(-velocity * actualHittime);
						}
						else if (playerBoundingBox.NormalCollision[0].x < 0.0f)
						{
							velocity.x = playerBoundingBox.Velocity.x;
							velocity.y = 0.0f;
							velocity.z = 0.0f;
							movablePhysicsMediatorsArray[k].UpdateVelocity(-velocity * actualHittime);
						}
						// TODO find out if better way for avoiding getting caught in platforms
						if (playerBoundingBox.NormalCollision[1].y < 0.0f && playerBoundingBox.Velocity.y < 0.0f)
						{
							
							velocity.x = 0.0f;
							velocity.y = playerBoundingBox.Velocity.y;
							velocity.z = 0.0f;
							movablePhysicsMediatorsArray[k].UpdateVelocity(-velocity * actualHittime);
						}
					}
					else if (movablePhysicsMediatorsArray[k] is Player && movablePhysicsMediatorsArray[k].velocity.y < Vector3.zero.y &&
						(movablePhysicsMediatorsArray[k] as Player).TouchGroundFrameCount > 2)
					{
						(movablePhysicsMediatorsArray[k] as Player).TouchGroundFrameCount = 0;
					}

					//Debug.Log("Player on ground for frames: " + (movablePhysicsMediators[k] as Player).TouchGroundFrameCount);
				}

				movablePhysicsMediatorsArray[k].UpdatePosition();
				movablePhysicsMediatorsArray[k].ResetVelocity();
			}
		}

		private int findPosition(PhysicsMediator[] ground, AABB3D movable)
		{
			for (int i = 0; i < ground.Length; i++)
			{
				if (ground[i].BoundingBox.Center.x  > movable.Center.x)
				{
					Debug.Log(i);
					return i;
				}
			}
			Debug.Log(0);
			return 0;
		}

		public override void Receive(ITelegram telegram)
		{
			/// TODO replace with Switch Statement
			if (telegram.Receiver == this)
			{
				if(telegram.Message is MovablePhysicsMediator)
				{
					movablePhysicsMediators.Add((MovablePhysicsMediator)telegram.Message);
					movablePhysicsMediatorsArray = movablePhysicsMediators.ToArray();
					if (telegram.Message is Player)
						player = movablePhysicsMediators.Find(p => p is Player);
					assignGravity = true;
				}
				else if (telegram.Message is Ground)
				{
					grounds.Add((Ground)telegram.Message);

					if (grounds.Count == groundCount)
					{
						groundsArray = grounds.ToArray();
						quicksort(groundsArray, 0, groundsArray.Length - 1);

						for (int i = 0; i < groundsArray.Length; i++)
						{
							var b0 = groundsArray[i].BoundingBox;
							for (int j = 1; j < groundsArray.Length; j++)
							{
								var b1 = groundsArray[j].BoundingBox;
								if (b0.HalfHeight > b0.HalfWidth && b1.HalfHeight > b1.HalfWidth &&
									Math.Abs(Vector3.Cross(groundsArray[i].BoundingBox.Center.normalized, groundsArray[j].BoundingBox.Center.normalized).y) <= 20.0f)
								{
									groundsArray[i].BoundingBox.IgnoreCollision = IgnoreCollision.Y_AXIS;
									groundsArray[j].BoundingBox.IgnoreCollision = IgnoreCollision.Y_AXIS;
								}

								if (b0.HalfWidth > b0.HalfHeight && b1.HalfWidth > b1.HalfHeight &&
									Math.Abs(Vector3.Cross(groundsArray[i].BoundingBox.Center.normalized, groundsArray[j].BoundingBox.Center.normalized).x) <= 130f)
								{
									groundsArray[i].BoundingBox.IgnoreCollision = IgnoreCollision.X_AXIS;
									groundsArray[j].BoundingBox.IgnoreCollision = IgnoreCollision.X_AXIS;
								}
							}

						}
					}
				}
				else if(telegram.Message is WeaponPickUpGameObject)
					weaponPickUpGameObjects.Add((WeaponPickUpGameObject)telegram.Message);
				else if(telegram.Message is AbilityPickUpGameObject)
					abilityPickUps.Add((AbilityPickUpGameObject)telegram.Message);
				else if(telegram.Message is HealthPack)
					healthPacks.Add((HealthPack)telegram.Message);
				else if(telegram.Message is Bullet)
					bullets.Add((Bullet)telegram.Message);

			}
		}
	}
}
