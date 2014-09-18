using System.Collections.Generic;
using Assets.Scripts.Projectiles.Interfaces;
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
		private IPooledProjectile projectile;

		// For Tests
		public IPooledProjectile Projectile { set { projectile = value; } }

		public List<IPooledProjectile> Projectiles
		{
			get { return  projectiles; }
		}

		public void Initialize(Vector3 startPosition, int count)
		{
			for (int i = 0; i < count; i++)
			{
				addProjectile();
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
