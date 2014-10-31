using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.PathFinding.Graphs.Base;
using UnityEngine;

namespace Assets.Scripts.PathFinding.Graphs
{
	public class NavGraphNode : Node
	{
		private Vector3 position;
		public Vector3 Position { get { return position; } }

		//TODO check if base constructor needs to be called
		public NavGraphNode(Vector3 position)
		{
			this.position = position;
		}

		public NavGraphNode(int index, Vector3 position)
			: base(index)
		{
			this.position = position;
		}
	}
}
