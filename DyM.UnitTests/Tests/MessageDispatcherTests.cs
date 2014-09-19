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
			IReceiver receiver = Substitute.For<Receiver>();

			ITelegram expected = new Telegram();
			messageDispatcher.SendMessage += receiver.HandleMessage;
			messageDispatcher.DispatchMessage(expected);
			ITelegram actual = receiver.TestTelegram;

			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void DispatchMessage_NoReceiverIsAttached_DoesNothing()
		{
			IMessageDispatcher messageDispatcher = makeDispatcher();
			IReceiver receiver = Substitute.For<Receiver>();

			ITelegram expected = null;
			messageDispatcher.DispatchMessage(new Telegram());
			ITelegram actual = receiver.TestTelegram;

			Assert.AreEqual(expected, actual);
		}
	}
}
