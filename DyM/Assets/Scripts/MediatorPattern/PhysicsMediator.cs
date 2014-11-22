using System;
using Assets.Scripts.DependencyInjection;
using Assets.Scripts.Utilities;
using Assets.Scripts.Utilities.Messaging;
using Assets.Scripts.Utilities.Messaging.Interfaces;
using ModestTree.Zenject;
using UnityEngine;

namespace Assets.Scripts.MediatorPattern
{
	public abstract class PhysicsMediator : MonoBehaviour
	{
		[Inject]
		protected IMessageDispatcher messageDispatcher;
		protected static PhysicsDirector physicsDirector;

		private Box3D box3D;
		public Box3D GetBox3D { get { return box3D; } }

		private Vector3 previousPosition;

		protected virtual void Start()
		{
			constructBox3D();
			previousPosition = transform.position;
			messageDispatcher.DispatchMessage(new Telegram(physicsDirector, this));
		}

		private void constructBox3D()
		{
			BoxCollider aCollider = gameObject.GetComponent<BoxCollider>();

			Vector3 aPos = transform.position;

			aPos += aCollider.center;
			aPos.x -= (aCollider.size.x * transform.lossyScale.x) / 2;
			aPos.y += (aCollider.size.y * transform.lossyScale.y) / 2;
			aPos.z += (aCollider.size.z * transform.lossyScale.z) / 2;

			box3D = new Box3D(aPos.x, aPos.y, aPos.z, 
				aCollider.size.x * transform.lossyScale.x, 
				-aCollider.size.y * transform.lossyScale.y, 
				-aCollider.size.z * transform.lossyScale.z, aCollider);
		}

		protected virtual void Awake()
		{
			if (physicsDirector == null)
				physicsDirector = FindObjectOfType<PhysicsDirector>();
		}

		private float Timer;
		protected virtual void Update()
		{
			var velocity = (transform.position - previousPosition)/Time.deltaTime;
			
			previousPosition = transform.position;
			box3D.UpdateBox3D(gameObject);
			box3D.xVelocity = velocity.x;
			box3D.yVelocity = velocity.y;
			box3D.zVelocity = velocity.z;
		}
	}
}
