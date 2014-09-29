using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.ObjectManipulation.Interfaces
{
	public interface ICardinalMovement
	{
		Vector3 Move(float stickInput, Vector3 acceleration, float time);
		Vector3 Jump(float pos, float playerPos);
		bool Falling { get; }
	}
}
