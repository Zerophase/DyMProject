using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Projectiles;
using Assets.Scripts.Projectiles.Interfaces;
using Assets.Scripts.Weapons;
using Assets.Scripts.Weapons.Interfaces;
using NSubstitute;
using NUnit.Framework;

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
	}
}
