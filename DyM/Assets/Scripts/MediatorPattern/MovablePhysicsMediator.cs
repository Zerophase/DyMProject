using vc = Assets.Scripts.Utilities.Constants.VectorConstants;
using Assets.Scripts.ObjectManipulation.Interfaces;
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

		protected override void Start()
		{
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
			for (int i = 0; i < transform.childCount; i++)
			{
				if (transform.GetChild(i).name.Contains("Mesh"))
				{
					model = transform.GetChild(i).gameObject;
				}
			}
		}

		private void findModelWithDeformationProtection()
		{
			for (int i = 0; i < transform.GetChild(preventDeformation).childCount; i++)
			{
				if (transform.GetChild(preventDeformation).GetChild(i).name.Contains("Mesh"))
				{
					model = transform.GetChild(preventDeformation).GetChild(i).gameObject;
				}
			}
		}

		private float time;
		protected void flip(float speed)
		{
			if (model.transform.eulerAngles != vc.RotationLeft && speed > 0f)
			{
				model.transform.eulerAngles = vc.RotationLeft;
			}
			else if (model.transform.eulerAngles != vc.RotationRight && speed < 0f)
				model.transform.eulerAngles = vc.RotationRight;
		}

		
	}
}
