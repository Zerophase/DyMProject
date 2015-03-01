using UnityEngine;
using System.Collections;
using ModestTree.Zenject;
using Assets.Scripts.Utilities.Messaging.Interfaces;
using Assets.Scripts.Utilities.Messaging;
using Assets.Scripts.MediatorPattern;
using Assets.Scripts.GameObjects;

public class HealthPack : PhysicsMediator
{
	int heal = 50;
	public void PickUp(GameObject player)
	{
		player.GetComponent<Player>().Heal(heal);
		Destroy(gameObject);
	}
}
