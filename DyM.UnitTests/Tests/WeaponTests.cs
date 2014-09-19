using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Character;
using Assets.Scripts.Character.Interfaces;
using Assets.Scripts.Projectiles;
using Assets.Scripts.Projectiles.Interfaces;
using Assets.Scripts.Utilities.Messaging;
using Assets.Scripts.Utilities.Messaging.Interfaces;
using Assets.Scripts.Weapons;
using Assets.Scripts.Weapons.Interfaces;
using NSubstitute;
using NUnit.Framework;
using UnityEditor.VersionControl;
using UnityEngine;

namespace DyM.UnitTests.Tests
{
	[TestFixture]
	public class WeaponTests
	{
		private IReceiver makeReceiver()
		{
			return Substitute.For<IReceiver>();
		}

		private IMessageDispatcher makeMessageDispatcher()
		{
			return Substitute.For<IMessageDispatcher>();
		}

		[Test]
		public void Fire_FiresWeaponAttack_ReturnsBullet()
		{
			IProjectile projectitle = Substitute.For<IProjectile>();
			IPooledProjectile pooledProjectile = Substitute.For<IPooledProjectile>();
			pooledProjectile.Projectile.Returns(projectitle);
			IBulletPool bulletPool = Substitute.For<IBulletPool>();
			bulletPool.Projectile = pooledProjectile;
			bulletPool.GetPooledProjectile().Returns(pooledProjectile);
			IReceiver receiver = makeReceiver();
			IMessageDispatcher messageDispatcher = makeMessageDispatcher();
			IWeapon weapon = new TestWeapon(receiver, messageDispatcher, bulletPool);

			IProjectile expected = pooledProjectile.Projectile;
			IProjectile actual = weapon.Fire();

			Assert.AreEqual(expected, actual);
		}

		//TODO fix test so it passes with current implementation
		[Test]
		public void PickUp_PicksWeaponUp_ReturnsWeapon()
		{
			ITelegram telegram = Substitute.For<ITelegram>();
			IMessageDispatcher messageDispatcher =
				makeMessageDispatcher();
			messageDispatcher.DispatchMessage(telegram);
			IReceiver receiverCharacter = 
				Substitute.For<Receiver>(messageDispatcher);
			//receiverCharacter.HandleMessage(telegram);
			IReceiver receiverWeapon = 
				Substitute.For<Receiver>(messageDispatcher);
			IBulletPool bulletPool = Substitute.For<IBulletPool>();
			ICharacter character = Substitute.For<TestCharacter>(receiverCharacter);
			IWeapon weapon = new TestWeapon(receiverWeapon, messageDispatcher,bulletPool);
			weapon.Position = Vector3.zero;

			IWeapon expected = weapon;
			character.Position = Vector3.zero;
			weapon.PickUp(character);
			IWeapon actual = character.Weapon;

			Assert.AreEqual(expected, actual);
		}
	}
}
