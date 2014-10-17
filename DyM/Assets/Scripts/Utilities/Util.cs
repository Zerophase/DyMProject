using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Schema;
using UnityEngine;

namespace Assets.Scripts.Utilities
{
	public static class Util
	{
		public static bool CompareVectors(this Vector2 v1, Vector2 v2)
		{
			return compareEachFloat(v1.x, v2.x) && compareEachFloat(v1.y, v2.y);
		}

		public static bool compareEachFloat(float a, float b)
		{
			float absA = Math.Abs(a);
			float absB = Math.Abs(b);
			float diff = Math.Abs(a - b);

			if (a == b)
			{ // shortcut, handles infinities
				return true;
			}
			else if (a == 0 || b == 0 || diff < float.MinValue)
			{
				// a or b is zero or both are extremely close to it
				// relative error is less meaningful here
				return diff < (float.Epsilon * float.MinValue);
			}
			else
			{ // use relative error
				return diff / (absA + absB) < float.Epsilon;
			}
		}

		public static T Clamp<T>(this T val, T min, T max) where T : IComparable<T>
		{
			if (val.CompareTo(min) < 0)
				return min;
			else if (val.CompareTo(max) > 0)
				return max;
			else
			{
				return val;
			}
		}
		
		
		private static bool collided;
		private static Vector3 maxDistance;
		public static bool CheckBounds(this Collider collider, Collider otherCollider)
		{
			collided = false;
			
			maxDistance = collider.bounds.center + otherCollider.bounds.center;
			
			if (maxDistance.x > Mathf.Abs(collider.bounds.size.x - otherCollider.bounds.size.x)) 
				collided = true;
			if(maxDistance.y > Mathf.Abs(collider.bounds.size.y - otherCollider.bounds.size.y))
				collided = true;
			if(maxDistance.z > Mathf.Abs(collider.bounds.size.z - otherCollider.bounds.size.z))
				collided = true;
			
			return collided;
		}
	}

}
