using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.ObjectManipulation;
using ModestTree.Zenject;
using UnityEngine;

namespace Assets.Scripts.DependencyInjection
{
	public class PlaneShiftFactory
	{
		private IFactory<PlaneShift> factory;
		private Instantiator instantiator;

		public PlaneShiftFactory(Instantiator instantiator)
		{
			this.instantiator = instantiator;
		}

		public PlaneShift Create(Vector3 position)
		{
			return instantiator.Instantiate<PlaneShift>(position);
		}
	}
}
