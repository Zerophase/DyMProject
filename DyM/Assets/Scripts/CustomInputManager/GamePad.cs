using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Abilities;
using Assets.Scripts.CustomInputManager.Interfaces;
using Assets.Scripts.Utilities;
using UnityEngine;

namespace Assets.Scripts.CustomInputManager
{
	public class GamePad : IInput
	{
		private bool pressed;
		private float savedPress;
        private float deadZone = .4f;

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
            if (Input.GetAxis("Horizontal") > deadZone)
            {
                return Input.GetAxis("Horizontal");
            }
            else if (Input.GetAxis("Horizontal") < -deadZone)
            {
                return Input.GetAxis("Horizontal");
            }
            return 0f;
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

		public AbilityTypes ActivateAbility()
		{
			if (Input.GetButtonDown("ActivateAbility"))
				return  AbilityTypes.SLOW_TIME;
			else if(Input.GetButtonDown("Sprint"))
			{
				return AbilityTypes.SPEED_UP_TIME;
			}

			return AbilityTypes.NONE;
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

        Vector2 aimVector = Vector2.zero;
        Vector2 prevPosition = Vector2.zero;

	    public Vector2 Aim()
	    {
            prevPosition = aimVector;

            aimVector.x = Input.GetAxis("CameraHorizontalMovement");
            aimVector.y = Input.GetAxis("CameraVerticalMovement");

            if (aimVector.magnitude < deadZone)
            {
                aimVector = prevPosition;
            }
            else
            {
                aimVector = aimVector.normalized * ((aimVector.magnitude - deadZone) / (1 - deadZone));
            }
            return aimVector;
	    }
	}
}
