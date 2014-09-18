using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Projectiles;
using Assets.Scripts.Projectiles.Interfaces;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace DyM.UnitTests.Tests
{
	[TestFixture]
	public class ProjectileTests
	{

		private IBulletPool makeBulletPool()
		{
			return new BulletPool();
		}

		private IPooledProjectile substituteForPooledProjectile()
		{
			return Substitute.For<IPooledProjectile>();
		}

		[Test]
		public void InitializePool_PoolContainsBulletCount_BulletCountIsTen()
		{
			IBulletPool bulletPool = makeBulletPool();
			IPooledProjectile projectile = substituteForPooledProjectile();

			int expected = 10;
			bulletPool.Projectile = projectile;
			bulletPool.Initialize(new Vector3(), 10);
			int actual = bulletPool.Projectiles.Count;

			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void GetPooledProjectiles_PoolHasProjectiles_ReturnsProjectile()
		{
			IBulletPool bulletPool = makeBulletPool();
			IPooledProjectile projectile = substituteForPooledProjectile();

			IProjectile expected = projectile.Projectile;
			bulletPool.Projectile = projectile;
			bulletPool.Initialize(new Vector3(), 10);
			IPooledProjectile actual = bulletPool.GetPooledProjectile();

			Assert.AreEqual(expected, actual.Projectile);
		}

		[Test]
		public void GetPooledProjectiles_PooledProjectileIsActive_StructValueIsTrue()
		{
			IBulletPool bulletPool = makeBulletPool();
			IPooledProjectile projectile = substituteForPooledProjectile();
			projectile.Active.Returns(false);

			bool expected = true;
			bulletPool.Projectile = projectile;
			bulletPool.Initialize(new Vector3(), 10);
			bool actual = bulletPool.GetPooledProjectile().Active;

			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void GetPooledProjectiles_AllProjectilesAreActive_CreateNewProjectile()
		{
			IBulletPool bulletPool = makeBulletPool();
			IPooledProjectile projectile = substituteForPooledProjectile();
			projectile.Active.Returns(true);

			IPooledProjectile expected = projectile;
			bulletPool.Projectile = projectile;
			bulletPool.Initialize(new Vector3(), 10);
			IPooledProjectile actual = bulletPool.GetPooledProjectile();

			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void GetPooledProjectiles_AllProjectilesAreActive_CountIncreasesByOne()
		{
			IBulletPool bulletPool = makeBulletPool();
			IPooledProjectile projectile = substituteForPooledProjectile();
			projectile.Active.Returns(true);

			int expected = 11;
			bulletPool.Projectile = projectile;
			bulletPool.Initialize(new Vector3(), 10);
			bulletPool.GetPooledProjectile();
			int actual = bulletPool.Projectiles.Count;

			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void DeactivatePooledProjectile_ProjectileIsActive_SetProjectileActiveToFalse()
		{
			IBulletPool bulletPool = makeBulletPool();
			IPooledProjectile projectile = substituteForPooledProjectile();
			projectile.Active.Returns(true);

			bool expected = false;
			bulletPool.Projectile = projectile;
			bulletPool.Initialize(new Vector3(), 10);
			bulletPool.DeactivatePooledProjectile(projectile);
			bool actual = bulletPool.Projectiles.Find(p => p.Active == false).Active;

			Assert.AreEqual(expected, actual);
		}
	}
}
