using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Text;
using Assets.Scripts.ObjectManipulation.Interfaces;
using Assets.Scripts.Utilities.Messaging;
using Assets.Scripts.Utilities.Messaging.Interfaces;
using ModestTree.Zenject;
using UnityEngine;

namespace Assets.Scripts.MediatorPattern
{
	public abstract class PhysicsMediator : Mediator
	{
		[Inject]
		protected ICardinalMovement cardinalMovement;

		public Vector3 Gravity { set { cardinalMovement.Gravity = value; } }

		private static PhysicsDirector physicsDirector;

		protected virtual void Awake()
		{
			if (physicsDirector == null)
				physicsDirector = FindObjectOfType<PhysicsDirector>();
		}

		protected virtual void Start()
		{
			messageDispatcher.DispatchMessage(new Telegram(physicsDirector, this));
			var Foo = typeof (PhysicsDirector);
			IOwner o = Foo as IOwner;
		}
	}
}
