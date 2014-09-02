using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Utilities
{
	public class Vector3EqualityComparerWithTolerance : IEqualityComparer<Vector3>
	{
		private float epsilon;

		public Vector3EqualityComparerWithTolerance(float epsilon = float.Epsilon)
		{
			this.epsilon = epsilon;
		}

		public bool Equals(Vector3 v1, Vector3 v2)
		{
			return equalsWithTolerance(v1, v2);
		}

		private bool equalsWithTolerance(Vector3 v1, Vector3 v2)
		{
			if (Util.compareEachFloat(v1.x, v2.x) && Util.compareEachFloat(v1.y, v2.y)
				&& Util.compareEachFloat(v1.z, v2.z))
				return true;

			return false;
		}

		public int GetHashCode(Vector3 obj)
		{
			throw new NotImplementedException();
		}
	}
}
