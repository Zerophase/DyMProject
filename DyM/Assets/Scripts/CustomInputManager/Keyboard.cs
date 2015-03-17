using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using Assets.Scripts.CustomInputManager.Interfaces;
using Assets.Scripts.Utilities;
using UnityEngine;

namespace Assets.Scripts.CustomInputManager
{
	public class Keyboard : IInput
	{

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
			if (Input.GetButton("KeyboardLeftMovement"))
				return -1f;
			else if (Input.GetButton("KeyboardRightMovement"))
				return 1f;
			else
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
			return Input.GetButton("MouseFire");
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
			return Input.GetButtonDown("SwitchWeaponKeyboard");
		}

		public float CameraHorizontalMovement()
		{
            //if (!Util.compareEachFloat(mousePosX, Input.mousePosition.x))
            //{
            //    mouseX = -Input.GetAxis("Mouse X");
            //    mousePosX = Input.mousePosition.x;
            //}
            //return -mouseX;
            
		    return Input.GetAxis("Mouse X");
		}

		public float CameraVerticalMovement()
		{
            //if (!Util.compareEachFloat(mousePosY, Input.mousePosition.y))
            //{
            //    mouseY = -Input.GetAxis("Mouse Y");
            //    mousePosY = Input.mousePosition.y;
            //}
            //return mouseY;

		    return Input.GetAxis("Mouse Y");
		}

        Vector2 aimVector = Vector2.zero;
	    public Vector2 Aim()
	    {
	        aimVector = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            aimVector.x -= 0.5f;
            aimVector.y -= 0.5f;
            aimVector.y = -aimVector.y;

            return aimVector;
	    }
	}
}
