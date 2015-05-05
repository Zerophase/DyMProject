using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.DependencyInjection;
using Assets.Scripts.Projectiles.Interfaces;
using Assets.Scripts.Projectiles.Projectiles;
using Assets.Scripts.Weapons.Interfaces;
using ModestTree.Zenject;
using UnityEngine;
using Assets.Scripts.Character.Interfaces;

namespace Assets.Scripts.Projectiles
{
	public class BulletPool : IBulletPool
	{
		[Inject]
		public BulletPool()
		{
		}

		List<IPooledProjectile> pooledProjectiles = new List<IPooledProjectile>();

        private Dictionary<ICharacter, List<IPooledProjectile>> projectilesBoundToCharacterType
            = new Dictionary<ICharacter, List<IPooledProjectile>>();

		[Inject]
		private PooledProjectileFactory pooledProjectileFactory;
		//private IPooledProjectile pooledProjectile;
		//private IProjectile projectile;

		private IRangeWeapon currentRangeWeapon;

        //public List<IPooledProjectile> Projectiles
        //{
        //    get { return  pooledProjectiles; }
        //}

        public Dictionary<ICharacter, List<IPooledProjectile>> ProjectileBoundToCharacterType
        {
            get { return projectilesBoundToCharacterType; }
        }
        public List<IPooledProjectile> GetProjectiles(ICharacter character)
        {
            return projectilesBoundToCharacterType[character];
        }

		public void Initialize(IRangeWeapon rangeWeapon, Vector3 startPosition, int count)
		{
			currentRangeWeapon = rangeWeapon;
            List<IPooledProjectile> temp = new List<IPooledProjectile>();
            temp.Add(new PooledProjectile(rangeWeapon.Projectile));
            projectilesBoundToCharacterType.Add(rangeWeapon.Character, temp);
			changeProjectileType(rangeWeapon);
			for (int i = 0; i < count; i++)
			{
				addProjectile(rangeWeapon);
			}
		}

        private void addProjectile(IRangeWeapon rangeWeapon)
        {
            projectilesBoundToCharacterType[rangeWeapon.Character].Add(pooledProjectileFactory.Create(rangeWeapon.Projectile));
        }

        private void changeProjectileType(IRangeWeapon rangeWeapon)
        {
			if(!projectilesBoundToCharacterType.ContainsKey(rangeWeapon.Character))
				AddCharacterKey(rangeWeapon);
	        projectilesBoundToCharacterType[rangeWeapon.Character].
				Find(x => x.Active == false).Projectile = rangeWeapon.Projectile;
        }

		public void AddCharacterKey(IRangeWeapon rangeWeapon)
		{
			List<IPooledProjectile> temp = new List<IPooledProjectile>();
			temp.Add(new PooledProjectile(rangeWeapon.Projectile));
			projectilesBoundToCharacterType.Add(rangeWeapon.Character, temp);
			for (int i = 0; i < 50; i++)
			{
				addProjectile(rangeWeapon);
			}
		}
        //private void changeProjectileType(IProjectile projectile)
        //{
        //    this.projectile = projectile;
        //}

        // TODO There might be bugs here
		public void ChangeBullet(IRangeWeapon rangeWeapon)
		{
            currentRangeWeapon = rangeWeapon;
			
            changeProjectileType(rangeWeapon);
            for (int i = 0; i < projectilesBoundToCharacterType[rangeWeapon.Character].Count; i++)
            {
                if (!projectilesBoundToCharacterType[rangeWeapon.Character][i].Active)
                    projectilesBoundToCharacterType[rangeWeapon.Character][i] = 
                        pooledProjectileFactory.Create(rangeWeapon.Projectile);
            }
		}

		public IPooledProjectile GetPooledProjectile(IRangeWeapon rangeWeapon)
		{
			IPooledProjectile currentProjectile = null;

            for (int i = 0; i < projectilesBoundToCharacterType[rangeWeapon.Character].Count; i++)
			{
                if (!projectilesBoundToCharacterType[rangeWeapon.Character][i].Active &&
                    !projectilesBoundToCharacterType[rangeWeapon.Character][i].Projectile.Equals(rangeWeapon.Projectile))
				{
                    changeProjectileType(rangeWeapon);
                    projectilesBoundToCharacterType[rangeWeapon.Character][i] = pooledProjectileFactory.Create(rangeWeapon.Projectile);
				}
			}

			addNewProjectileToList(rangeWeapon, ref currentProjectile);
			
			iterateThroughCreatedProjectiles(rangeWeapon, ref currentProjectile);

            //if (pooledProjectiles.Any(b => b.Projectile is LightningGunProjectile))
            //    Debug.Log("machineGun Projectile when should be lightning:" + pooledProjectiles.Find(b => b.Projectile is MachineGunProjectile));
			return currentProjectile;
		}

		private void iterateThroughCreatedProjectiles(IRangeWeapon rangeWeapon, ref IPooledProjectile currentProjectile)
		{
            for (int i = 0; i < projectilesBoundToCharacterType[rangeWeapon.Character].Count; i++)
			{
                if (!projectilesBoundToCharacterType[rangeWeapon.Character][i].Active)
				{
                    projectilesBoundToCharacterType[rangeWeapon.Character][i].Active = true;
                    currentProjectile = projectilesBoundToCharacterType[rangeWeapon.Character][i];
					break;
				}
			}
		}

        private void addNewProjectileToList(IRangeWeapon rangeWeapon, ref IPooledProjectile currentProjectile)
		{
            if (projectilesBoundToCharacterType[rangeWeapon.Character][projectilesBoundToCharacterType[rangeWeapon.Character].Count - 1].Active)
			{
                int lastElement = projectilesBoundToCharacterType[rangeWeapon.Character].Count - 1;
                addProjectile(rangeWeapon);
                projectilesBoundToCharacterType[rangeWeapon.Character][lastElement].Active = true;
                currentProjectile = projectilesBoundToCharacterType[rangeWeapon.Character][lastElement];
			}
		}

		public void DeactivatePooledProjectile(ICharacter character, IProjectile projectile)
		{
            projectilesBoundToCharacterType[character].Find(p => p.Projectile == projectile).Active = false;
		}
	}
}
