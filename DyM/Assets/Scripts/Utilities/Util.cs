using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Schema;
using Assets.Scripts.MediatorPattern;
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
		
		public static bool CheckBounds(this PhysicsMediator physicsMediatorOne, PhysicsMediator physicsMediatorTwo)
		{
			return IntersectAABB(physicsMediatorOne.GetBox3D, physicsMediatorTwo.GetBox3D);
		}

		private static Box3D BoxToRect(this GameObject a)
		{
			BoxCollider aCollider = a.GetComponent<BoxCollider>();

			Vector3 aPos = a.transform.position;
			
			aPos += aCollider.center;
			aPos.x -= (aCollider.size.x * a.transform.lossyScale.x) / 2;
			aPos.y += (aCollider.size.y * a.transform.lossyScale.y) /2;
		    aPos.z -= (aCollider.size.z * a.transform.lossyScale.z) /2;

			return new Box3D(aPos.x, aPos.y, aPos.z, aCollider.size.x * a.transform.lossyScale.x, -aCollider.size.y * a.transform.lossyScale.y, aCollider.size.z * a.transform.lossyScale.z, aCollider);
		}

		// player is physicsMediatorOne Ground is two.
		public static bool SweepTest(this PhysicsMediator physicsMediatorOne, PhysicsMediator physicsMediatorTwo)
		{
			Box3D broadphaseBox = sweptBroadPhaseBox(physicsMediatorOne.GetBox3D);
			//if (IntersectAABB(broadphaseBox, physicsMediatorTwo.GetBox3D))
			{
				float normalX, normalY, normalZ;
				float collisionTime = sweptAABB(physicsMediatorOne.GetBox3D, physicsMediatorTwo.GetBox3D, out normalX, out normalY, out normalZ);
				physicsMediatorOne.GetBox3D.xMin += physicsMediatorOne.GetBox3D.xVelocity * collisionTime;
				physicsMediatorOne.GetBox3D.yMin += physicsMediatorOne.GetBox3D.yVelocity * collisionTime;
				physicsMediatorOne.GetBox3D.zMin += physicsMediatorOne.GetBox3D.zVelocity * collisionTime;

				return IntersectAABB(physicsMediatorTwo.GetBox3D, physicsMediatorOne.GetBox3D);
			}

			return false;
		}

		private static bool IntersectAABB(Box3D box3DOne, Box3D box3DTwo)
		{
			bool xMinComp = box3DOne.xMin < box3DTwo.XMax;
			bool xMaxComp = box3DOne.XMax > box3DTwo.xMin;

			bool yMinComp = box3DOne.yMin > box3DTwo.YMax;
			bool yMaxComp = box3DOne.YMax < box3DTwo.yMin;

			bool zMinComp = box3DOne.zMin < box3DTwo.ZMax;
			bool zMaxComp = box3DOne.ZMax > box3DTwo.zMin;

			return xMinComp && xMaxComp && yMinComp && yMaxComp && zMinComp && zMaxComp;
		}

		private static Box3D sweptBroadPhaseBox(Box3D box)
		{
			Box3D broadPhaseBox = Box3D.Zero;
			broadPhaseBox.xMin = box.xVelocity > 0 ? box.xMin : box.xMin + box.xVelocity;
			broadPhaseBox.yMin = box.yVelocity > 0 ? box.yMin : box.yMin + box.yVelocity;
			broadPhaseBox.zMin = box.zVelocity > 0 ? box.zMin : box.zMin + box.zVelocity;

			broadPhaseBox.Width = box.xVelocity > 0 ? box.xVelocity + box.Width : box.Width - box.xVelocity;
			broadPhaseBox.Height = box.yVelocity > 0 ? box.yVelocity + box.Height : box.Height - box.yVelocity;
			broadPhaseBox.Depth = box.zVelocity > 0 ? box.zVelocity + box.Depth : box.Depth - box.zVelocity;

			return broadPhaseBox;
		}

        private static float sweptAABB(Box3D b1, Box3D b2, out float normalX, out float normalY, out float normalZ)
        {
	        normalX = normalY = normalZ = 0.0f;

            float xInvEntry, yInvEntry, zInvEntry;
            float xInvExit, yInvExit, zInvExit;

            if(b1.xVelocity > 0.0f)
            {
				xInvEntry = b2.xMin - b1.XMax;
				xInvExit = b2.XMax - b1.xMin;
            }
            else
            {
				xInvEntry = b2.XMax - b1.xMin;
				xInvExit = b2.xMin - b1.XMax;
            }

            if(b1.yVelocity > 0.0f)
            {
				yInvEntry = b2.yMin - b1.YMax;
				yInvExit = b2.YMax - b1.yMin;
            }
            else
            {
				yInvEntry = b2.YMax - b1.yMin;
				yInvExit = b2.yMin - b1.YMax;
            }

            if(b1.zVelocity > 0.0f)
            {
				zInvEntry = b2.zMin - b1.ZMax;
				zInvExit = b2.ZMax - b1.zMin;
            }
            else 
            {
				zInvEntry = b2.ZMax - b1.zMin;
				zInvExit = b2.zMin - b1.ZMax;
            }


            float xEntry, yEntry, zEntry;
            float xExit, yExit, zExit;

            if(b1.xVelocity == 0.0f)
            {
                xEntry = float.NegativeInfinity;
                xExit = float.PositiveInfinity;
            }
            else
            {
                xEntry = xInvEntry / b1.xVelocity;
                xExit = xInvExit / b1.xVelocity;
            }

            if(b1.yVelocity == 0.0f)
            {
                yEntry = float.NegativeInfinity;
                yExit = float.PositiveInfinity;
            }
            else
            {
                yEntry = yInvEntry / b1.yVelocity;
                yExit = yInvExit / b1.yVelocity;
            }

            if(b1.zVelocity == 0.0f)
            {
                zEntry = float.NegativeInfinity;
                zExit = float.PositiveInfinity;
            }
            else
            {
                zEntry = zInvEntry / b1.zVelocity;
                zExit = zInvExit / b1.zVelocity;
            }

	        if (yEntry > 1.0f)
		        yEntry = -float.MaxValue;
	        if (xEntry > 1.0f)
		        xEntry = -float.MaxValue;
	        if (zEntry > 1.0f)
		        zEntry = -float.MaxValue;

            float entryTime = Math.Max(xEntry, yEntry);
            entryTime = Math.Max(entryTime, zEntry);
            float exitTime = Math.Max(xExit, yExit);
            exitTime = Math.Max(exitTime, zExit);

	        if (entryTime > exitTime)
		        return 1.0f;
	        if (xEntry < 0.0f && yEntry < 0.0f && zEntry < 0.0f)
		        return 1.0f;
	        if (xEntry < 0.0f)
	        {
				if (b1.XMax < b2.xMin || b1.xMin > b2.XMax)
			        return 1.0f;
	        }
	        if (yEntry < 0.0f)
	        {
				if (b1.YMax < b2.yMin || b1.yMin > b2.YMax)
			        return 1.0f;
	        }
	        if (zEntry < 0.0f)
	        {
				if (b1.ZMax < b2.zMin || b1.zMin > b2.YMax)
			        return 1.0f;
	        }
	        
			//TODO delte if above checks work.
			//if (entryTime > exitTime || xEntry < 0.0f && yEntry < 0.0f && zEntry < 0.0f ||
			//	xEntry > 1.0f || yEntry > 1.0f || zEntry > 1.0f)
			//{
			//	return 1.0f;
			//}
			//else
            if(xEntry > yEntry)
            {
                if(xInvEntry < 0.0f)
                {
                    normalX = 1.0f;
                    normalY = 0.0f;
                    normalZ = 0.0f;
                }
                else
                {
                    normalX = -1.0f;
                    normalY = 0.0f;
                    normalZ = 0.0f;
                }
            }
            else if (xEntry > zEntry)
            {
                if (xInvEntry < 0.0f)
                {
                    normalX = 1.0f;
                    normalY = 0.0f;
                    normalZ = 0.0f;
                }
                else
                {
                    normalX = -1.0f;
                    normalY = 0.0f;
                    normalZ = 0.0f;
                }
            }
            else if (yEntry > zEntry)
            {
                if (yInvEntry < 0.0f)
                {
                    normalX = 0.0f;
                    normalY = 1.0f;
                    normalZ = 0.0f;
                }
                else
                {
                    normalX = 0.0f;
                    normalY = -1.0f;
                    normalZ = 0.0f;
                }
            }
            return entryTime;
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
