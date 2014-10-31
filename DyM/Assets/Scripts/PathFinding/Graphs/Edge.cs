using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Utilities;

namespace Assets.Scripts.PathFinding.Graphs
{
	public class Edge
	{
		private int from;
		public int From { get { return from; } }
		private int to;
		public int To { get { return to; } }

		private float cost;
		public float Cost { get { return cost; } set { cost = value; } }

		public Edge()
		{
			cost = 1.0f;
			from = (int) NodeTypes.INVALID;
			to = (int) NodeTypes.INVALID;
		}

		public Edge(int from, int to)
		{
			cost = 1.0f;
			this.from = from;
			this.to = to;
		}

		public Edge(int from, int to, float cost)
		{
			this.cost = cost;
			this.from = from;
			this.to = to;
		}

		public bool EqualityCheck(Edge edge)
		{
			return (edge.from == from && edge.to == to && 
				Util.compareEachFloat(edge.cost, cost));
		}
	}
}
