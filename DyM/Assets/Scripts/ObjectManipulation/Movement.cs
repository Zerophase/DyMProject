﻿using System;
using Assets.Scripts.DependencyInjection;
using Assets.Scripts.ObjectManipulation.Interfaces;
using ModestTree.Zenject;
using UnityEngine;

namespace Assets.Scripts.ObjectManipulation
{
	public class Movement : IMovement
	{
		private IPlaneShift planeShift;
		[Inject]
		private ICardinalMovement cardinalMovement;

		
		public Movement(IPlaneShift planeShift, ICardinalMovement cardinalMovement)
		{
			createPlaneShift(planeShift);
			createCardinalMovement(cardinalMovement);
		}

		
		public Movement()
		{
			//createPlaneShift(new PlaneShift());
			//createCardinalMovement(new CardinalMovement());
		}

		public Movement(Vector3 position)
		{
			planeShift = new PlaneShift(position, PlanePosition.DOWN);
			createCardinalMovement(new CardinalMovement());
		}

		private void createCardinalMovement(ICardinalMovement cardinalMovement)
		{
			this.cardinalMovement = cardinalMovement;
		}

		private void createPlaneShift(IPlaneShift planeShift)
		{
			this.planeShift = planeShift;
		}

		public Vector3 Move(float pos, float time)
		{
			return cardinalMovement.Move(pos, time);
		}

		public Vector3 Jump(float pos, float playerPos)
		{
			return cardinalMovement.Jump(pos, playerPos);
		}

		public Vector3 ShiftPlane(KeyCode activatePlaneShift, Vector3 currentPosition)
		{
			return planeShift.ShiftPlane(activatePlaneShift, currentPosition);
		}
		public Vector3 Dodge(Vector3 currentPlane, bool keyIsPressed, float timing)
		{
			return planeShift.Dodge(currentPlane, keyIsPressed, timing);
		}
	}
}
