using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Character;
using Assets.Scripts.Character.Interfaces;
using Assets.Scripts.Weapons;
using Assets.Scripts.Weapons.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace DyM.UnitTests.Tests
{
	[TestFixture]
	public class CharacterTest
	{
		[Test]
		public void Equip_WeaponGetsEquiped_SetsEquippedWeapon()
		{
			IWeapon weapon = Substitute.For<IWeapon>();
			ICharacter character = new TestCharacter();

			IWeapon expected = weapon;
			character.Equip(weapon);
			IWeapon actual = character.Weapon;

			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void SwitchWeapon_SwitchWeaponEquipped_SetsEquippedWeapon()
		{
			BaseWeapon weapon = Substitute.For<TestWeapon>();
			BaseWeapon weapon2 = Substitute.For<TestWeapon2>();
			ICharacter character = new TestCharacter();

			BaseWeapon expected = weapon2;
			character.AddWeapon(weapon);
			character.AddWeapon(weapon2);
			character.Equip(weapon);
			character.SwitchWeapon();
			BaseWeapon actual = (BaseWeapon)character.Weapon;

			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void SwitchWeapon_SwitchesToNextWeaponWhenMultipleWeaponsArePickedUp_SetsEquippedWeapon()
		{
			BaseWeapon weapon = Substitute.For<TestWeapon>();
			BaseWeapon weapon2 = Substitute.For<TestWeapon2>();
			BaseWeapon weapon3 = Substitute.For<TestWeapon3>();
			ICharacter character = new TestCharacter();

			BaseWeapon expected = weapon2;
			character.AddWeapon(weapon);
			character.AddWeapon(weapon3);
			character.AddWeapon(weapon2);
			character.Equip(weapon);
			character.SwitchWeapon();
			BaseWeapon actual = (BaseWeapon)character.Weapon;

			Assert.AreEqual(expected, actual);
		}
	}
}
