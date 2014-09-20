using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Abilities;
using Assets.Scripts.Abilities.Interfaces;
using Assets.Scripts.Character.Interfaces;
using Assets.Scripts.StatusEffects;
using Assets.Scripts.Utilities.Messaging;
using Assets.Scripts.Utilities.Messaging.Interfaces;
using DyM.UnitTests.Tests.BaseTest;
using NSubstitute;
using NSubstitute.Routing.Handlers;
using NUnit.Framework;

namespace DyM.UnitTests.Tests
{
	[TestFixture]
	public class AbilityTests : CommonTestsWithMessenger
	{
		private IAbility makeAbility(IMessageDispatcher messageDispatcher)
		{
			return new Ability(messageDispatcher);
		}

		private IAbility makeAbilityTwo(IMessageDispatcher messageDispatcher)
		{
			return new AbilityTwo(messageDispatcher);
		}

		[Test]
		public void PickUp_AddAbilityToListOfAbilities_AbilityListHasAValue()
		{
			makeMessageSystem();
			ICharacter character = Substitute.For<ICharacter>();
			IAbility ability = makeAbility(messageDispatcher);

			receiver.Owner = character;
			telegram.Message.Returns(ability);
			messageDispatcher.When(dispatch => messageDispatcher.DispatchMessage(Arg.Any<Telegram>())).
				Do(x => character.Receive(telegram));
			character.When(receive => character.Receive(telegram)).
				Do(x => character.Ability.Returns(ability));
			IAbility expected = ability;
			ability.PickUp(character);
			IAbility actual = character.Ability;

			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void Activate_AbilityActivatesSendingOutMessage_CharacterHassCorrectStatusEffect()
		{
			makeMessageSystem();
			ICharacter character = Substitute.For<ICharacter>();
			IAbility ability = makeAbility(messageDispatcher);

			receiver.Owner = character;
			telegram.Message.Returns(StatusEffect.TEST);
			messageDispatcher.When(dispatch => 
				messageDispatcher.DispatchMessage(Arg.Any<Telegram>()))
					.Do(x => character.Receive(telegram) );
			character.When(receive => character.Receive(telegram)).
				Do(x => character.StatusEffect.Returns(StatusEffect.TEST));
			StatusEffect expected = StatusEffect.TEST;
			ability.Activate(character);
			StatusEffect actual = character.StatusEffect;

			Assert.AreEqual(expected, actual);
		}

		//TODO move to character tests.
		[Test]
		public void Activate_AbilityActivatesOnTwoAbilities_StackingBothStatusEffects()
		{
			makeMessageSystem();
			ICharacter character = Substitute.For<ICharacter>();
			IAbility ability = makeAbility(messageDispatcher);
			IAbility abilityTwo = makeAbilityTwo(messageDispatcher);
			int count = 0;
			StatusEffect temp = StatusEffect.NONE;

			receiver.Owner = character;
			telegram.Message.Returns(StatusEffect.TEST);
			messageDispatcher.When(dispatch =>
				messageDispatcher.DispatchMessage(Arg.Any<Telegram>()))
					.Do(x => character.Receive(telegram));
			character.When(receive => character.Receive(telegram)).
				Do(x => character.StatusEffect.Returns((count == 0) ? StatusEffect.TEST :
					StatusEffect.TEST | StatusEffect.TESTTWO));
			StatusEffect expected = StatusEffect.TEST | StatusEffect.TESTTWO;
			ability.Activate(character);
			count++;
			abilityTwo.Activate(character);
			StatusEffect actual = character.StatusEffect;

			Assert.AreEqual(expected, actual);
		}
	}
}
