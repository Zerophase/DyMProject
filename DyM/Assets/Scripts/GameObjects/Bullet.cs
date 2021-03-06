﻿using Assets.Scripts.GameObjects;
using Assets.Scripts.MediatorPattern;
using Assets.Scripts.Projectiles;
using Assets.Scripts.Projectiles.Interfaces;
using Assets.Scripts.Projectiles.Projectiles;
using ModestTree.Zenject;
using UnityEngine;
using System.Collections;
using Assets.Scripts.Character;
using Assets.Scripts.Utilities;
using Assets.Scripts.Utilities.EqualityComparison.Float;
using Assets.Scripts.Utilities.Messaging.Interfaces;

namespace Assets.Scripts.GameObjects
{
	public partial class Bullet : PhysicsMediator
	{
		// TODO see if can remove these and just use IPooledGameObjects
		private IProjectile projectile;
		public IProjectile Projectile { set { projectile = value; }  get { return projectile; }}

		private Vector3 startPosition;
		private Quaternion rotation;

		private Vector3 playerXMove;
		[Inject]
		public IPooledGameObjects PooledBulletGameObjects;

		private CharacterTypes characterType;
		public CharacterTypes CharacterType { get { return characterType; } set { characterType = value; } }

		public void Initialize()
		{
			receiver.Owner = this;
			receiver.SubScribe();
		}

		void Update()
		{
			ResetVelocity();
			 
			if (!projectile.IsProjectileSetup)
			{
				audio.Play();
				transform.rotation = rotation;
				transform.position = startPosition;
				projectile.SetUpProjectile(transform.position);
				BoundingBox.Center = transform.position;
			}

			var projectilePosition = projectile.ProjectilePattern();

			//playerXMove.x = player.BoundingBox.Velocity.x;
			UpdateVelocity(projectilePosition);
			UpdatePosition();

			if (projectile.ShouldProjectileDeactivate(transform.position))
			{
				playerXMove = Vector3.zero;
				projectile.DeactivateProjectile();
				PooledBulletGameObjects.DeactivatePooledBullet(gameObject,
                    projectile.Character, projectile);
			}

			//base.Update();
		}

		public int DealDamage()
		{
			return projectile.DealDamage();
		}
	}

	public partial class Bullet : IOwner
	{

		[Inject]
		private IReceiver receiver;
		public IReceiver Receiver
		{
			set { receiver = value; }
		}

		public void Receive(ITelegram telegram)
		{
			if (telegram.Receiver == this)
			{
				var gunModel = (GameObject) telegram.Message;
				startPosition = gunModel.transform.position;
				rotation = gunModel.transform.rotation;
				projectile.ShotDirection = -gunModel.transform.right;
				//Debug.Log("GunModel Rotation: " + gunModel.transform.rotation);
				if (FloatComparer.Compare(gunModel.transform.rotation.z, 0f, 0.1f))
				{
					var rotationVector = rotation.eulerAngles;
					rotation = Quaternion.Euler(rotationVector.x, 180f, rotationVector.z);
				}
			}
		}
	}
}

