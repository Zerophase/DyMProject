using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Abilities;
using Assets.Scripts.CustomInputManager.Interfaces;
using UnityEngine;

namespace Assets.Scripts.CustomInputManager
{
	public static class InputManager
	{
		private static IInput input;

        public static IInput InputType { get { return input; } }
		static InputManager()
		{
			if(Input.GetJoystickNames().Length == 0)
				input = new Keyboard();
			else
				input = new GamePad();
		}

		public static bool CheckDodgeKeys()
		{
			return input.CheckDodgeKeys();
		}

		public static bool PlaneShiftDown()
		{
			return input.PlaneShiftDown();
		}

		public static bool PlaneShiftUp()
		{
			return input.PlaneShiftUp();
		}

		public static float MovementHorizontal()
		{
			return input.MovementHorizontal();
		}

		public static bool Jumping()
		{
			return input.Jumping();
		}

		public static bool Jump()
		{
			return input.Jump();
		}

		public static bool Fire()
		{
			return input.Fire();
		}

		public static bool WeakAttack()
		{
			return input.WeakAttack();
		}

		public static AbilityTypes? ActivateAbility()
		{
			return input.ActivateAbility();
		}

		public static bool SwitchWeapon()
		{
			return input.SwitchWeapon();
		}

	    public static Vector2 Aim()
	    {
	        return input.Aim();
	    }
	}
}
