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
		private List<UnitPhysicsMediator> unitPhysicsMediatorsTempList = new List<UnitPhysicsMediator>();
		private List<Ground> grounds = new List<Ground>();

		private UnitPhysicsMediator[] unitPhysicsMediators;
		private static Ground[] groundsArray;
		private int groundCount;

		private List<ItemPickUp> itemPickUps = new List<ItemPickUp>(100); 

		private List<PhysicsMediator> bullets = new List<PhysicsMediator>(200);  

		private Vector3 gravity = new Vector3(0f, -30f, 0f);

		private static UnitPhysicsMediator player;

		private static AABBIntersection aabbIntersection = new AABBIntersection();

		private Trigger trigger;
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
				//Application.LoadLevel("GameOver");
				
			}

			GroundCollision();

			pickUpCollisions();

			bulletCollision();
			slugCollision();

			if(aabbIntersection.Intersect(player.BoundingBox, trigger.BoundingBox))
				trigger.Tripped();
		}

		private int damage;
		private void slugCollision()
		{
            for (int i = 0; i < unitPhysicsMediators.Length; i++)
			{
                if (unitPhysicsMediators[i] is Player)
					continue;
                if (aabbIntersection.Intersect(player.BoundingBox, unitPhysicsMediators[i].BoundingBox))
				{
                    ((Player)player).TakeDamage(((Slug)unitPhysicsMediators[i]).DealDamage());

					if (player.Die())
					{
						PhysicsMediator removeMe = unitPhysicsMediatorsTempList.Find(p => p == player);
						unitPhysicsMediatorsTempList.RemoveAt(i);
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
                for (int j = 0; j < unitPhysicsMediators.Length; j++)
                {
					if (((Bullet)bullets[i]).Projectile.CharacterType == unitPhysicsMediators[j].Character.CharacterType &&
						aabbIntersection.Intersect(bullets[i].BoundingBox, unitPhysicsMediators[j].BoundingBox))
					{
                        unitPhysicsMediators[j].TakeDamage(((Bullet)bullets[i]).DealDamage());

                        if (unitPhysicsMediators[j].Die())
						{
                            PhysicsMediator removeMe = unitPhysicsMediators[j];
                            unitPhysicsMediators[j] = null;
                            quicksort(unitPhysicsMediators, 0, unitPhysicsMediators.Length - 1);
                            Array.Resize(ref unitPhysicsMediators, unitPhysicsMediators.Length - 1);
							Destroy(removeMe.gameObject);
						}
					}
				}
			}
		}

		private void pickUpCollisions()
		{
			for (int i = 0; i < itemPickUps.Count; i++)
			{
				if (aabbIntersection.Intersect(itemPickUps[i].BoundingBox, player.BoundingBox))
				{
					itemPickUps[i].PickUp(player.gameObject);
					itemPickUps.RemoveAt(i);
				}
			}
		}

		// TODO move into Seperate class
		private static AABB3D playerBoundingBox;
		private static AABB3D groundBoundingBox;
		private Vector3 velocity = new Vector3(0.0f, 0.0f, 0.0f);

		private Sweep sweep = new Sweep();
		private AABB3D a;
		private AABB3D b;

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
                a = left.BoundingBox;
                b = right.BoundingBox;
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
			quicksort(unitPhysicsMediators, 0, unitPhysicsMediators.Length - 1);

			float hitTime = 0f;

			for (int k = 0; k < unitPhysicsMediators.Length; k++)
			{
				try
				{
					unitPhysicsMediators[k].UpdateVelocity(gravity);
					playerBoundingBox = unitPhysicsMediators[k].BoundingBox;
				}
				catch (MissingReferenceException e)
				{
					Debug.Log("Test");
					throw e;
				}

				for (int i = 0; i < groundsArray.Length; i++)
				{
					groundBoundingBox = groundsArray[i].BoundingBox;
					var groundMin = groundBoundingBox.Center - new Vector3(groundBoundingBox.HalfWidth, groundBoundingBox.HalfHeight);
					var unitMax = playerBoundingBox.Center + new Vector3(playerBoundingBox.HalfWidth, playerBoundingBox.HalfHeight) + playerBoundingBox.Velocity * Time.deltaTime;
					if (groundMin.x > unitMax.x)
					{
						break;
					}

					if (aabbIntersection.Intersect(playerBoundingBox, groundBoundingBox))
					{
						var penetrationVector = playerBoundingBox.Center - groundBoundingBox.Center;
						unitPhysicsMediators[k].UpdateVelocity(Vector3.up * penetrationVector.y - gravity);
					}
					else if (sweep.TestMovingAABB(playerBoundingBox,
						playerBoundingBox.Velocity * Time.deltaTime, 0f, 1f,
						groundBoundingBox, ref hitTime))
					{
						float actualHittime = 1.0f - hitTime;
						if (playerBoundingBox.NormalCollision[0].x > 0.0f)
						{
							velocity.x = playerBoundingBox.Velocity.x;
							velocity.y = 0.0f;
							velocity.z = 0.0f;
							unitPhysicsMediators[k].UpdateVelocity(-velocity * actualHittime);
						}
						else if (playerBoundingBox.NormalCollision[0].x < 0.0f)
						{
							velocity.x = playerBoundingBox.Velocity.x;
							velocity.y = 0.0f;
							velocity.z = 0.0f;
							unitPhysicsMediators[k].UpdateVelocity(-velocity * actualHittime);
						}
						// TODO find out if better way for avoiding getting caught in platforms
						if (playerBoundingBox.NormalCollision[1].y < 0.0f && playerBoundingBox.Velocity.y < 0.0f)
						{

							velocity.x = 0.0f;
							velocity.y = playerBoundingBox.Velocity.y;
							velocity.z = 0.0f;
							unitPhysicsMediators[k].UpdateVelocity(-velocity * actualHittime);
						}
					}
					else if (unitPhysicsMediators[k] is Player && unitPhysicsMediators[k].velocity.y < Vector3.zero.y &&
						(unitPhysicsMediators[k] as Player).TouchGroundFrameCount > 2)
					{
						(unitPhysicsMediators[k] as Player).TouchGroundFrameCount = 0;
					}
				}

				try
				{
					unitPhysicsMediators[k].UpdatePosition();
					unitPhysicsMediators[k].ResetVelocity();
				}
				catch (MissingReferenceException e)
				{
					throw e;
				}
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
				if(telegram.Message is UnitPhysicsMediator)
				{
					unitPhysicsMediatorsTempList.Add((UnitPhysicsMediator)telegram.Message);
					unitPhysicsMediators = unitPhysicsMediatorsTempList.ToArray();
					if (telegram.Message is Player)
						player = unitPhysicsMediatorsTempList.Find(p => p is Player);
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
				else if(telegram.Message is ItemPickUp)
					itemPickUps.Add((ItemPickUp)telegram.Message);
				else if (telegram.Message is Bullet)
				{
					bullets.Add((Bullet)telegram.Message);
				}
				else if (telegram.Message is Trigger)
				{
					trigger = (Trigger) telegram.Message;
				}
			}
		}

		public static bool ShiftCollision(Vector3 planeChange)
		{
			for (int i = 0; i < groundsArray.Length; i++)
			{
				groundBoundingBox = groundsArray[i].BoundingBox;
				var groundMin = groundBoundingBox.Center - new Vector3(groundBoundingBox.HalfWidth, groundBoundingBox.HalfHeight);
				var pBoundingBox = player.BoundingBox;
				var unitMax = pBoundingBox.Center + new Vector3(pBoundingBox.HalfWidth, pBoundingBox.HalfHeight) + pBoundingBox.Velocity * Time.deltaTime;
				if (groundMin.x > unitMax.x)
				{
					break;
				}

				var BoundingBoxCenter = pBoundingBox.Center + planeChange;
				AABB3D checkBoundingBox = new AABB3D(BoundingBoxCenter, pBoundingBox.HalfWidth * 2,
					pBoundingBox.HalfHeight * 2, pBoundingBox.HalfDepth * 2);
				if (aabbIntersection.Intersect(groundBoundingBox, checkBoundingBox))
				{
					return false;
				}
				//if (Math.Abs(groundBoundingBox.Center.z - (playerBoundingBox.Center.z + planeChange.z))
				//	> (groundBoundingBox.HalfDepth + playerBoundingBox.HalfDepth))
				//	return false;
			}
			return true;
		}
	}
}
