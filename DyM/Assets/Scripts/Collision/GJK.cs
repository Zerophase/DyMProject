using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Utilities;
using UnityEngine;

namespace Assets.Scripts.Collision
{
	public class GJK
	{
		List<Vector3> simplex = new List<Vector3>();
 
		private static GJK instance;

		public static GJK Instance
		{
			get { return instance ?? (instance = new GJK()); }
		}

		public bool TestCollision(Box3D movingBox, Box3D staticBox)
		{
			// Might need to update velocity calcuation.
			Vector3 direction = movingBox.Velocity;

			simplex.Add(movingBox.Support(staticBox, direction));

			direction = -direction;

			while (true)
			{
				simplex.Add(movingBox.Support(staticBox, direction));

				if (Vector3.Dot(simplex[simplex.Count - 1], direction) <= 0.0f)
				{
					return false;
				}
				else
				{
					// need to figure out how to calculate the origin.
					Vector3 origin = new Vector3();
					if (simplex.Contains(origin))
						return true;
					else
						direction = GetDirection(simplex);
				}
			}
		}

		public Vector3 GetDirection(List<Vector3> simplex)
		{
			// do stuff with simplex to find the next vector 3.
			return  new Vector3();
		}
	}
}
