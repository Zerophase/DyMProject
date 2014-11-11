using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Utilities
{
    public class Box3D
    {
	    private BoxCollider boxCollider;
		public BoxCollider GetBoxCollider { get { return boxCollider; }}
        private Vector3 position;

	    private float width;

	    public float Width
	    {
		    get { return width; }
		    set
		    {
			    width = value;
			    xMax = xMin + width;
		    }
	    }
		private float height;
		public float Height
		{
			get { return height; }
			set
			{
				height = value;
				yMax = yMin + height;
			}
		}
        private float depth;
		public float Depth
		{
			get { return depth; }
			set
			{
				depth = value;
				zMax = zMin + depth;
			}
		}


		public float xMin {get { return position.x; } set { position.x = value; }}
	    private float xMax;
		public float XMax { get { return xMin + width; } }

		public float yMin { get { return position.y; } set { position.y = value; } }
	    private float yMax;
        public float YMax {get { return yMin + height; } }

		public float zMin { get { return position.z; } set { position.z = value; } }
        private float zMax;
		public float ZMax { get { return zMin + depth; } }

        public float xVelocity;
        public float yVelocity;
        public float zVelocity;

	    public static Box3D Zero = new Box3D(Vector3.zero, 0f, 0f, 0f);

        public Box3D(Vector3 position, float width, float height, float depth)
        {
            this.position = position;
            this.width = width;
            this.height = height;
            this.depth = depth;
            xMax = position.x + width;
            yMax = position.y + height;
            zMax = position.z + depth;
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

			// updates x min, y min, and z min
			position += boxCollider.center;
			position.x -= (boxCollider.size.x * gameObject.transform.lossyScale.x) / 2;
			position.y += (boxCollider.size.y * gameObject.transform.lossyScale.y) / 2;
			position.z -= (boxCollider.size.z * gameObject.transform.lossyScale.z) / 2;

		    xMax = position.x + width;
		    yMax = position.y + height;
		    zMax = position.z + depth;
	    }
    }
}
