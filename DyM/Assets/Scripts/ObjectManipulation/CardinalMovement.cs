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
		private Vector3 previousJumpPos;
		private Vector3 fallVelocity;

		private float timeAtJump;
		private float timeSinceJump;
		private float maxUpwardsJumpTime;
		private float timeToStartDropping;

		private float delta;

		private uint returnValue;
		private uint frameSame;

		private bool onGround;
		private bool hasJumped;
		private bool savedPress;
		private bool released;

		private delegate Vector3 JumpAnimations();
		
		private readonly JumpAnimations[] jumpAnimations = new JumpAnimations[5];

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
			return velocity;
		}

		private Vector3 OnPress()
		{
			timeAtJump = Time.time;
			timeSinceJump = 0f;
			maxUpwardsJumpTime = 0.4f;
			timeToStartDropping = maxUpwardsJumpTime + 0.5f;
			jumpVelocity = new Vector3(0f, 60f, 0f);
			fallVelocity = new Vector3(0f, -20f, 0f);
			hasJumped = true;
			onGround = false;

			delta = 1f;

			return Vector3.zero;
		}

		private Vector3 Rising()
		{
			calculateTimeSinceJump();
			return jumpVelocity;
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
			return Vector3.zero;
		}

		private Vector3 Landing()
		{
			jumpVelocity = Vector3.zero;
			hasJumped = false;
			released = false;
			return jumpVelocity;
		}

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

		private uint jumpComparision(bool pressed)
		{
			if (pressed && !savedPress && !hasJumped)
			{
				returnValue = (int)JumpComparison.ON_Press;
			}
			else if (pressed && !released && timeSinceJump <= maxUpwardsJumpTime)
			{
				returnValue = (int)JumpComparison.RISING;
			}
			else if (hasJumped && timeSinceJump >= maxUpwardsJumpTime &&
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
				released = true;
				returnValue = (int) JumpComparison.DROPPING;
			}

			savedPress = pressed;
			return returnValue;
		}
	}
}
