using UnityEngine;

namespace Assets.Scripts.ObjectManipulation
{
	public class JumpPrototype
	{
		

		private enum LadderState
		{
			off
		}

		private enum JumpState
		{
			grounded,
			impulse,
			falling,
			cresting,
			terminal
		}

		private enum InpuRef
		{
			jumpDown
		}

		private JumpState jumpState;
		private LadderState ladderState;
		private InpuRef inputRef;
		float impulseTime;
		float jumpSpeed;
		float impulseLength;
		float gravity;
		float fallSpeed;
		bool touchingGround;
		bool touchingCeiling;
		bool playerControl;

		private Vector3 velocity;

		private void JumpBehavior()
		{
			/* * JUMP * */

			if (jumpState == JumpState.grounded)
			{
				//On the ground
				if ((playerControl == true)) //&& (inputRef.jumpDown == true))
				{
					jumpState = JumpState.impulse;
					impulseTime = 0f;
				}
				if (touchingGround == false)
				{
					jumpState = JumpState.falling;
				}
			}
			if (jumpState == JumpState.impulse)
			{
				//move upward while button is held down, up to max impulse time
				velocity = new Vector3(velocity.x, jumpSpeed, 0f);
				impulseTime += Time.deltaTime;
				if ( /*(inputRef.jumpPressed == false) ||*/ (impulseTime >= impulseLength))
				{
					jumpState = JumpState.cresting;
				}
				if (touchingCeiling == true)
				{
					//hit ceiling
					velocity = new Vector3(velocity.x, 0f, 0f);
					jumpState = JumpState.falling;
				}
			}
			if (jumpState == JumpState.cresting)
			{
				//gravity is slowing the player's ascent
				velocity = new Vector3(velocity.x, velocity.y - gravity * Time.deltaTime, 0f);
				if (velocity.y < 0f)
				{
					jumpState = JumpState.falling;
				}
				if (touchingCeiling == true)
				{
					//hit ceiling
					velocity = new Vector3(velocity.x, 0f, 0f);
					jumpState = JumpState.falling;
				}
			}
			if (jumpState == JumpState.falling)
			{
				//gravity is pulling the player downwards
				velocity = new Vector3(velocity.x, velocity.y - gravity * Time.deltaTime, 0f);
				if (velocity.y <= -fallSpeed)
				{
					jumpState = JumpState.terminal;
				}
				if ((touchingGround == true) && (velocity.y < 0f))
				{
					velocity = new Vector3(velocity.x, 0f, 0f);
					jumpState = JumpState.grounded;
				}
			}
			if (jumpState == JumpState.terminal)
			{
				//terminal velocity
				velocity = new Vector3(velocity.x, -fallSpeed, 0f);
				if (touchingGround == true)
				{
					velocity = new Vector3(velocity.x, 0f, 0f);
					jumpState = JumpState.grounded;
				}
			}
		}
	}
}