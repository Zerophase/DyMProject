using Assets.Scripts.Abilities.Interfaces;
using ModestTree.Zenject;
using UnityEngine;
using System.Collections;

namespace Assets.Scripts.GameObjects
{
	public class Ability : MonoBehaviour
	{
		[Inject] 
		private IAbility ability;

		void OnCollisionEnter(Collision collision)
		{
			if (collision.gameObject.tag == "Player")
			{
				ability.PickUp(collision.gameObject.GetComponent<Player>().character);
				Destroy(gameObject);
			}
		}
	}
}

