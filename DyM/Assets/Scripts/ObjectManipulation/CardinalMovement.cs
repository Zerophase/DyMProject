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
		private enum JumpComparison
		{
			ON_Press = 0,
			RISING,
			DROPPING,
			LANDING
		}
		private Vector3 position;
	    private Vector3 velocity;
	    private Vector3 acceleration;

	    private Vector3 jumpVelocity;
		private Vector3 initialJumpHeight = new Vector3(0f, 1f, 0f);

		private Vector3 gravity;
		public Vector3 Gravity { set { gravity = value; } }

		private Vector3 jumpPosition;
		private float prevPosition;
		
		private float timeAtJump;
		private float timeSinceJump;
		private Vector3 jumpHeight;

        private bool hasJumped;
		public bool HasJumped { set { hasJumped = value; } }

		private delegate Vector3 JumpAnimations(Vector3 currentPosition);
		
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

	    public Vector3 CalculateTotalMovement(float direction, Vector3 xVelocity, bool isJumping, Vector3 currentPosition)
	    {
	        Vector3 total = Vector3.zero;
	        total += Move(direction, xVelocity, Time.deltaTime);
	        total += Jump(isJumping, currentPosition) * Time.deltaTime;
		    return total;
	    }
		public Vector3 Move(float direction, Vector3 acceleration, float deltaTime)
		{
		    velocity = PhysicsFuncts.calculateVelocity(acceleration, deltaTime) * direction;
            //Mapped a sprint button for testing, that kind of works.
            //Worth looking into if we decide to add sprint, very easy.
            //if (Input.GetAxis("Sprint") > .5)
            //{
            //    velocity+= PhysicsFuncts.calculateVelocity(new Vector3(5,0,0), deltaTime) * direction;
            //}
			return velocity;
		}
		private Vector3 OnPress(Vector3 currentPosition)
		{
			jumpPosition = new Vector3(0f, currentPosition.y, 0f);
			maxJumpHeight = Vector3.zero;
			timeAtJump = Time.time;
			jumpHeight = Vector3.zero;
			jumpVelocity = new Vector3(0f, 10f, 0f);
			timeSinceDropping = 0f;
			return jumpHeight ;
		}

		private Vector3 maxJumpHeight;
		private Vector3 Rising(Vector3 currentPosition)
		{
			calculateTimeSinceJump();
			maxJumpHeight = jumpVelocity*timeSinceJump + 0.5f*(gravity / 1.1f)*timeSinceJump * timeSinceJump;
			//if(jumpHeight.y > 22f)
			//	return maxJumpHeight = (jumpVelocity /5f) *timeSinceJump + 0.5f * gravity * timeSinceJump * timeSinceJump;
			
			return maxJumpHeight;
		}

		private void calculateTimeSinceJump()
		{
			timeSinceJump = Time.time - timeAtJump;
		}

		private float timeSinceDropping;
		private Vector3 Dropping(Vector3 currentPosition)
		{
			calculateTimeSinceJump();
			if (Util.compareEachFloat(timeSinceJump, 0f))
			{
				//jumpPosition = currentPosition;
				timeAtJump = Time.time;
			}
			if (Util.compareEachFloat(timeSinceDropping, 0f))
			{
				timeSinceDropping = Time.time;
			}

			jumpHeight = 0.5f * (gravity * 5f) * timeSinceJump * timeSinceJump;
			jumpHeight.y = Math.Max(jumpHeight.y, -15f);

			if(Time.time - timeSinceDropping > 0f)
				jumpHeight = Vector3.Lerp(maxJumpHeight, jumpHeight, Time.time - timeSinceDropping);
			return jumpHeight;
		}

		private Vector3 Landing(Vector3 currentPosition)
		{
			timeSinceDropping = 0f;
			jumpVelocity = Vector3.zero;
			return jumpVelocity;
		}
		
		public Vector3 Jump(bool pressed, Vector3 currentPosition)
		{
			return jumpAnimations[jumpComparision(pressed)].Invoke(currentPosition);
		}

		private int returnValue;
		private bool savedPress;
		private int jumpComparision(bool pressed)
		{
			if (pressed && !savedPress && !hasJumped)
				returnValue = (int)JumpComparison.ON_Press;
			else if (pressed && maxJumpHeight.y >= 0f)
				returnValue = (int)JumpComparison.RISING;
			else if (Util.compareEachFloat(gravity.y, -7f))
				returnValue = (int)JumpComparison.DROPPING;
			else if (Util.compareEachFloat(gravity.y, 0.0f))
				returnValue = (int)JumpComparison.LANDING;
			
			savedPress = pressed;
			return returnValue;
		}
	}
}
