using System;
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
		
		
	}
}
