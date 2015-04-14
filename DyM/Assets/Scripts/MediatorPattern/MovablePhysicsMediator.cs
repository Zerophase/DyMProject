using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Text;
using Assets.Scripts.CollisionBoxes.ThreeD;
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

		public bool HasJumped { set { cardinalMovement.HasJumped = value; } }

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

		Vector3 rotationRight = new Vector3(0f, 270f, 0f);
		Vector3 rotationLeft = new Vector3(0f, 90f, 0f);
		private float time;
		protected void flip(float speed)
		{
			if (model.transform.eulerAngles != rotationLeft && speed > 0f)
			{
				model.transform.eulerAngles = rotationLeft;
			}
			else if (model.transform.eulerAngles != rotationRight && speed < 0f)
				model.transform.eulerAngles = rotationRight;
		}
	}
}
