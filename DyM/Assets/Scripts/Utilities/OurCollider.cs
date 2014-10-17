using UnityEngine;

namespace Assets.Scripts.Utilities
{
	public class OurCollider 
	{
		private Bounds bounds;
		public Bounds GetBounds { get { return bounds; }}

		public Vector3 Center { set { bounds.center = value; } }
		private bool collided;
		Vector3 maxDistance;
		public OurCollider(Vector3 center, Vector3 size)
		{
			bounds = new Bounds(center, size);
		}
	
		public bool CheckBounds(Bounds boundsToCheck)
		{
			collided = false;
		 
			maxDistance = this.bounds.center + boundsToCheck.center;
		 
			if (maxDistance.x > Mathf.Abs(this.bounds.size.x - boundsToCheck.size.x)) 
				collided = true;
			if(maxDistance.y > Mathf.Abs(this.bounds.size.y - boundsToCheck.size.y))
				collided = true;
			if(maxDistance.z > Mathf.Abs(this.bounds.size.z - boundsToCheck.size.z))
				collided = true;
		   		
			return collided;
		}

		public void DrawDebug()
		{
			Vector3 point = bounds.center + bounds.extents;
			Vector3 point2 = bounds.center - bounds.extents;
			Debug.DrawLine(point, point2);
		}
	}
}
