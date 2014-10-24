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

		public static bool CheckBounds(this GameObject ground, GameObject player)
		{
            
			collided = false;
			Box3D aBox = ground.BoxToRect();
			Box3D bBox = player.BoxToRect();

			return Intersect(aBox, bBox);
		}

		private static bool Intersect(Box3D ground, Box3D player)
		{	
            bool comp1 = ground.xMin < player.xMax;
			bool comp2 = ground.xMax > player.xMin;

			bool comp3 = ground.yMin > player.yMax;
			bool comp4 = ground.yMax < player.yMin;

			bool comp5 = ground.zMin < player.zMax;
			bool comp6 = ground.zMax > player.zMin;

			return comp1 && comp2 && comp3 && comp4 && comp5 && comp6;
		}

		private static Box3D BoxToRect(this GameObject a)
		{
			BoxCollider aCollider = a.GetComponent<BoxCollider>();

			Vector3 aPos = a.transform.position;
			
			aPos += aCollider.center;
			aPos.x -= (aCollider.size.x * a.transform.lossyScale.x) / 2;
			aPos.y += (aCollider.size.y * a.transform.lossyScale.y) /2;
		    aPos.z -= (aCollider.size.z * a.transform.lossyScale.z) /2;

			return new Box3D(aPos.x, aPos.y, aPos.z, aCollider.size.x * a.transform.lossyScale.x, -aCollider.size.y * a.transform.lossyScale.y, aCollider.size.z * a.transform.lossyScale.z);
		}

		private static Vector2 v2(this Vector3 v)
		{
			return	 new Vector2(v.x, v.y);
		}

		public static Vector3 CheckForOverlap(this Vector3 position, Collider collider)
		{
			return position;
		}
	}

}
