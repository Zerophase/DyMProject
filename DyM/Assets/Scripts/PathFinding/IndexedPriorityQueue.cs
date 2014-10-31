using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.PathFinding
{
	public class IndexedPriorityQueue
	{
		private List<float> keys = new List<float>();
		public List<float> Keys { set { keys = value; } }
 		private List<int> heap = new List<int>();
 		private List<int> inventoryHeap = new List<int>();
		private int size, maxSize;

		public IndexedPriorityQueue(List<float> keys, int maxSize)
		{
			this.keys = keys;

			size = 0;
			this.maxSize = maxSize;

			for (int i = 0; i < maxSize + 1; i++)
			{
				heap.Add(0);
				inventoryHeap.Add(0);
			}
		}

		private void swap(int a, int b)
		{
			int temp = heap[a];

			heap[a] = heap[b];
			heap[b] = temp;

			inventoryHeap[heap[a]] = a;
			inventoryHeap[heap[b]] = b;
		}

		private void reorderUpwards(int index)
		{
			while (index > 1 && keys[heap[index/2]] > keys[heap[index]])
			{
				swap(index / 2, index);

				index /= 2;
			}
		}

		private void reorderDownwards(int index, int heapSize)
		{
			while (2 * index <= heapSize)
			{
				int child = 2*index;

				if (child < heapSize && keys[heap[child]] > keys[heap[child + 1]])
					++child;

				if (keys[heap[index]] > keys[heap[child]])
				{
					swap(child, index);
					index = child;
				}
				else
					break;
			}
		}

		public bool Empty()
		{
			return size == 0;
		}

		public void Insert(int index)
		{
			++size;
			heap[size] = index;

			inventoryHeap[index] = size;
			reorderUpwards(size);
		}

		public int Pop()
		{
			swap(1, size);

			reorderDownwards(1, size - 1);

			return heap[size--];
		}

		public void ChangePriority(int index)
		{
			reorderUpwards(inventoryHeap[index]);
		}
	}
}
