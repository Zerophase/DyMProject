using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Utilities
{
    public class Box3D
    {
	    private BoxCollider boxCollider;
        private Vector3 position;

        public float width;
        public float height;
        public float depth;

		public float xMin {get { return position.x; }}
        public float xMax;

	    public float yMin {get { return position.y; }}
        public float yMax;

	    public float zMin {get { return position.z; }}
        public float zMax;

        public float xVelocity;
        public float yVelocity;
        public float zVelocity;

        public Box3D(Vector3 position, float width, float height, float depth)
        {
            this.position = position;
            this.width = width;
            this.height = height;
            this.depth = depth;
            this.xMax = position.x + width;
            this.yMax = position.y + height;
            this.zMax = position.z + depth;
        }

        public Box3D(float x, float y, float z, float width, float height, float depth, BoxCollider boxCollider)
        {
			position.x = x;
			position.y = y;
			position.z = z;

	        this.width = width;
	        this.height = height;
	        this.depth = depth;

			xMax = position.x + this.width;
			yMax = position.y + this.height;
			zMax = position.z + this.depth;

	        this.boxCollider = boxCollider;
        }

	    public void UpdateBox3D(GameObject gameObject)
	    {
			position = gameObject.transform.position;

			position += boxCollider.center;
			position.x -= (boxCollider.size.x * gameObject.transform.lossyScale.x) / 2;
			position.y += (boxCollider.size.y * gameObject.transform.lossyScale.y) / 2;
			position.z -= (boxCollider.size.z * gameObject.transform.lossyScale.z) / 2;

		    xMax = xMin + width;
		    yMax = yMin + height;
		    zMax = zMin + depth;
	    }
    }
}
