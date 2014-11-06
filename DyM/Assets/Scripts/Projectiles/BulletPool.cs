using System.Collections.Generic;
using Assets.Scripts.DependencyInjection;
using Assets.Scripts.Projectiles.Interfaces;
using Assets.Scripts.Weapons.Interfaces;
using ModestTree.Zenject;
using UnityEngine;

namespace Assets.Scripts.Projectiles
{
	public class BulletPool : IBulletPool
	{
		[Inject]
		public BulletPool()
		{
		}

		List<IPooledProjectile> projectiles = new List<IPooledProjectile>();

		[Inject]
		private PooledProjectileFactory pooledProjectileFactory;
		private IPooledProjectile projectile;

		// For Tests
		public IPooledProjectile Projectile { set { projectile = value; } }

		public List<IPooledProjectile> Projectiles
		{
			get { return  projectiles; }
		}

		public void Initialize(IRangeWeapon rangeWeapon, Vector3 startPosition, int count)
		{
			projectile = pooledProjectileFactory.Create(rangeWeapon.Projectile);
			for (int i = 0; i < count; i++)
			{
				addProjectile();
			}
		}

		public void ChangeBullet(IRangeWeapon rangeWeapon)
		{
			for (int i = 0; i < projectiles.Count; i++)
			{
				projectiles[i] = projectile = pooledProjectileFactory.Create(rangeWeapon.Projectile);
			}
		}

		private void addProjectile()
		{
			projectiles.Add(projectile);
		}

		public IPooledProjectile GetPooledProjectile()
		{
			IPooledProjectile currentProjectile = null;

			addNewProjectileToList(ref currentProjectile);

			iterateThroughCreatedProjectiles(ref currentProjectile);
			return currentProjectile;
		}

		private void iterateThroughCreatedProjectiles(ref IPooledProjectile currentProjectile)
		{
			for (int i = 0; i < projectiles.Count; i++)
			{
				if (!projectiles[i].Active)
				{
					projectiles[i].Active = true;
					currentProjectile = projectiles[i];
				}
			}
		}

		private void addNewProjectileToList(ref IPooledProjectile currentProjectile)
		{
			if (projectiles[projectiles.Count - 1].Active)
			{
				int lastElement = projectiles.Count - 1;
				addProjectile();
				projectiles[lastElement].Active = true;
				currentProjectile = projectiles[lastElement];
			}
		}

		public void DeactivatePooledProjectile(IPooledProjectile projectile)
		{
			projectiles.Find(p => p == projectile).Active = false;
		}
	}
}
