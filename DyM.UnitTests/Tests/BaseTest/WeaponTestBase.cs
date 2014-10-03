using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Utilities.Messaging.Interfaces;
using NSubstitute;

namespace DyM.UnitTests.Tests.BaseTest
{
	public class WeaponTestBase
	{
		protected IReceiver substituteReceiver()
		{
			return Substitute.For<IReceiver>();
		}

		protected IMessageDispatcher substituteMessageDispatcher()
		{
			return Substitute.For<IMessageDispatcher>();
		}
	}
}
