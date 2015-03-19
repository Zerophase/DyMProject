using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Abilities;
using UnityEngine;

namespace Assets.Scripts.CustomInputManager.Interfaces
{
	public interface IInput
	{
		bool CheckDodgeKeys();
		bool PlaneShiftDown();
		bool PlaneShiftUp();
		float MovementHorizontal();
		bool Jumping();
		bool Jump();
		bool Fire();
		bool WeakAttack();
		AbilityTypes? ActivateAbility();
		bool SwitchWeapon();
        Vector2 Aim();
	}
}
