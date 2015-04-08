using Assets.Scripts.GameObjects;
using Assets.Scripts.MediatorPattern;
using Assets.Scripts.Projectiles;
using Assets.Scripts.Projectiles.Interfaces;
using Assets.Scripts.Projectiles.Projectiles;
using ModestTree.Zenject;
using UnityEngine;
using System.Collections;
using Assets.Scripts.Utilities;

namespace Assets.Scripts.GameObjects
{
	public class Bullet : PhysicsMediator
	{
		// TODO see if can remove these and just use IPooledGameObjects
		private IProjectile projectile;
		public IProjectile Projectile { set { projectile = value; } }

		// TODO remove and replace with messaging.
		private static Player player;

		private Vector3 playerXMove;
		[Inject]
		public IPooledGameObjects PooledBulletGameObjects;

		protected override void Start ()
		{
			base.Start ();

			if(player == null)
				player = FindObjectOfType<Player>();
		}

		void Update()
		{
			ResetVelocity();
			 
			if (!projectile.IsProjectileSetup)
			{
				audio.Play();
				transform.rotation = Quaternion.Inverse(
					Quaternion.Inverse(Player.GunModel.transform.rotation)
					);
				transform.position = Player.GunModel.transform.position;
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
				PooledBulletGameObjects.DeactivatePooledBullet(gameObject, projectile);
			}

			//base.Update();
		}

		public int DealDamage()
		{
			return projectile.DealDamage();
		}
	}
}

