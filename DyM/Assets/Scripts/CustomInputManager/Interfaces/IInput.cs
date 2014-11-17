using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.CustomInputManager.Interfaces
{
	public interface IInput
	{
		bool CheckDodgeKeys();
		bool PlaneShiftDown();
		bool PlaneShiftUp();
		float MovementHorizontal();
		bool Jump();
		bool Fire();
		bool WeakAttack();
		bool ActivateAbility();
		bool SwitchWeapon();
		float CameraHorizontalMovement();
		float CameraVerticalMovement();
	}
}
