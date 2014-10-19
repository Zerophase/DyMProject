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
		public static bool CheckBounds(this GameObject ground, GameObject player)
		{
			collided = false;
			Rect aBox = ground.BoxToRect();
			Rect bBox = player.BoxToRect();

			return Intersect(aBox, bBox);
		}

		private static bool Intersect(Rect ground, Rect player)
		{
			bool comp1 = ground.yMin > player.yMax;
			bool comp2 = ground.yMax < player.yMin;
			bool comp3 = ground.xMin < player.xMax;
			bool comp4 = ground.xMax > player.xMin;

			return comp1 && comp2 && comp3 && comp4;
		}

		private static Rect BoxToRect(this GameObject a)
		{
			BoxCollider2D aCollider = a.GetComponent<BoxCollider2D>();

			Vector2 aPos = a.transform.position.v2();

			aPos += aCollider.center;
			aPos.x -= aCollider.size.x/2;
			aPos.y += aCollider.size.y/2;

			return new Rect(aPos.x, aPos.y, aCollider.size.x, -aCollider.size.y);
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
