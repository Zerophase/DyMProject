using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.ObjectManipulation.Interfaces
{
	public interface ICardinalMovement
	{
	    Vector3 CalculateTotalMovement(float direction, Vector3 xVelocity, bool isJumping, Vector3 currentPosition);
		Vector3 Move(float stickInput, Vector3 acceleration, float deltaTime);
		Vector3 Jump(bool pressed, Vector3 currentPosition);
		Vector3 Gravity { set; }
		bool HasJumped { get; set; }
	}
}
