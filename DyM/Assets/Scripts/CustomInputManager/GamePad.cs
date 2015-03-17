using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.CustomInputManager.Interfaces;
using Assets.Scripts.Utilities;
using UnityEngine;

namespace Assets.Scripts.CustomInputManager
{
	public class GamePad : IInput
	{
		private bool pressed;
		private float savedPress;

		public bool CheckDodgeKeys()
		{
			return (Input.GetButton("PlaneShiftDown") || Input.GetButton("PlaneShiftUp"));
		}

		public bool PlaneShiftDown()
		{
			return Input.GetButtonDown("PlaneShiftDown");
		}

		public bool PlaneShiftUp()
		{
			return Input.GetButtonDown("PlaneShiftUp");
		}

		public float MovementHorizontal()
		{
			return Input.GetAxis("Horizontal");
		}

		public bool Jumping()
		{
			return Input.GetButton("Jump");
		}

		public bool Jump()
		{
			return Input.GetButtonDown("Jump");
		}

		public bool Fire()
		{
			if (Input.GetAxis("Fire1") > 0f)
				return true;
			else
				return false;
		}

		public bool WeakAttack()
		{
			return Input.GetButtonDown("WeakAttack");
		}

		public bool ActivateAbility()
		{
			return Input.GetButtonDown("ActivateAbility");
		}

		public bool SwitchWeapon()
		{
			return pressAxisDown();
		}

		private  bool pressAxisDown()
		{
			savedPress = Input.GetAxis("SwitchWeapon");
			if (!pressed && (savedPress > 0.0f || savedPress < 0.0f))
			{
				pressed = true;
				return true;
			}
			else if (pressed && Util.compareEachFloat(savedPress, 0.0f))
			{
				pressed = false;
			}

			return false;
		}

		public float CameraHorizontalMovement()
		{
			return Input.GetAxis("CameraHorizontalMovement");
		}

		public float CameraVerticalMovement()
		{
			return Input.GetAxis("CameraVerticalMovement");
		}

        Vector2 aimVector = Vector2.zero;
	    public Vector2 Aim()
	    {
            aimVector.x = Input.GetAxis("CameraHorizontalMovement");
            aimVector.y = Input.GetAxis("CameraVerticalMovement");
            return aimVector;
	    }
	}
}
