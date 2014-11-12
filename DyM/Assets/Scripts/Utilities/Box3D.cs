using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Utilities
{
    public class Box3D
    {
	    private BoxCollider boxCollider;
		public BoxCollider GetBoxCollider { get { return boxCollider; }}
		public BoxCollider SetBoxCollider { set { boxCollider = value; } }
        private Vector3 position;
		public Vector3 SetPosition { get { return position; } set { position = value; } }
		public float setXPosition { get { return position.x; } set { position.x = value; } }
		public float setYPosition { get { return position.y; } set { position.y = value; } }
		public float setZPosition { get { return position.z; } set { position.z = value; } }

	    private float width;

	    public float Width
	    {
		    get { return width; }
		    set
		    {
			    width = value;
		    }
	    }
		private float height;
		public float Height
		{
			get { return height; }
			set
			{
				height = value;
			}
		}
        private float depth;
		public float Depth
		{
			get { return depth; }
			set
			{
				depth = value;
			}
		}


		public float xMin {get { return position.x; } set { position.x = value; }}
		public float XMax { get { return xMin + width; } }

		public float yMin { get { return position.y; } set { position.y = value; } }
        public float YMax {get { return position.y + height; } }

		public float zMin { get { return position.z; } set { position.z = value; } }
		public float ZMax { get { return zMin + depth; } }

        public float xVelocity;
        public float yVelocity;
        public float zVelocity;

	    public Vector3 Velocity;

	    public static Box3D Zero = new Box3D(Vector3.zero, 0f, 0f, 0f);

        public Box3D(Vector3 position, float width, float height, float depth)
        {
            this.position = position;
            this.width = width;
            this.height = height;
            this.depth = depth;
        }

        public Box3D(float x, float y, float z, float width, float height, float depth, BoxCollider boxCollider)
        {
			position.x = x;
			position.y = y;
			position.z = z;

	        this.width = width;
	        this.height = height;
	        this.depth = depth;

	        this.boxCollider = boxCollider;
        }

		public Box3D(float x, float y, float z, float width, float height, float depth)
		{
			position.x = x;
			position.y = y;
			position.z = z;

			this.width = width;
			this.height = height;
			this.depth = depth;
		}

	    public void UpdateBox3D(GameObject gameObject)
	    {
			position = gameObject.transform.position;

			// updates x min, y min, and z min
			position += boxCollider.center;
			position.x -= (boxCollider.size.x * gameObject.transform.lossyScale.x) / 2;
			position.y += (boxCollider.size.y * gameObject.transform.lossyScale.y) / 2;
			position.z += (boxCollider.size.z * gameObject.transform.lossyScale.z) / 2;
			
			//To YMAX 
			Debug.DrawLine(new Vector3(xMin, yMin, zMin), new Vector3(xMin, YMax, zMin), Color.blue, 1f);
			//TO XMAX
			Debug.DrawLine(new Vector3(xMin, yMin, zMin), new Vector3(XMax, yMin, zMin), Color.blue, 1f);
			//TO ZMAX
			Debug.DrawLine(new Vector3(xMin, yMin, zMin), new Vector3(xMin, yMin, ZMax), Color.blue, 1f);
	    }
    }
}
