using System;
using Assets.Scripts.DependencyInjection;
using Assets.Scripts.Utilities;
using Assets.Scripts.Utilities.Messaging;
using Assets.Scripts.Utilities.Messaging.Interfaces;
using ModestTree.Zenject;
using UnityEngine;

namespace Assets.Scripts.MediatorPattern
{
	public abstract class Mediator : MonoBehaviour
	{
		[Inject]
		protected IMessageDispatcher messageDispatcher;
		protected static PhysicsDirector physicsDirector;

		protected virtual void Awake()
		{
			if (physicsDirector == null)
				physicsDirector = FindObjectOfType<PhysicsDirector>();
		}
	}
}
