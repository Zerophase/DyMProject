using System;
using Assets.Scripts.Utilities.Messaging;
using Assets.Scripts.Utilities.Messaging.Interfaces;
using ModestTree.Zenject;
using UnityEngine;

namespace Assets.Scripts.MediatorPattern
{
	public class Mediator : MonoBehaviour
	{
		[Inject]
		private IMessageDispatcher messageDispatcher;

		private static Director director;
		
		void Awake()
		{
			if(director == null)
				director = FindObjectOfType<Director>();
		}

		// Use this for initialization
		void Start () 
		{
			messageDispatcher.DispatchMessage(new Telegram(director, this));
		}
	
		// Update is called once per frame
		void Update () 
		{
			
		}
	}
}
