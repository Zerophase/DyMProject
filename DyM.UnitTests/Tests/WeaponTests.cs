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
		[Test]
		public void Fire_FiresWeaponAttack_ReturnsBullet()
		{
			IProjectile projectitle = Substitute.For<Projectile>();
			IWeapon weapon = new TestWeapon(projectitle);

			IProjectile expected = projectitle;
			IProjectile actual = weapon.Fire();

			Assert.AreEqual(expected, actual);
		}

		// TODO Make Messaging so, this works
		[Test]
		public void PickUp_PicksWeaponUp_ReturnsWeapon()
		{
			IMessageDispatcher messageDispatcher = 
				Substitute.For<MessageDispatcher>();
			IReceiver receiverCharacter = 
				Substitute.For<Receiver>(messageDispatcher);
			IReceiver receiverWeapon = 
				Substitute.For<Receiver>(messageDispatcher);
			ICharacter character = Substitute.For<TestCharacter>(receiverCharacter);
			IWeapon weapon = new TestWeapon(receiverWeapon, messageDispatcher);
			weapon.Position = Vector3.zero;

			IWeapon expected = weapon;
			character.Position = Vector3.zero;
			weapon.PickUp(character);
			IWeapon actual = character.Weapon;

			Assert.AreEqual(expected, actual);
		}
	}
}
