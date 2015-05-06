using UnityEngine;

namespace Assets.Scripts.MediatorPattern
{
	public abstract class ItemPickUp : PhysicsMediator
	{
		public abstract void PickUp(GameObject player);
	}
}