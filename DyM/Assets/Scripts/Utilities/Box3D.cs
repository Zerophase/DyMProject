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
        public float xMax;
        public float yMax;
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

        public Box3D(float xMin, float xMax, float yMin, float yMax, float zMin, float zMax)
        {
            this.position = new Vector3(xMin, yMin, zMin);
            this.width = xMax - xMin;
            this.height = yMax - yMin;
            this.depth = zMax - zMin;
            this.xMax = xMax;
            this.yMax = yMax;
            this.zMax = zMax;
        }


    }
}
