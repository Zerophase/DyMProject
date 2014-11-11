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

		List<IPooledProjectile> projectiles = new List<IPooledProjectile>();

		[Inject]
		private PooledProjectileFactory pooledProjectileFactory;
		private IPooledProjectile pooledProjectile;
		private IProjectile projectile;

		// For Tests
		public IPooledProjectile Projectile { set { pooledProjectile = value; } }

		public List<IPooledProjectile> Projectiles
		{
			get { return  projectiles; }
		}

		public void Initialize(IRangeWeapon rangeWeapon, Vector3 startPosition, int count)
		{
			changeProjectileType(rangeWeapon.Projectile);
			for (int i = 0; i < count; i++)
			{
				addProjectile();
			}
		}

		public void ChangeBullet(IRangeWeapon rangeWeapon)
		{
			changeProjectileType(rangeWeapon.Projectile);
			for (int i = 0; i < projectiles.Count; i++)
			{
				if(!projectiles[i].Active)
					projectiles[i] = pooledProjectile = pooledProjectileFactory.Create(rangeWeapon.Projectile);
			}
		}

		private void changeProjectileType(IProjectile projectile)
		{
			this.projectile = projectile;
		}

		private void addProjectile()
		{
			projectiles.Add(pooledProjectileFactory.Create(projectile));
		}

		public IPooledProjectile GetPooledProjectile()
		{
			IPooledProjectile currentProjectile = null;

			addNewProjectileToList(ref currentProjectile);
			
			iterateThroughCreatedProjectiles(ref currentProjectile);

			if (projectiles.Any(b => b.Projectile is LightningGunProjectile))
				Debug.Log("machineGun Projectile when should be lightning:" + projectiles.Find(b => b.Projectile is MachineGunProjectile));
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
					break;
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

		public void DeactivatePooledProjectile(IProjectile projectile)
		{
			projectiles.Find(p => p.Projectile == projectile).Active = false;
		}
	}
}
