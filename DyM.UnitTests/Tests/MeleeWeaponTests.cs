using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Character.Interfaces;
using Assets.Scripts.Utilities.Messaging;
using Assets.Scripts.Utilities.Messaging.Interfaces;
using Assets.Scripts.Weapons;
using Assets.Scripts.Weapons.Interfaces;
using DyM.UnitTests.Tests.BaseTest;
using NSubstitute;
using NUnit.Framework;

namespace DyM.UnitTests.Tests
{
	[TestFixture]
	public class MeleeWeaponTests : WeaponTestBase
	{
		private TestMeleeWeapon makeTestMeleeWeapon()
		{
			return new TestMeleeWeapon();
		}

		private TestMeleeWeapon makeTestMeleeWeapon(IReceiver receiver, 
			IMessageDispatcher messageDispatcher)
		{
			return new TestMeleeWeapon(receiver, messageDispatcher);
		}

		[Test]
		public void Attack_DeterminesDamageValue_ReturnsDamage()
		{
			IMeleeWeapon meleeWeapon = makeTestMeleeWeapon();

			int expected = 10;
			int actual = meleeWeapon.Attack();

			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void PickUp_PicksMeleeWeaponUp_ReturnsWeapon()
		{
			ITelegram telegram = Substitute.For<ITelegram>();
			IReceiver receiver = substituteReceiver();
			ICharacter character = Substitute.For<ICharacter>();
			receiver.Owner = character;
			IMessageDispatcher messageDispatcher =
				substituteMessageDispatcher();
			IReceiver receiverWeapon = substituteReceiver();
			IMeleeWeapon weapon = makeTestMeleeWeapon(receiverWeapon, messageDispatcher);
			receiverWeapon.Owner = weapon;

			telegram.Message.Returns(weapon);
			messageDispatcher.SendMessage += receiver.HandleMessage;
			messageDispatcher.When(dispatch => messageDispatcher.DispatchMessage(Arg.Any<Telegram>())).
				Do(x => character.Receive(telegram));
			character.When(receive => character.Receive(telegram)).
				Do(x => character.MeleeWeapon.Returns(weapon));
			IMeleeWeapon expected = weapon;
			weapon.PickUp(character);
			IMeleeWeapon actual = character.MeleeWeapon;

			Assert.AreEqual(expected, actual);
		}
	}
}
