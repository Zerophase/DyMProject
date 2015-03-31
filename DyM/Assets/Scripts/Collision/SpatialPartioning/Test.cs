using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Assets.Scripts.CollisionBoxes.ThreeD;

namespace Assets.Scripts.Collision.SpatialPartioning
{
    public class Test
    {
        LinkedList<AABB3D> listOfBoxes = new LinkedList<AABB3D>();

        AABB3D[] test;
        public void LinkedListTest()
        {
            test = new AABB3D[50];
            for (int i = 0; i < test.Length; i++)
            {
                test[i] = new AABB3D();
                listOfBoxes.AddFirst(test[i]);
            }

            for (int i = 0; i < listOfBoxes.Count; i++)
            {
                lis
            }
        }
    }
}
