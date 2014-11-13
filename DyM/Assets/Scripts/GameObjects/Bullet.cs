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

		[Inject]
		public IPooledGameObjects PooledBulletGameObjects;

		void Update()
		{
			if (!projectile.IsProjectileSetup)
			{
				transform.position = Player.GunModel.transform.position;
				projectile.SetUpProjectile(transform.position);
			}

			transform.Translate(projectile.ProjectilePattern());

			if (projectile.ShouldProjectileDeactivate(transform.position))
			{
				projectile.DeactivateProjectile();
				PooledBulletGameObjects.DeactivatePooledBullet(gameObject, projectile);
			}

			base.Update();
		}
	}
}

