using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Text;
using Assets.Scripts.ObjectManipulation.Interfaces;
using Assets.Scripts.Utilities;
using Assets.Scripts.Utilities.Messaging;
using Assets.Scripts.Utilities.Messaging.Interfaces;
using ModestTree.Zenject;
using UnityEngine;

namespace Assets.Scripts.MediatorPattern
{
	public abstract class MovablePhysicsMediator : PhysicsMediator
	{
		[Inject]
		protected ICardinalMovement cardinalMovement;

		public Vector3 Gravity { set { cardinalMovement.Gravity = value; } }

		public bool HasJumped { set { cardinalMovement.HasJumped = value; } }
	}
}
