using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Utilities.CustomExceptions;
using Assets.Scripts.Utilities.Messaging;
using Assets.Scripts.Utilities.Messaging.Interfaces;
using NSubstitute;
using NUnit.Framework;
using UnityEditor.VersionControl;

namespace DyM.UnitTests.Tests
{
	[TestFixture]
	public class MessageDispatcherTests
	{
		private MessageDispatcher makeDispatcher()
		{
			return new MessageDispatcher();
		}

		[Test]
		public void DispatchMessage_SendsOutMessageToReceivers_ReceiverReceivesMesage()
		{
			IMessageDispatcher messageDispatcher = makeDispatcher();
			IReceiver receiver = Substitute.For<IReceiver>();

			ITelegram expected = Substitute.For<ITelegram>();
			receiver.When(receivedMessage => receiver.HandleMessage(expected))
				.Do(x => receiver.TestTelegram.Returns(expected));
			messageDispatcher.SendMessage += receiver.HandleMessage;
			messageDispatcher.DispatchMessage(expected);
			ITelegram actual = receiver.TestTelegram;

			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void DispatchMessage_NoReceiverIsAttached_DoesNothing()
		{
			IMessageDispatcher messageDispatcher = makeDispatcher();
			IReceiver receiver = Substitute.For<IReceiver>();
			ITelegram telegram = Substitute.For<ITelegram>();

			ITelegram expected = null;
			receiver.When(receivedMessage => receiver.HandleMessage(expected))
				.Do(x => receiver.TestTelegram.Returns(expected));
			messageDispatcher.SendMessage += receiver.HandleMessage;
			messageDispatcher.DispatchMessage(expected);
			ITelegram actual = receiver.TestTelegram;

			Assert.AreEqual(expected, actual);
		}
	}
}
