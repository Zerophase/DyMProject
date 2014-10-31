using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.PathFinding.Graphs;
using Assets.Scripts.PathFinding.Heuristics;

namespace Assets.Scripts.PathFinding
{
	public class AStar
	{
		private SparseGraph sparseGraph;
		private int source;
		private int target;
		private List<float> fCosts = new List<float>();
 		private List<float> gCosts = new List<float>();
		public float CostToTarget { get { return gCosts[target]; } }

		private List<Edge> shortestPathTree = new List<Edge>();
		public List<Edge> ShortestPathTree { get { return shortestPathTree; } }
 		private List<Edge> searchFrontier = new List<Edge>();

		public AStar(SparseGraph graph, int source, int target)
		{
			sparseGraph = graph;
			this.source = source;
			this.target = target;

			shortestPathTree.Capacity = sparseGraph.NumNodes();
			searchFrontier.Capacity = sparseGraph.NumNodes();

			for (int i = 0; i < sparseGraph.NumNodes(); i++)
			{
				shortestPathTree.Add(null);
				searchFrontier.Add(null);

				gCosts.Add(0);
				fCosts.Add(0);
			}

			Search();
		}

		public void Search()
		{
			IndexedPriorityQueue pq = 
				new IndexedPriorityQueue(fCosts, sparseGraph.NumNodes());

			pq.Insert(source);
			while (!pq.Empty())
			{
				int nextClosestNode = pq.Pop();

				shortestPathTree[nextClosestNode] = searchFrontier[nextClosestNode];

				if(nextClosestNode == target)
					return;

				foreach (var edge in sparseGraph.NodeEdges[nextClosestNode])
				{
					float hCost = Heuristic_Squared_Space.Calculate(sparseGraph,
						target, edge.To);
					float gCost = gCosts[nextClosestNode] + edge.Cost;

					if (searchFrontier[edge.To] == null)
					{
						fCosts[edge.To] = gCost + hCost;
						pq.Keys = fCosts;
						gCosts[edge.To] = gCost;
						pq.Insert(edge.To);

						searchFrontier[edge.To] = edge;
					}
					else if (gCost < gCosts[edge.To] &&
						shortestPathTree[edge.To] == null)
					{
						fCosts[edge.To] = gCost + hCost;
						gCosts[edge.To] = gCost;

						pq.ChangePriority(edge.To);
						searchFrontier[edge.To] = edge;
					}
				}
			}
		}

		public List<int> PathToTarget()
		{
			List<int> path = new List<int>();

			if (target < 0)
				return path;

			int index = target;

			path.Add(index);

			while (index != source && shortestPathTree[index] != null)
			{
				index = shortestPathTree[index].From;
				path.Add(index);
			}

			return path;
		}
	}
}
