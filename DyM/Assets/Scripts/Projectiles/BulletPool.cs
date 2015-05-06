﻿using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Character;
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

        private Dictionary<CharacterTypes, List<IPooledProjectile>> projectilesBoundToCharacterType
			= new Dictionary<CharacterTypes, List<IPooledProjectile>>();

		[Inject]
		private PooledProjectileFactory pooledProjectileFactory;
		//private IPooledProjectile pooledProjectile;
		//private IProjectile projectile;

		private IRangeWeapon currentRangeWeapon;

        //public List<IPooledProjectile> Projectiles
        //{
        //    get { return  pooledProjectiles; }
        //}

		public Dictionary<CharacterTypes, List<IPooledProjectile>> ProjectileBoundToCharacterType
        {
            get { return projectilesBoundToCharacterType; }
        }
        public List<IPooledProjectile> GetProjectiles(ICharacter character)
        {
            return projectilesBoundToCharacterType[character.CharacterType];
        }

		public void Initialize(IRangeWeapon rangeWeapon, Vector3 startPosition, int count)
		{
			currentRangeWeapon = rangeWeapon;
            List<IPooledProjectile> temp = new List<IPooledProjectile>();
            temp.Add(new PooledProjectile(rangeWeapon.Projectile));
			if(!projectilesBoundToCharacterType.ContainsKey(rangeWeapon.Character.CharacterType))
				projectilesBoundToCharacterType.Add(rangeWeapon.Character.CharacterType, temp);
			changeProjectileType(rangeWeapon);
			for (int i = 0; i < count; i++)
			{
				addProjectile(rangeWeapon);
			}
		}

        private void addProjectile(IRangeWeapon rangeWeapon)
        {
			projectilesBoundToCharacterType[rangeWeapon.Character.CharacterType].Add(pooledProjectileFactory.Create(rangeWeapon.Projectile));
        }

        private void changeProjectileType(IRangeWeapon rangeWeapon)
        {
			if (!projectilesBoundToCharacterType.ContainsKey(rangeWeapon.Character.CharacterType))
				AddCharacterKey(rangeWeapon);
			projectilesBoundToCharacterType[rangeWeapon.Character.CharacterType][0].Projectile = rangeWeapon.Projectile;
        }

		public void AddCharacterKey(IRangeWeapon rangeWeapon)
		{
			if (!projectilesBoundToCharacterType.ContainsKey(rangeWeapon.Character.CharacterType))
			{
				List<IPooledProjectile> temp = new List<IPooledProjectile>();
				temp.Add(new PooledProjectile(rangeWeapon.Projectile));
				projectilesBoundToCharacterType.Add(rangeWeapon.Character.CharacterType, temp);
				for (int i = 0; i < 100; i++)
				{
					addProjectile(rangeWeapon);
				}
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
			for (int i = 0; i < projectilesBoundToCharacterType[rangeWeapon.Character.CharacterType].Count; i++)
            {
				if (!projectilesBoundToCharacterType[rangeWeapon.Character.CharacterType][i].Active)
					projectilesBoundToCharacterType[rangeWeapon.Character.CharacterType][i] = 
                        pooledProjectileFactory.Create(rangeWeapon.Projectile);
            }
		}

		public IPooledProjectile GetPooledProjectile(IRangeWeapon rangeWeapon)
		{
			IPooledProjectile currentProjectile = null;

			for (int i = 0; i < projectilesBoundToCharacterType[rangeWeapon.Character.CharacterType].Count; i++)
			{
				if (!projectilesBoundToCharacterType[rangeWeapon.Character.CharacterType][i].Active &&
					!projectilesBoundToCharacterType[rangeWeapon.Character.CharacterType][i].Projectile.Equals(rangeWeapon.Projectile))
				{
                    changeProjectileType(rangeWeapon);
					projectilesBoundToCharacterType[rangeWeapon.Character.CharacterType][i] = pooledProjectileFactory.Create(rangeWeapon.Projectile);
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
			for (int i = 0; i < projectilesBoundToCharacterType[rangeWeapon.Character.CharacterType].Count; i++)
			{
				if (!projectilesBoundToCharacterType[rangeWeapon.Character.CharacterType][i].Active)
				{
					projectilesBoundToCharacterType[rangeWeapon.Character.CharacterType][i].Active = true;
					currentProjectile = projectilesBoundToCharacterType[rangeWeapon.Character.CharacterType][i];
					break;
				}
			}
		}

        private void addNewProjectileToList(IRangeWeapon rangeWeapon, ref IPooledProjectile currentProjectile)
		{
			if (projectilesBoundToCharacterType[rangeWeapon.Character.CharacterType][projectilesBoundToCharacterType[rangeWeapon.Character.CharacterType].Count - 1].Active)
			{
				int lastElement = projectilesBoundToCharacterType[rangeWeapon.Character.CharacterType].Count - 1;
                addProjectile(rangeWeapon);
				projectilesBoundToCharacterType[rangeWeapon.Character.CharacterType][lastElement].Active = true;
				currentProjectile = projectilesBoundToCharacterType[rangeWeapon.Character.CharacterType][lastElement];
			}
		}

		public void DeactivatePooledProjectile(ICharacter character, IProjectile projectile)
		{
            projectilesBoundToCharacterType[character.CharacterType].Find(p => p.Projectile == projectile).Active = false;
		}
	}
}
