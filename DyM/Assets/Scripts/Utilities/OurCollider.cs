using UnityEngine;

namespace Assets.Scripts.Utilities
{
	public class OurCollider 
	{
		private Bounds bounds;
	
		private bool collided;
		Vector3 maxDistance;
		public OurCollider(Vector3 center, Vector3 size)
		{
			this.bounds = new Bounds(center, size);
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
	}
}
