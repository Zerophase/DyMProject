using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Utilities;
using ModestTree.Zenject;
using UnityEngine;

namespace Assets.Scripts.DependencyInjection
{
	public class OurColliderFactory
	{
		private Instantiator instantiator;

		public OurColliderFactory(Instantiator instantiator)
		{
			this.instantiator = instantiator;
		}


		public OurCollider Create(Vector3 center, Vector3 size)
		{
			return instantiator.Instantiate<OurCollider>(center, size);
		}
	}
}
