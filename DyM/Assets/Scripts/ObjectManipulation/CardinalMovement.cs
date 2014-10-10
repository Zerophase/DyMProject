using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.ObjectManipulation.Interfaces;
using Assets.Scripts.Utilities;
using UnityEngine;
using Physics = UnityEngine.Physics;

namespace Assets.Scripts.ObjectManipulation
{
	public class CardinalMovement : ICardinalMovement
	{
		private Vector3 position;
	    private Vector3 velocity;
	    private Vector3 acceleration;

	    private Vector3 jumpVelocity;
	    private Vector3 jumpAcceleration = new Vector3(0f, .3f, 0f);

		private Vector3 gravity;
		public Vector3 Gravity { set { gravity = value; } }

		private Vector3 jumpPosition;
		private float prevPosition;
		private float startJump;
		private float jumpHeight = 5f;

		private bool falling;
		public bool Falling
		{
			get { return falling; }
		}

		public CardinalMovement()
		{
		}

		public Vector3 Move(float stickInput, Vector3 acceleration, float time)
		{
		    velocity = PhysicsFuncts.calculateVelocity(acceleration, time) * stickInput;
		    
			return velocity;
		}

		public Vector3 Jump(bool pressed, float playerPos)
		{
			if(pressed)
			{
				jumpVelocity = new Vector3(0f,.5f,0f);
				jumpVelocity += PhysicsFuncts.calculateVelocity(gravity, Time.deltaTime);
				
			}
			else
			{
				jumpVelocity = Vector3.zero;
			}
			return jumpVelocity;
		}
		
	}
}
