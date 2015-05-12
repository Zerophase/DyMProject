using Assets.Scripts.CollisionBoxes.ThreeD;
using vc = Assets.Scripts.Utilities.Constants.VectorConstants;
using Assets.Scripts.ObjectManipulation.Interfaces;
using Assets.Scripts.Utilities;
using ModestTree.Zenject;
using UnityEngine;

namespace Assets.Scripts.MediatorPattern
{
	public abstract class MovablePhysicsMediator : PhysicsMediator
	{
		[Inject]
		protected ICardinalMovement cardinalMovement;

		private GameObject model;

		private const int preventDeformation = 0;

		protected float speed;
		protected float movementMultiplier;

		AABB3D aabb3D = new AABB3D();
		private AABB3D rotationBox;

		protected override void Start()
		{
			var boxCollider = gameObject.GetComponent<BoxCollider>();
			rotationBox = new AABB3D(Vector3.zero, 
				boxCollider.size.x * transform.lossyScale.x,
				boxCollider.size.y * transform.lossyScale.y,
				boxCollider.size.z * transform.lossyScale.z);
			if (transform.GetChild(preventDeformation).name == "PreventDeformedObject")
			{
				findModelWithDeformationProtection();
			}
			else
			{
				findModelWithoutDeformationProtection();
			}
			
			base.Start();
		}

		private void findModelWithoutDeformationProtection()
		{
			model = transform.gameObject;
			//for (int i = 0; i < transform.childCount; i++)
			//{
			//	if (transform.GetChild(i).name.Contains("Mesh"))
			//	{
			//		model = transform.GetChild(i).gameObject;
			//	}
			//}
		}

		private void findModelWithDeformationProtection()
		{
			model = transform.gameObject;
		}

		private float time;
		protected void flip(float speed)
		{
			if (model.transform.eulerAngles != vc.RotationLeft && speed > 0f)
			{
				model.transform.eulerAngles = vc.RotationLeft;
				aabb3D.UpdateAABB(rotationBox, transform.localRotation.QuaternionTo3x3(),
					transform.position, ref boundingBox);
			}
			else if (model.transform.eulerAngles != vc.RotationRight && speed < 0f)
			{
				model.transform.eulerAngles = vc.RotationRight;
				aabb3D.UpdateAABB(rotationBox, transform.localRotation.QuaternionTo3x3(),
					transform.position, ref boundingBox);
			}
				
		}

		
	}
}
