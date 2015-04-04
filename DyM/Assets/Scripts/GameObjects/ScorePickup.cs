using UnityEngine;
using System.Collections;
using ModestTree.Zenject;
using Assets.Scripts.Utilities.Messaging.Interfaces;
using Assets.Scripts.Utilities.Messaging;
using Assets.Scripts.MediatorPattern;
using Assets.Scripts.GameObjects;

namespace Assets.Scripts.GameObjects
{    
    public class ScorePickup : PhysicsMediator
    {
        public int value = 0;
        public void PickUp(GameObject player)
        {
            player.GetComponent<Player>().AddScore(value);
            Debug.Log("Score Picked Up");
            Destroy(gameObject);
        }
    }
}
