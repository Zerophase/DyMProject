using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Utilities.Messaging.Interfaces;
using Assets.Scripts.Weapons.Bases;
using ModestTree.Zenject;

namespace Assets.Scripts.Weapons
{
	public class TestMeleeWeapon : MeleeWeaponBase
	{
		public TestMeleeWeapon() : base()
		{
			
		}

		[Inject]
		public TestMeleeWeapon(IReceiver receiver, IMessageDispatcher messageDispatcher) :
			base(receiver, messageDispatcher)
		{
			
		}
	}
}