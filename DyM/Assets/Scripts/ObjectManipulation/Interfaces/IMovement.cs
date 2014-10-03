using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.ObjectManipulation.Interfaces
{
	public interface IMovement
	{
		Vector3 Move(float pos, Vector3 acc, float time);
		Vector3 Jump(bool pressed, float playerPos);
		Vector3 ShiftPlane(KeyCode activatePlaneShift, Vector3 currentPosition);
		Vector3 Dodge(Vector3 currentPlane, bool keyIsPressed, float timing);
	}
}
