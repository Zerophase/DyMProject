using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Utilities
{
	public static class PhysicsFuncts 
	{
		public static Vector3 calculateVelocity(Vector3 currentAcceleration, float deltaTime)
		{
			return currentAcceleration * deltaTime;
		}

		public static Vector3 calculatePosition(Vector3 currentVelocity, float deltaTime)
		{
			return currentVelocity * deltaTime;
		}

		public static Vector3 calculateJumpingVelocity(Vector3 jumpVelocity, Vector3 gravity, float deltaTime)
	    {
	        return new Vector3();
	    }
	}
}

