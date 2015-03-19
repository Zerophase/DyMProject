using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.ObjectManipulation.Interfaces;
using Assets.Scripts.Utilities;
using Assets.Scripts.GameObjects;
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

	    private Vector3 velocity;
	    private Vector3 jumpVelocity;

		private Vector3 gravity;
		public Vector3 Gravity { set { gravity = value; } }

		private float prevPosition;
		
		private float timeAtJump;
		private float timeSinceJump;
		private float timeAtDrop;
		private float timeSinceDropping;

		private Vector3 jumpHeight;
		private Vector3 positionPassedIn;

        private bool hasJumped;
		public bool HasJumped { get { return hasJumped; } set { hasJumped = value; } }

		private delegate Vector3 JumpAnimations();
		
		private JumpAnimations[] jumpAnimations = new JumpAnimations[4];

		private Player player;

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
		    positionPassedIn = currentPosition;
	        Vector3 total = Vector3.zero;
	        total += Move(direction, xVelocity, Time.deltaTime);
	        total += Jump(isJumping, currentPosition);
		    return total;
	    }
		public Vector3 Move(float direction, Vector3 acceleration, float deltaTime)
		{
		    velocity = acceleration * direction;
            //// TODO put in Time System
            //if (Input.GetButton("Sprint"))
            //{
            //    velocity+= new Vector3(20f, 0f, 0f) * direction;
            //}
			return velocity;
		}

		private bool jumpFromPlatform;
		private Vector3 OnPress()
		{
			timeAtJump = Time.time;
			jumpHeight = Vector3.zero;
			jumpVelocity = new Vector3(0f, 10f, 0f);
			timeAtDrop = 0f;
			jumpFromPlatform = true;
			hasJumped = true;
			onGround = false;
			
			return jumpHeight ;
		}

		private Vector3 Rising()
		{
			calculateTimeSinceJump();

			
			//Debug.Log("Time Since Jump " + timeSinceJump);
			if(timeSinceJump < .5f)
			{
				//Debug.Log("Rise");
				jumpHeight = .9f *jumpVelocity + (.9f)*jumpHeight;
			}
			else
			{
				//Debug.Log("Fall");
				jumpHeight = 0.11f * jumpVelocity + ( 0.05f) * jumpHeight;

			}

			//Debug.Log("Jump Height: " + jumpHeight);
			return jumpHeight;
		}

		private void calculateTimeSinceJump()
		{
			timeSinceJump = Time.time - timeAtJump;
		}

		
		private Vector3 Dropping()
		{
			if (jumpFromPlatform)
				jumpHeight = 0.51f * jumpVelocity + (0.05f) * jumpHeight;
			else
			{
				if (Util.compareEachFloat(timeAtDrop, 0f))
					timeAtDrop = Time.time;

				timeSinceDropping = Time.time - timeAtDrop;
				//Debug.Log("Drop from platform time: " + timeSinceDropping);
				jumpHeight = 0.51f * jumpVelocity + (0.05f) * jumpHeight;
			}
				
			return jumpHeight;
		}

		private Vector3 Landing()
		{
			timeAtDrop = 0f;
			jumpHeight = Vector3.zero;
			jumpFromPlatform = false;
			timeSinceDropping = 0f;
			jumpVelocity = Vector3.zero;
			hasJumped = false;
			released = false;
			return jumpVelocity;
		}

		private bool onGround;
		private Vector3 previousJumpPos;
		private int frameSame;
		public Vector3 Jump(bool pressed, Vector3 currentPosition)
		{
			if (frameSame == 5)
			{
				frameSame = 0;
				onGround = true;
			}
			else if (Util.compareEachFloat(currentPosition.y, previousJumpPos.y) && onGround == false)
			{
				frameSame++;
			}

			previousJumpPos = currentPosition;
			return jumpAnimations[jumpComparision(pressed)].Invoke();
		}

		private int returnValue;
		private bool savedPress;
		private float howLongJumpIs;
		private bool released;
		private int jumpComparision(bool pressed)
		{
			if (pressed && !savedPress && !hasJumped)
			{

				howLongJumpIs = 0f;
				//Debug.Log("Key Pressed ");
				returnValue = (int)JumpComparison.ON_Press;
			}
				
			else if (pressed && !released)
			{

				returnValue = (int)JumpComparison.RISING;
			}
			else if (onGround)
			{
				returnValue = (int)JumpComparison.LANDING;
			}
			else if (!pressed && hasJumped)
			{

				released = true;
				returnValue = (int) JumpComparison.DROPPING;
			}
			
				
				

			savedPress = pressed;
			return returnValue;
		}
	}
}
