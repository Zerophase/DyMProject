using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Debug = UnityEngine.Debug;

namespace Assets.Scripts.PathFinding.Graphs
{
	public class SparseGraph
	{
		private bool digraph;
		public bool Digraph { get { return digraph; } }

		private int nextNodeIndex;

		private List<NavGraphNode> navNodes = new List<NavGraphNode>();
		public List<NavGraphNode> NavNodes { get { return navNodes; } }
 
		private Dictionary<int, List<Edge>> nodeEdges = new Dictionary<int, List<Edge>>();
		public Dictionary<int, List<Edge>> NodeEdges { get { return nodeEdges; } }

		public SparseGraph(bool digraph)
		{
			this.digraph = digraph;
			nextNodeIndex = 0;
		}

		private bool uniqueEdge(int from, int to)
		{
			foreach (var item in nodeEdges[from])
			{
				if (item.To == to)
					return false;
			}

			return true;
		}

		private void cullInvalidEdges()
		{
			for (int i = 0; i < nodeEdges.Count; i++)
			{
				foreach (var nodeEdge in nodeEdges[i])
				{
					if (navNodes[nodeEdge.To].Index == (int) NodeTypes.INVALID ||
					    navNodes[nodeEdge.From].Index == (int) NodeTypes.INVALID)
					{
						nodeEdges[i].Remove(nodeEdge);
					}
				}
			}
		}

		public NavGraphNode GetNode(int index)
		{
			try
			{
				return navNodes[index];
			}
			catch (Exception)
			{
				Debug.Log("Null reference exception when accessing node: " + navNodes[index]);
				throw;
			}
		}

		public Edge GetEdge(int from, int to)
		{
			foreach (var nodeEdge in nodeEdges[from])
			{
				if (nodeEdge.To == to)
					return nodeEdge;
			}

			return null;
		}

		public int GetNextFreeNodeIndex() { return nextNodeIndex; }

		public int AddNode(NavGraphNode node)
		{
			if (node.Index < navNodes.Count)
			{
				navNodes[node.Index] = node;
				return nextNodeIndex;
			}
			else
			{
				navNodes.Add(node);
				nodeEdges.Add(nextNodeIndex, new List<Edge>());
				return nextNodeIndex++;
			}
		}

		public void RemoveNode(int index)
		{
			navNodes[index].Index = (int) NodeTypes.INVALID;

			if (!digraph)
			{
				foreach (var edgeFrom in nodeEdges[index])
				{
					foreach (var edgeTo in nodeEdges[edgeFrom.To])
					{
						if (edgeTo.To == index)
						{
							nodeEdges[edgeTo.To].Remove(edgeTo);
							break;
						}
					}
				}

				nodeEdges[index].Clear();
			}
			else
				cullInvalidEdges();
		}

		public void AddEdge(Edge edge)
		{
			if (navNodes[edge.To].Index != (int) NodeTypes.INVALID &&
			    navNodes[edge.From].Index != (int) NodeTypes.INVALID)
			{
				if(uniqueEdge(edge.From, edge.To))
					nodeEdges[edge.From].Add(edge);
			}
		}

		public void RemoveEdge(Edge edge)
		{
			if (navNodes[edge.To].Index != (int)NodeTypes.INVALID &&
			navNodes[edge.From].Index != (int)NodeTypes.INVALID)
			{
				if (uniqueEdge(edge.From, edge.To))
					nodeEdges[edge.From].Add(edge);
			}

			//adds an edge going back in the other direction
			if (!digraph)
			{
				if (uniqueEdge(edge.To, edge.From))
				{
					Edge newEdge = new Edge(edge.To, edge.From);
					nodeEdges[edge.To].Add(newEdge);
				}
			}
		}

		public void SetEdgeCost(int from, int to, float cost)
		{
			foreach (var nodeEdge in nodeEdges[from])
			{
				if (nodeEdge.To == to)
				{
					nodeEdge.Cost = cost;
					break;
				}
			}
		}

		public int NumNodes() { return navNodes.Count; }

		public int NumActiveNodes()
		{
			int count = 0;

			for (int i = 0; i < navNodes.Count; i++)
			{
				if (navNodes[i].Index != (int) NodeTypes.INVALID)
					count++;
			}

			return count;
		}

		public int NumEdges()
		{
			int count = 0;
			for (int i = 0; i < nodeEdges.Count; i++)
			{
				count += nodeEdges[i].Count;
			}

			return count;
		}

		public bool IsEmpty()
		{
			return (navNodes.Count == 0);
		}

		public bool IsNodePresent(int index)
		{
			return !(index >= navNodes.Count ||
			         navNodes[index].Index == (int) NodeTypes.INVALID);
		}

		public bool IsEdgePresent(int from, int to)
		{
			if (IsNodePresent(from) && IsNodePresent(to))
			{
				foreach (var nodeEdge in nodeEdges[from])
				{
					if (nodeEdge.To == to)
						return true;
				}
			}

			return false;
		}

		public void Clear()
		{
			nextNodeIndex = 0;
			navNodes.Clear();
			nodeEdges.Clear();
		}

		public void RemoveEdges()
		{
			for (int i = 0; i < nodeEdges.Count; i++)
			{
				nodeEdges[i].Clear();
			}
		}
	}
}
