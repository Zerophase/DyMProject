using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.ObjectManipulation.Interfaces;
using Assets.Scripts.Utilities;
using UnityEngine;

namespace Assets.Scripts.ObjectManipulation
{
	public class CardinalMovement : ICardinalMovement
	{
		private Vector3 position;
	    private Vector3 velocity;
	    private Vector3 acceleration;

		private Vector3 jumpPosition;
		private float prevPosition;
		private float startJump;
		private float jumpHeight = 5f;
		private bool falling;
		public bool Falling
		{
			get { return falling; }
		}
		private float timer;
		private float speed = 0.2f;
		private float maxSpeed = .75f;
		private float prevPos;

		public CardinalMovement()
		{
			startJump = 0f;
		}

		public Vector3 Move(float stickInput, Vector3 acceleration, float time)
		{
		    velocity = Physics.calculateVelocity(acceleration,stickInput);
			return velocity;
		}

		public Vector3 Jump(float pos, float playerPos)
		{
			return jumpPosition;
		}
		
	}
}
