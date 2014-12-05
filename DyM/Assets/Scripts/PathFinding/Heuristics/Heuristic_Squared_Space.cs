using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.PathFinding.Graphs;
using UnityEngine;

namespace Assets.Scripts.PathFinding.Heuristics
{
	public class Heuristic_Squared_Space : Interfaces.Heuristics
	{
		public static float Calculate(SparseGraph sparseGraph, int index1, int index2)
		{
			return vec3DDistanceSquared(sparseGraph.GetNode(index1).Position,
				sparseGraph.GetNode(index2).Position);
		}

        //TODO  Make seperate Distance squareds for each axis and a total one 
		public static float vec3DDistanceSquared(Vector3 v1, Vector3 v2)
		{
			float ySeperation = 0f;
			float xSeperation = v2.x - v1.x;
			float zSeperation = 0f;

			return ySeperation*ySeperation + xSeperation*xSeperation +
			       zSeperation*zSeperation;
		}
	}
}
