using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.ObjectManipulation.Interfaces
{
	public interface IPlaneShift
	{
		Vector3 ShiftPlane(KeyCode activatePlaneShift, Vector3 currentPosition);
		Vector3 Dodge(Vector3 currentPlane, bool keyIsPressed, float timing);
	}
}
