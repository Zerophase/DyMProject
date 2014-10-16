using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Character;
using Assets.Scripts.Character.Interfaces;
using Assets.Scripts.Projectiles;
using Assets.Scripts.Projectiles.Interfaces;
using Assets.Scripts.Projectiles.Projectiles;
using Assets.Scripts.Utilities.Messaging;
using Assets.Scripts.Utilities.Messaging.Interfaces;
using Assets.Scripts.Weapons;
using Assets.Scripts.Weapons.Interfaces;
using DyM.UnitTests.Tests.BaseTest;
using NSubstitute;
using NUnit.Framework;
using UnityEditor.VersionControl;
using UnityEngine;

namespace DyM.UnitTests.Tests
{
	[TestFixture]
	public class RangeWeaponTests : WeaponTestBase
	{
		private TestRangeWeapon makeTestRangeWeapon(IReceiver receiver, IMessageDispatcher messageDispatcher,
			IBulletPool bulletPool)
		{
			return new TestRangeWeapon(receiver, messageDispatcher, new Projectile(), bulletPool);
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
			IReceiver receiver = substituteReceiver();
			IMessageDispatcher messageDispatcher = substituteMessageDispatcher();
			IRangeWeapon weapon = makeTestRangeWeapon(receiver, messageDispatcher, bulletPool);

			IProjectile expected = pooledProjectile.Projectile;
			IProjectile actual = weapon.Fire();

			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void PickUp_PicksRangeWeaponUp_ReturnsWeapon()
		{
			ITelegram telegram = Substitute.For<ITelegram>();
			IReceiver receiver = substituteReceiver();
			ICharacter character = Substitute.For<ICharacter>();
			receiver.Owner = character;
			IMessageDispatcher messageDispatcher =
				substituteMessageDispatcher();
			IReceiver receiverWeapon = substituteReceiver();
			IBulletPool bulletPool = Substitute.For<IBulletPool>();
			IRangeWeapon weapon = makeTestRangeWeapon(receiverWeapon, messageDispatcher,bulletPool);
			receiverWeapon.Owner = weapon;

			telegram.Message.Returns(weapon);
			messageDispatcher.SendMessage += receiver.HandleMessage;
			messageDispatcher.When(dispatch => messageDispatcher.DispatchMessage(Arg.Any<Telegram>())).
				Do(x => character.Receive(telegram));
			character.When(receive => character.Receive(telegram)).
				Do(x => character.RangeWeapon.Returns(weapon));
			IRangeWeapon expected = weapon;
			character.Position = Vector3.zero;
			weapon.PickUp(character);
			IRangeWeapon actual = character.RangeWeapon;

			Assert.AreEqual(expected, actual);
		}
	}
}
