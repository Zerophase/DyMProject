using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Abilities.Interfaces;
using Assets.Scripts.Character;
using Assets.Scripts.Character.Interfaces;
using Assets.Scripts.StatusEffects;
using Assets.Scripts.Utilities.Messaging;
using Assets.Scripts.Weapons;
using Assets.Scripts.Weapons.Bases;
using Assets.Scripts.Weapons.Interfaces;
using DyM.UnitTests.Tests.BaseTest;
using NSubstitute;
using NUnit.Framework;

namespace DyM.UnitTests.Tests
{
	//todo refactor to use interfaces vs actual classes.
	[TestFixture]
	public class CharacterTest : CommonTestsWithMessenger
	{
		[Test]
		public void Equip_MeleeWeaponGetsEquiped_SetsMeleeEquippedWeapon()
		{
			IMeleeWeapon meleeWeapon = Substitute.For<IMeleeWeapon>();
			ICharacter character = new TestCharacter();

			IMeleeWeapon expected = meleeWeapon;
			character.Equip(meleeWeapon);
			IMeleeWeapon actual = character.MeleeWeapon;
			
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void Equip_RangeWeaponGetsEquiped_SetsRangeEquippedWeapon()
		{
			IRangeWeapon weapon = Substitute.For<IRangeWeapon>();
			ICharacter character = new TestCharacter();

			IRangeWeapon expected = weapon;
			character.Equip(weapon);
			IRangeWeapon actual = character.RangeWeapon;

			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void SwitchWeapon_SwitchWeaponEquipped_SetsEquippedWeapon()
		{
			RangeWeaponBase weapon = Substitute.For<TestRangeWeapon>();
			RangeWeaponBase weapon2 = Substitute.For<TestWeapon2>();
			ICharacter character = new TestCharacter();

			RangeWeaponBase expected = weapon2;
			character.AddWeapon(weapon);
			character.AddWeapon(weapon2);
			character.Equip(weapon);
			character.SwitchWeapon();
			RangeWeaponBase actual = (RangeWeaponBase)character.RangeWeapon;

			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void SwitchWeapon_SwitchesToNextWeaponWhenMultipleWeaponsArePickedUp_SetsEquippedWeapon()
		{
			RangeWeaponBase weapon = Substitute.For<TestRangeWeapon>();
			RangeWeaponBase weapon2 = Substitute.For<TestWeapon2>();
			RangeWeaponBase weapon3 = Substitute.For<TestWeapon3>();
			ICharacter character = new TestCharacter();

			RangeWeaponBase expected = weapon2;
			character.AddWeapon(weapon);
			character.AddWeapon(weapon3);
			character.AddWeapon(weapon2);
			character.Equip(weapon);
			character.SwitchWeapon();
			RangeWeaponBase actual = (RangeWeaponBase)character.RangeWeapon;

			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void Receive_GainStatusEffectAddsSecondStatusEffect_ReturnsTwoStatusEffects()
		{
			makeMessageSystem();
			ICharacter character = new TestCharacter();
			IAbility ability = Substitute.For<IAbility>();
			IAbility abilityTwo = Substitute.For<IAbility>();
			int count = 0;
			StatusEffect temp = StatusEffect.NONE;

			receiver.Owner = character;
			ability.When(activates => ability.Activate(character))
				.Do(x => messageDispatcher.DispatchMessage(telegram));
			abilityTwo.When(activates => abilityTwo.Activate(character))
				.Do(x => messageDispatcher.DispatchMessage(telegram));
			messageDispatcher.When(dispatch =>
				messageDispatcher.DispatchMessage(telegram))
					.Do(x => receiver.HandleMessage(telegram));
			receiver.When(received => receiver.HandleMessage(telegram))
				.Do(x => receiver.Owner.Receive(telegram));
			StatusEffect expected = StatusEffect.TEST | StatusEffect.TESTTWO;
			telegram.Message.Returns(StatusEffect.TEST);
			ability.Activate(character);
			telegram.Message.Returns(StatusEffect.TESTTWO);
			abilityTwo.Activate(character);
			StatusEffect actual = character.StatusEffect;

			Assert.AreEqual(expected, actual);
		}
	}
}
