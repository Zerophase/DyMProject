using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.PathFinding.Graphs.Base
{
	public abstract class Node
	{
		private int index;
		public int Index { get { return index; } set { index = value; } }
		private static int indexIterator = 0;

		public Node()
		{
			index = indexIterator++;
		}

		public Node(int index)
		{
			this.index = index;
			indexIterator++;
		}
	}
}
