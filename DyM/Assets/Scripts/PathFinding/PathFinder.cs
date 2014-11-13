using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.PathFinding.Graphs;
using Assets.Scripts.PathFinding.Heuristics;
using UnityEngine;

namespace Assets.Scripts.PathFinding
{
	public class PathFinder
	{
		private enum NodesFound { NO_CLOSEST_NODE = -1 };

		private List<int> path;
		private List<Edge> subTree;
		private List<NavGraphNode> currentNeighbor =  new List<NavGraphNode>();

		private SparseGraph sparseGraph;
		public int NodeCount { get { return sparseGraph.NavNodes.Count; } }

		private float range;

		private float costToTarget;

		private int source;
		private int prevNode;

		public bool FirstSearchDone;

		private int closesetNode;
		private int previousClosestNode;

		public PathFinder()
		{
			path = new List<int>();
			subTree = new List<Edge>();
			sparseGraph = new SparseGraph(true);
			costToTarget = 0.0f;
			source = 0;
		}

		public void UpdateGraph(int index, Vector3 location)
		{
			if (!sparseGraph.IsNodePresent(index))
				sparseGraph.AddNode(new NavGraphNode(index, location));
		}

		public void Initialize()
		{
			aStar = new AStar();
		}
		private AStar aStar;
		public Vector3 CreatePathAStarDistanceSquared(Vector3 position)
		{
			range = CalculateAverageGraphEdgeLength() + 1;

			aStar = new AStar(sparseGraph, GetSourceNode(), GetClosestNodeToPosition(position));
			//bugged if using the same a star for multiple path searches.
			//aStar.Reset(sparseGraph,GetSourceNode(), GetClosestNodeToPosition(position));
			//aStar.Search();

			if (!FirstSearchDone)
				FirstSearchDone = true;

			source = prevNode;
			path = aStar.PathToTarget();
			return sparseGraph.GetNode(path[0]).Position;
		}

		public int GetSourceNode()
		{
			return source;
		}

		public int GetClosestNodeToPosition(Vector3 position)
		{
			float closestSoFar = float.MaxValue;
			closesetNode = (int) NodesFound.NO_CLOSEST_NODE;
			if(currentNeighbor.Count > 0)
				currentNeighbor.Clear();

			CalculateNeighbors(position);
			for (int i = 0; i < currentNeighbor.Count; i++)
			{
				float distance = Heuristic_Squared_Space.vec3DDistanceSquared(position,
					currentNeighbor[i].Position);
				if (distance < closestSoFar && distance > 1f)
				{
					closestSoFar = distance;
					closesetNode = currentNeighbor[i].Index;
				}
			}

			previousClosestNode = source;
			prevNode = closesetNode;
			return closesetNode;
		}

		public void CalculateNeighbors(Vector3 position)
		{
			Vector3 queryBox = new Vector3(position.x - range, range, position.z + range);
			for (int i = 0; i < sparseGraph.NumNodes(); i++)
			{
				if (Heuristic_Squared_Space.vec3DDistanceSquared(
					sparseGraph.GetNode(i).Position, position) < queryBox.sqrMagnitude)
				{
					currentNeighbor.Add(sparseGraph.GetNode(i));
				}
			}
		}

		public float CalculateAverageGraphEdgeLength()
		{
			float totalLength = 0.0f;
			int numEdgesCounted = 0;

			for (int i = 0; i < sparseGraph.NumNodes(); i++)
			{
				for (int j = 0; j < 1; j++)
				{
					numEdgesCounted++;
					totalLength += Heuristic_Squared_Space.vec3DDistanceSquared(
						sparseGraph.GetNode(sparseGraph.NodeEdges[i][j].From).Position,
						sparseGraph.GetNode(sparseGraph.NodeEdges[i][j].To).Position);
				}
			}

			return totalLength/(float) numEdgesCounted;
		}

		//TODO fix so works for node counts greater than 4
		public void AddAllNeighborsToNode(int index)
		{
			for (int i = 0; i < sparseGraph.NavNodes.Count; i++)
			{
				Vector3 posNode = sparseGraph.GetNode(index).Position;
				Vector3 posNeighbor;
				if (i == sparseGraph.NavNodes.Count - 1)
					posNeighbor = sparseGraph.GetNode(0).Position;
				else
					posNeighbor = sparseGraph.GetNode(index + i).Position;

				float distance = Heuristic_Squared_Space.
					vec3DDistanceSquared(posNode, posNeighbor);

				Edge edge;
				if(i == sparseGraph.NumEdges())
					edge = new Edge(index, 0, distance);
				else if(index == sparseGraph.NavNodes.Count - 1)
					edge = new Edge(index, 0, distance);
				else
					edge = new Edge(index, index + i, distance);

				sparseGraph.AddEdge(edge);
			}
		}

		public float UpdateDistanceTraveled(Vector3 position)
		{
			return Heuristic_Squared_Space.vec3DDistanceSquared(
				sparseGraph.GetNode(closesetNode).Position, position);
		}
	}
}
