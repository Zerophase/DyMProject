using Assets.Scripts.DependencyInjection;
using Assets.Scripts.MediatorPattern;
using Assets.Scripts.Utilities;
using Assets.Scripts.Utilities.Messaging;
using ModestTree.Zenject;
using  UnityEngine;

namespace Assets.Scripts.GameObjects
{
	public class Ground : PhysicsMediator
	{
		public void Update()
		{
			boundingBox.DrawBoundingBox();
		}
	}
}
