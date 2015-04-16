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
			CRESTING,
			DROPPING,
			LANDING
		}

	    private Vector3 velocity;
	    private Vector3 jumpVelocity;

		private float prevPosition;
		
		private float timeAtJump;
		private float timeSinceJump;
		private float maxUpwardsJumpTime;
		
		private float timeToStartDropping;

		private Vector3 jumpHeight;

        private bool hasJumped;
		public bool HasJumped { get { return hasJumped; } set { hasJumped = value; } }

		private delegate Vector3 JumpAnimations();
		
		private JumpAnimations[] jumpAnimations = new JumpAnimations[5];

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
						jumpAnimations[i] = Cresting;
						break;
					case 3:
						jumpAnimations[i] = Dropping;
						break;
					case 4:
						jumpAnimations[i] = Landing;
						break;
				}
			}
		}

	    public Vector3 CalculateTotalMovement(float direction, Vector3 xVelocity, bool isJumping, Vector3 currentPosition)
	    {
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

		private float delta;
		private bool jumpFromPlatform;
		private Vector3 fallVelocity;

		private Vector3 OnPress()
		{
			timeAtJump = Time.time;
			timeSinceJump = 0f;
			maxUpwardsJumpTime = 1f;
			timeToStartDropping = maxUpwardsJumpTime + 1f;
			jumpHeight = Vector3.zero;
			jumpVelocity = new Vector3(0f, 40f, 0f);
			fallVelocity = new Vector3(0f, -20f, 0f);
			jumpFromPlatform = true;
			hasJumped = true;
			onGround = false;

			delta = 1f;

			return jumpHeight ;
		}

		private Vector3 Rising()
		{
			calculateTimeSinceJump();
			Vector3 rise = jumpVelocity;

			return rise;
		}

		private Vector3 Cresting()
		{
			calculateTimeSinceJump();
			Vector3 crest =  delta * jumpVelocity + (1 - delta) * fallVelocity;
			delta -= Time.deltaTime;
			return crest;
		}

		private void calculateTimeSinceJump()
		{
			timeSinceJump = Time.time - timeAtJump;
		}

		private Vector3 Dropping()
		{
            //var temp = currentPositionCopy + (-jumpVelocity - currentPositionCopy) * timeSinceJump;
			if (jumpFromPlatform)
				jumpHeight = 0.51f*jumpVelocity; //+ (0.05f) * jumpHeight;
            else
            {
                //Debug.Log("Drop from platform time: " + timeSinceDropping);
                jumpHeight = 0.51f * jumpVelocity;
            }

            return jumpHeight;
		}

		private Vector3 Landing()
		{
			jumpHeight = Vector3.zero;
			jumpFromPlatform = false;
			jumpVelocity = Vector3.zero;
			hasJumped = false;
			released = false;
			return jumpVelocity;
		}

        private Vector3 currentPositionCopy;
		private bool onGround;
		private Vector3 previousJumpPos;
		private int frameSame;
		public Vector3 Jump(bool pressed, Vector3 currentPosition)
		{
            if(!hasJumped)
                currentPositionCopy = new Vector3(0.0f,currentPosition.y);
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
			else if (pressed && !released && timeSinceJump <= maxUpwardsJumpTime)
			{
				//Debug.Log(JumpComparison.RISING);
				returnValue = (int)JumpComparison.RISING;
			}
			else if (HasJumped && timeSinceJump >= maxUpwardsJumpTime &&
				timeToStartDropping > timeSinceJump)
			{
				Debug.Log(JumpComparison.CRESTING);
				returnValue = (int)JumpComparison.CRESTING;
			}
			else if (onGround)
			{
				returnValue = (int)JumpComparison.LANDING;
			}
			else if (!pressed && hasJumped || timeToStartDropping < timeSinceJump)
			{
				//Debug.Log(JumpComparison.DROPPING);
				released = true;
				returnValue = (int) JumpComparison.DROPPING;
			}

			savedPress = pressed;
			return returnValue;
		}
	}
}
