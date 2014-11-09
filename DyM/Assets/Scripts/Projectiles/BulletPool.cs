using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.DependencyInjection;
using Assets.Scripts.Projectiles.Interfaces;
using Assets.Scripts.Projectiles.Projectiles;
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

		List<IPooledProjectile> pooledProjectiles = new List<IPooledProjectile>();

		[Inject]
		private PooledProjectileFactory pooledProjectileFactory;
		private IPooledProjectile pooledProjectile;
		private IProjectile projectile;

		private IRangeWeapon currentRangeWeapon;

		// For Tests
		public IPooledProjectile Projectile { set { pooledProjectile = value; } }

		public List<IPooledProjectile> Projectiles
		{
			get { return  pooledProjectiles; }
		}

		public void Initialize(IRangeWeapon rangeWeapon, Vector3 startPosition, int count)
		{
			currentRangeWeapon = rangeWeapon;
			changeProjectileType(rangeWeapon.Projectile);
			for (int i = 0; i < count; i++)
			{
				addProjectile();
			}
		}

		public void ChangeBullet(IRangeWeapon rangeWeapon)
		{
			currentRangeWeapon = rangeWeapon;
			changeProjectileType(rangeWeapon.Projectile);
			for (int i = 0; i < pooledProjectiles.Count; i++)
			{
				if(!pooledProjectiles[i].Active)
					pooledProjectiles[i] = pooledProjectile = pooledProjectileFactory.Create(rangeWeapon.Projectile);
			}
		}

		private void changeProjectileType(IProjectile projectile)
		{
			this.projectile = projectile;
		}

		private void addProjectile()
		{
			pooledProjectiles.Add(pooledProjectileFactory.Create(projectile));
		}

		public IPooledProjectile GetPooledProjectile()
		{
			IPooledProjectile currentProjectile = null;

			for (int i = 0; i < pooledProjectiles.Count; i++)
			{
				if (!pooledProjectiles[i].Active &&
				    !pooledProjectiles[i].Projectile.Equals(currentRangeWeapon.Projectile))
				{
					changeProjectileType(currentRangeWeapon.Projectile);
					pooledProjectiles[i] = pooledProjectileFactory.Create(projectile);
				}
			}

			addNewProjectileToList(ref currentProjectile);
			
			iterateThroughCreatedProjectiles(ref currentProjectile);

			if (pooledProjectiles.Any(b => b.Projectile is LightningGunProjectile))
				Debug.Log("machineGun Projectile when should be lightning:" + pooledProjectiles.Find(b => b.Projectile is MachineGunProjectile));
			return currentProjectile;
		}

		private void iterateThroughCreatedProjectiles(ref IPooledProjectile currentProjectile)
		{
			for (int i = 0; i < pooledProjectiles.Count; i++)
			{
				if (!pooledProjectiles[i].Active)
				{
					pooledProjectiles[i].Active = true;
					currentProjectile = pooledProjectiles[i];
					break;
				}
			}
		}

		private void addNewProjectileToList(ref IPooledProjectile currentProjectile)
		{
			if (pooledProjectiles[pooledProjectiles.Count - 1].Active)
			{
				int lastElement = pooledProjectiles.Count - 1;
				addProjectile();
				pooledProjectiles[lastElement].Active = true;
				currentProjectile = pooledProjectiles[lastElement];
			}
		}

		public void DeactivatePooledProjectile(IProjectile projectile)
		{
			pooledProjectiles.Find(p => p.Projectile == projectile).Active = false;
		}
	}
}
