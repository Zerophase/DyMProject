using System.Collections.Generic;
using Assets.Scripts.DependencyInjection;
using Assets.Scripts.Projectiles.Interfaces;
using ModestTree.Zenject;
using UnityEngine;

namespace Assets.Scripts.Projectiles
{
	public interface IPooledGameObjects
	{
		void Initialize();
		GameObject GetPooledBullet();
		void DeactivatePooledBullet(GameObject bullet);
	}

	public class PooledGameobjects : IPooledGameObjects
	{
		List<GameObject> pooledBullets = new List<GameObject>();

		[Inject] private IBulletPool bulletPool;
		//[PostInject]
		[Inject]
		public PooledGameobjects()
		{
			
		}
		public void Initialize()
		{
			for (int i = 0; i < bulletPool.Projectiles.Count; i++)
			{
				addProjectile();
				pooledBullets[i].SetActive(false);
			}
		}

		private void addProjectile()
		{
			GameObject go =
				ProjectRoot.Instantiator.Instantiate(Resources.Load<GameObject>("Prefabs/Bullet/Bullet")); //GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Bullet/Bullet")) as GameObject;
			//go.transform.parent = GameObject.Find("ContextView").transform;
			pooledBullets.Add(go);
		}

		public GameObject GetPooledBullet()
		{
			GameObject currentBullet = null;

			addNewProjectileToList(ref currentBullet);

			iterateThroughCreatedProjectiles(ref currentBullet);

			return currentBullet;
		}

		private void iterateThroughCreatedProjectiles(ref GameObject currentProjectile)
		{
			for (int i = 0; i < pooledBullets.Count; i++)
			{
				if (!pooledBullets[i].activeInHierarchy)
				{
					pooledBullets[i].SetActive(true);
					currentProjectile = pooledBullets[i];
				}
			}
		}

		private void addNewProjectileToList(ref GameObject currentProjectile)
		{
			if (pooledBullets[pooledBullets.Count - 1].activeInHierarchy)
			{
				int lastElement = pooledBullets.Count - 1;
				addProjectile();
				pooledBullets[lastElement].SetActive(true);
				currentProjectile = pooledBullets[lastElement];
			}
		}

		public void DeactivatePooledBullet(GameObject bullet)
		{
			pooledBullets.Find(b => b == bullet).SetActive(false);
		}
	}
}
