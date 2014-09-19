using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Abilities;
using Assets.Scripts.Abilities.Interfaces;
using Assets.Scripts.Character.Interfaces;
using Assets.Scripts.Utilities.Messaging.Interfaces;
using NSubstitute;
using NSubstitute.Routing.Handlers;
using NUnit.Framework;

namespace DyM.UnitTests.Tests
{
	[TestFixture]
	public class AbilityTests
	{
		private IAbility makeAbility(IMessageDispatcher messageDispatcher)
		{
			return new Ability(messageDispatcher);
		}
		[Test]
		public void PickUP_AddAbilityToListOfAbilities_AbilityListHasAValue()
		{
			ITelegram telegram = Substitute.For<ITelegram>();
			IReceiver receiver = Substitute.For<IReceiver>();
			ICharacter character = Substitute.For<ICharacter>();
			receiver.Owner = character;
			IMessageDispatcher messageDispatcher = Substitute.For<IMessageDispatcher>();

			IAbility ability = makeAbility(messageDispatcher);
			telegram.Message.Returns(ability);
			messageDispatcher.SendMessage += receiver.HandleMessage;
			messageDispatcher.When(dispatch => messageDispatcher.DispatchMessage(telegram)).
				Do(x => character.Receive(telegram));

			
			character.Receiver = receiver;
			character.When(receive => character.Receive(telegram)).
				Do(x => character.Ability.Returns(ability));
			IAbility expected = ability;
			ability.PickUp(character);
			IAbility actual = character.Ability;

			Assert.AreEqual(expected, actual);
		}
	}
}
