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

		private Vector3 jumpPosition;
		private float prevPosition;
		private float startJump;
		private float jumpHeight = 5f;
		private bool falling;
		public bool Falling
		{
			get { return falling; }
		}

		private float acceleration;
        public float Acceleration
        {
            get { return acceleration; }
        }
		private float timer;
		private float speed = 0.2f;
		private float maxSpeed = .75f;
		private float prevPos;

		public CardinalMovement()
		{
			startJump = 0f;
		}

		public Vector3 Move(float pos, float time)
		{
			resetMovementModifiers(pos);
			addAccellerationToSpeed(pos, time);
			
			setSpeed();
			directionMoved(pos);

			return position;
		}

		private void directionMoved(float pos)
		{
			if (pos > 0f)
			{
				position = new Vector3(speed, 0f, 0f);
			}
			else if (pos < 0f)
			{
				position = new Vector3(-speed, 0f, 0f);
			}
			else if (pos == 0f)
			{
				position = Vector3.zero;
			}
		}

		private void setSpeed()
		{
			speed = Math.Min(speed, maxSpeed);
		}

		private void addAccellerationToSpeed(float pos, float time)
		{
			if (Math.Abs(pos) > 0f)
				speed += accelerate(time);
		}

		private float accelerate(float time)
		{
			timer += time;
			if (timer > .5f)
			{
				acceleration = .15f;
				timer = 0f;
			}
			else if (acceleration != 0f)
			{
				acceleration = 0f;
			}

			return acceleration;
		}

		private void resetMovementModifiers(float pos)
		{
			if (pos > prevPos || pos < prevPos)
			{
				acceleration = 0f;
				speed = .2f;
			}

			setPrevPos(pos);
		}

		private void setPrevPos(float pos)
		{
			prevPos = pos;
		}

		public Vector3 Jump(float pos, float playerPos)
		{
			setFallingTrue(playerPos);
			
			allowJump(playerPos);

			setJumpVelocity(pos, playerPos);

			updatePreviousPlayerPosition(playerPos);

			return jumpPosition;
		}

		private void updatePreviousPlayerPosition(float playerPos)
		{
			prevPosition = playerPos;
		}

		private void setJumpVelocity(float pos, float playerPos)
		{
			if (pos > 0f && playerPos < startJump + jumpHeight && !falling)
			{
				jumpPosition += new Vector3(0f, .5f, 0f);
			}
			else
			{
				jumpPosition = Vector3.zero;
			}
		}

		private void allowJump(float playerPos)
		{
			if (Util.compareEachFloat(prevPosition, playerPos))
			{
				normalizeJumpStartPosition(playerPos);
				falling = false;
			}
		}

		private void normalizeJumpStartPosition(float playerPos)
		{
			startJump = playerPos;
		}

		private void setFallingTrue(float playerPos)
		{
			if (playerPos >= startJump + jumpHeight)
			{
				// Use to debug player reaching correct max height when rapid key presses occur.
				//if(!falling)
				//	Debug.Log("player position: " + playerPos);
				falling = true;
			}
		}
	}
}
