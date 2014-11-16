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

		private Vector3 gravity;
		public Vector3 Gravity { set { gravity = value; } }

		private Vector3 jumpPosition;
		private float prevPosition;
		private float startJump;
		private float jumpHeight = 5f;
	    private float jumpTimer = 0f;
        private bool hasJumped;
		public bool HasJumped { set { hasJumped = value; } }

		private bool falling;
		public bool Falling
		{
			get { return falling; }
		}
		private delegate Vector3 JumpAnimations();
		
		private JumpAnimations[] jumpAnimations = new JumpAnimations[4];

		public CardinalMovement()
		{
			for (int i = 0; i < jumpAnimations.Count(); i++)
			{
				switch (i)
				{
					case 0:
						jumpAnimations[i] = OnPress;
						break;
					case 1:
						jumpAnimations[i] = Rising;
						break;
					case 2:
						jumpAnimations[i] = Dropping;
						break;
					case 3:
						jumpAnimations[i] = Landing;
						break;
				}
			}
		}

	    public Vector3 CalculateTotalMovement(float direction, Vector3 xVelocity, bool isJumping, float distanceJumped)
	    {
	        Vector3 total = Vector3.zero;
	        total += Move(direction, xVelocity, Time.deltaTime);
	        total += Jump(isJumping, distanceJumped);
	        return total;
	    }
		public Vector3 Move(float direction, Vector3 acceleration, float time)
		{
		    velocity = PhysicsFuncts.calculateVelocity(acceleration, time) * direction;
            //Mapped a sprint button for testing, that kind of works.
            //Worth looking into if we decide to add sprint, very easy.
            //if (Input.GetAxis("Sprint") > .5)
            //{
            //    velocity+= PhysicsFuncts.calculateVelocity(new Vector3(5,0,0), time) * direction;
            //}
			return velocity;
		}
		private Vector3 OnPress()
		{
			jumpTimer = 0;
			jumpVelocity = new Vector3(0f, 1.5f, 0f);
			falling = false;
			return jumpVelocity;
		}
		private Vector3 Rising()
		{
			jumpTimer += Time.deltaTime;
			jumpVelocity = (gravity * jumpTimer) + jumpVelocity;
			falling = true;
			return jumpVelocity;
		}

		//private Vector3 Released()
		//{
		//	jumpTimer = 0f;
		//	jumpVelocity = gravity * jumpTimer;
		//	falling = true;
		//	return jumpVelocity;
		//}

		private Vector3 Dropping()
		{
			jumpTimer += Time.deltaTime;
			jumpVelocity = gravity * jumpTimer;
			return jumpVelocity;
		}

		private Vector3 Landing()
		{
			jumpVelocity = Vector3.zero;
			jumpTimer = 0f;
			return jumpVelocity;
		}
		
		public Vector3 Jump(bool pressed, float distanceJumped)
		{
			return jumpAnimations[jumpComparision(pressed)].Invoke();
		}

		private int returnValue;
		private bool savedPress;
		private int jumpComparision(bool pressed)
		{
			if (pressed && !savedPress && !hasJumped)
				returnValue = 0;
			else if (pressed && jumpVelocity.y > 0.0f)
				returnValue = 1;
			else if (Util.compareEachFloat(gravity.y, -1f))
				returnValue = 2;
			else if (Util.compareEachFloat(gravity.y, 0.0f))
				returnValue = 3;
			
			savedPress = pressed;
			return returnValue;
		}
	}
}
