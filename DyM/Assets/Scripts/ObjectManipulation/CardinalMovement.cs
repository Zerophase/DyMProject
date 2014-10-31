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

		private bool falling;
		public bool Falling
		{
			get { return falling; }
		}

		public CardinalMovement()
		{
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

		public Vector3 Jump(bool pressed, float distanceJumped)
		{
            
			if(Input.GetButtonDown("Jump"))
			{
			    jumpTimer += Time.deltaTime;
			    jumpVelocity = new Vector3(0f, 2f, 0f);
			}
            else if (Input.GetButton("Jump") && jumpVelocity.y > 0f)
            {
                jumpTimer += Time.deltaTime;
                jumpVelocity = (.5f*gravity*Mathf.Pow(jumpTimer,2)) + jumpVelocity;
            }
            else if (Input.GetButtonUp("Jump"))
            {
                jumpTimer = 0f;
                jumpVelocity = gravity*jumpTimer;
            }
            else
            {
                jumpTimer += Time.deltaTime;
                jumpVelocity = gravity*jumpTimer;
            }
			return jumpVelocity;
		}
		
	}
}
