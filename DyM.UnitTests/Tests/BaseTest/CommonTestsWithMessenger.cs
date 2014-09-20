using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Utilities.Messaging.Interfaces;
using NSubstitute;

namespace DyM.UnitTests.Tests.BaseTest
{
	public class CommonTestsWithMessenger
	{
		protected ITelegram telegram;
		protected IReceiver receiver;
		protected IMessageDispatcher messageDispatcher;

		protected void makeMessageSystem()
		{
			telegram = Substitute.For<ITelegram>();
			receiver = Substitute.For<IReceiver>();
			messageDispatcher = Substitute.For<IMessageDispatcher>();
		}
	}
}
