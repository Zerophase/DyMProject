using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Utilities
{
    public class Box3D
    {
        public Vector3 position;
        public float width;
        public float height;
        public float depth;

		public float xMin;
        public float xMax;

	    public float yMin;
        public float yMax;

	    public float zMin;
        public float zMax;
	    

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

        public Box3D(float x, float y, float z, float width, float height, float depth)
        {
            this.xMax = x + width;
            this.yMax = y + height;
            this.zMax = z + depth;

	        this.xMin = x;
	        this.yMin = y;
	        this.zMin = z;
        }


    }
}
