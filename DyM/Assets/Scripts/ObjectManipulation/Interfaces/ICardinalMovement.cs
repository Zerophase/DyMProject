using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.ObjectManipulation.Interfaces
{
	public interface ICardinalMovement
	{
		Vector3 Move(float pos, float time);
		Vector3 Jump(float pos, float playerPos);
        float Acceleration { get; }
		bool Falling { get; }
	}
}
