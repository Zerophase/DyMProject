using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.CollisionBoxes.ThreeD;

namespace Assets.Scripts.Collision.SpatialPartioning
{
    

    public class DoublyLinkedList
    {
        private AABB3D aabb;
        
        private DoublyLinkedList[] previous = new DoublyLinkedList[3];
        private DoublyLinkedList[] next = new DoublyLinkedList[3];
        private DoublyLinkedList[] head = new DoublyLinkedList[3];

        float[] coordinateValue = new float[3];
        bool minOrMax;

        private const int dimensions = 3;

        public DoublyLinkedList()
        {
            aabb = null;
            next = null;
            previous = null;
        }

        public DoublyLinkedList(AABB3D value)
        {
            aabb = value;
            next = null;
            previous = null;
        }

        //public DoublyLinkedList InsertNext(AABB3D aabb)
        //{
        //    DoublyLinkedList node = new DoublyLinkedList(aabb);
        //    if(this.next == null)
        //    {
        //        node.previous = this;
        //        this.next = node;
        //    }
        //    else
        //    {
        //        DoublyLinkedList temp = this.next;
        //        node.previous = this;
        //        node.next = temp;
        //        this.next = node;
        //        temp.previous = node;
        //    }
        //    return node;
        //}

        //public DoublyLinkedList InsertPrevious(AABB3D aabb)
        //{
        //    DoublyLinkedList node = new DoublyLinkedList(aabb);
        //    if (this.previous == null)
        //    {
        //        node.next = this;
        //        this.previous = node;
        //    }
        //    else
        //    {
        //        DoublyLinkedList temp = this.previous;
        //        node.previous = temp;
        //        node.next = this;
        //        this.previous = node;
        //        temp.next = node;
        //    }

        //    return node;
        //}

        //public void TraverseFront()
        //{
        //    TraverseFront(this);
        //}

        //public void TraverseFront(DoublyLinkedList node)
        //{
        //    if (node == null)
        //        node = this;

        //    while (node != null)
        //    {
        //        node = node.next;
        //    }
        //}

        //public void TraverseBack()
        //{

        //}

        //public void TraverseBack(DoublyLinkedList node)
        //{
        //    if (node == null)
        //        node = this;

        //    while (node != null)
        //    {
        //        node = node.previous;
        //    }
        //}

        // TODO work on implentation
        public AABB3D GetAABB(DoublyLinkedList element)
        {
            return minOrMax == true  ? element.aabb : element.aabb;
        }

        // TODO figure out
        public void Iterate()
        {
            DoublyLinkedList sentinel = new DoublyLinkedList();
            for (int i = 0; i < dimensions; i++)
            {
               
            }
        }

        public void InsertAABBIntoList(AABB3D aabb)
        {
            for (int i = 0; i < dimensions; i++)
            {
                DoublyLinkedList element = head[i];

                while (element.coordinateValue[i] < aabb.Center[i]) // add min doublylinked list to aabb3d
                    element = element.next[i];
                // aabb minValue.Previous[i] = element.previous[i];
                // aabb minValue.Next[i] = element
                // element.previous[i].next[i] = aabb.min
                // elemnt.previous[i] = aabb.min

                while (element.coordinateValue[i] < aabb.Center[i]) // add max doublylinked list to aabb3d
                    element = element.next[i];
                //aabb.max.previous[i] = element.previous[i]
                // aabb.max.next[i] = element;
                // element.previous[i].next[i] = aabb.max;
                // element.previous[i] = aabb.max;
            }

            // todo merge pair tracking into loop above if speed needs to be faster.
            for (DoublyLinkedList element = head[0]; ; ) // TODO figure out equivalent to this in c#
            {
                if (element.minOrMax == false)
                {
                    if (element.coordinateValue[0] > aabb.Center[0]) // replace with max
                        break;
                    
                }
            }
        }
    }
}
