using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.DependencyInjection;
using Assets.Scripts.GameObjects;
using Assets.Scripts.Projectiles.Interfaces;
using Assets.Scripts.Character.Interfaces;
using ModestTree.Zenject;
using UnityEngine;

namespace Assets.Scripts.Projectiles
{
	

	public class PooledGameobjects : IPooledGameObjects
	{
		List<GameObject> pooledBullets = new List<GameObject>();
		List<Bullet> bullets = new List<Bullet>();
		[Inject] private IBulletPool bulletPool;
		
		[Inject]
		public PooledGameobjects()
		{
			
		}
		public void Initialize(ICharacter character)
		{
			for (int i = 0; i < bulletPool.GetProjectiles(character).Count; i++)
			{
				addProjectile();
				var b = pooledBullets[i].GetComponent<Bullet>();
				b.Initialize();
				bullets.Add(b);
				pooledBullets[i].SetActive(false);
				SetArt(character, i);
			}
		}

		private void SetArt(ICharacter character, int index)
		{
			pooledBullets[index].gameObject.renderer.material =
				bulletPool.GetProjectiles(character)[index].Projectile.GetMaterial;
			pooledBullets[index].gameObject.GetComponent<MeshFilter>().mesh =
                bulletPool.GetProjectiles(character)[index].Projectile.GetMesh;
		}

		private void addProjectile()
		{
			GameObject go =
				ProjectRoot.Instantiator.Instantiate(Resources.Load<GameObject>("Prefabs/Bullet/Bullet")); //GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Bullet/Bullet")) as GameObject;
			//go.transform.parent = GameObject.Find("ContextView").transform;
			pooledBullets.Add(go);
		}

		private GameObject currentBullet;
		List<IPooledProjectile> projectiles = new List<IPooledProjectile>(100);
		public Bullet GetPooledBullet(ICharacter character)
		{
			currentBullet = null;
			//projectiles = bulletPool.GetProjectiles(character);
			//for (int i = 0; i < pooledBullets.Count; i++)
			//{
			//	for (int j = 0; j < projectiles.Count; j++)
			//	{
			//		if (pooledBullets[i].renderer.material != projectiles[j].Projectile.GetMaterial)
			//		{
			//			if (!pooledBullets[i].activeInHierarchy)
			//				SetArt(character, i);
			//		}
			//	}
			//}
			//bulletPool.GetProjectiles(character).Single(p => p.Projectile.GetMaterial);
			int bulletPlaceInList = -1; 
			if (bulletPool.GetProjectiles(character).Any(p =>
				   pooledBullets.Find(x => x.renderer.material != p.Projectile.GetMaterial)))
			{
				for (int i = 0; i < pooledBullets.Count; i++)
				{
					if (!pooledBullets[i].activeInHierarchy)
					{
						SetArt(character, i);
						bulletPlaceInList = i;
						break;
					}
						
				}
			}

			addNewProjectileToList(character, ref currentBullet);
			
			iterateThroughCreatedProjectiles(ref currentBullet);

			return bullets[bulletPlaceInList];
		}

		private void iterateThroughCreatedProjectiles(ref GameObject currentProjectile)
		{
			for (int i = 0; i < pooledBullets.Count; i++)
			{
				if (!pooledBullets[i].activeInHierarchy)
				{
					pooledBullets[i].SetActive(true);
					currentProjectile = pooledBullets[i];
					break;
				}
			}
		}

		private void addNewProjectileToList(ICharacter character, ref GameObject currentProjectile)
		{
			if (pooledBullets[pooledBullets.Count - 1].activeInHierarchy)
			{
				int lastElement = pooledBullets.Count - 1;
				addProjectile();
				SetArt(character, lastElement);
				pooledBullets[lastElement].SetActive(true);
				currentProjectile = pooledBullets[lastElement];
			}
		}

		public void DeactivatePooledBullet(GameObject bullet, ICharacter character, IProjectile projectile)
		{
			bulletPool.DeactivatePooledProjectile(character, projectile);
			pooledBullets.Find(b => b == bullet).SetActive(false);
		}
	}
}
