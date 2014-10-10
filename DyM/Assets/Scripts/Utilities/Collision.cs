using UnityEngine;
using System.Collections;
namespace Assets.Scripts.Utilities
{
	public static class Collision
	{
		static Vector3EqualityComparerWithTolerance tolVec = new Vector3EqualityComparerWithTolerance ();
		public static bool Collided(Transform object1, Transform object2)
		{
			if(Mathf.Abs(object1.collider.bounds.size.x - object2.collider.bounds.size.x) > 
			   (object1.collider.bounds.size.x + object2.collider.bounds.size.x)) return false;
			if(Mathf.Abs(object1.collider.bounds.size.y - object2.collider.bounds.size.y) > 
			   (object1.collider.bounds.size.y + object2.collider.bounds.size.y)) return false;
			if(Mathf.Abs(object1.collider.bounds.size.z - object2.collider.bounds.size.z) > 
			   (object1.collider.bounds.size.z + object2.collider.bounds.size.z)) return false;

			return true;

		}
	}
}