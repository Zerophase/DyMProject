using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Utilities
{
	public static class PhysicsFuncts 
	{
		public static Vector3 calculateVelocity(Vector3 currentAcceleration, float time)
		{
			return currentAcceleration * time;
		}
		
		public static Vector3 calculatePosition(Vector3 currentVelocity, float time)
		{
			return currentVelocity * time;
		}

	    public static Vector3 calculateJumpingVelocity(Vector3 jumpVelocity, Vector3 gravity, float time)
	    {
	        return new Vector3();
	    }
	}
}

