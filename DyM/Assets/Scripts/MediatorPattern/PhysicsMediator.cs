using System;
using Assets.Scripts.CollisionBoxes.ThreeD;
using Assets.Scripts.DependencyInjection;
using Assets.Scripts.Utilities;
using Assets.Scripts.Utilities.Messaging;
using Assets.Scripts.Utilities.Messaging.Interfaces;
using ModestTree.Zenject;
using UnityEngine;
using Assets.Scripts.GameObjects;

namespace Assets.Scripts.MediatorPattern
{
	public abstract class PhysicsMediator : MonoBehaviour
	{
		[Inject]
		protected IMessageDispatcher messageDispatcher;
		protected static PhysicsDirector physicsDirector;

		private AABB3D boundingBox;

		public AABB3D BoundingBox
		{
			get { return boundingBox; }
			set { boundingBox = value; }
		}
		
		public Vector3 velocity = Vector3.zero;
		
		private Vector3 previousPosition;

        protected virtual void Awake()
        {
	        if (physicsDirector == null)
	        {
				physicsDirector = FindObjectOfType<PhysicsDirector>();
		        physicsDirector.Initialize();
	        } 
        }

		protected virtual void Start()
		{
			constructBox3D();
			previousPosition = transform.position;
            messageDispatcher.DispatchMessage(new Telegram(physicsDirector, this));
		}

		private void constructBox3D()
		{
			var boxCollider = gameObject.GetComponent<BoxCollider>();
			boundingBox = new AABB3D(transform.position,
				boxCollider.size.x * transform.lossyScale.x,
				boxCollider.size.y * transform.lossyScale.y,
				boxCollider.size.z * transform.lossyScale.z);
		}

		public void UpdateVelocity(Vector3 velocity)
		{
			boundingBox.Velocity += velocity;
		}

		public void ResetVelocity()
		{
			boundingBox.Velocity = Vector3.zero;
		}

		public void UpdatePosition()
		{
			var framePosition = boundingBox.Velocity*Time.deltaTime;
			boundingBox.Center += framePosition;
			transform.Translate(framePosition, Space.World);
		}

		public void UpdatePlane(Vector3 planeChange)
		{
			boundingBox.Center += planeChange;
			transform.Translate(planeChange, Space.World);
		}

		public void UpdatePlane(float zPosition)
		{
			var updatedPosition = new Vector3(0.0f, 0.0f, zPosition);
			boundingBox.Center += updatedPosition;
		}
		//private float Timer;
		//protected virtual void Update()
		//{
		//	boundingBox.Center = transform.position;
		//	//velocity = (transform.position - previousPosition) / Time.deltaTime;

		//	//previousPosition = transform.position;
		//	//box3D.UpdateBox3D(gameObject);
		//	//box3D.xVelocity = velocity.x;
		//	//box3D.yVelocity = velocity.y;
		//	//box3D.zVelocity = velocity.z;
		//}
	}
}
