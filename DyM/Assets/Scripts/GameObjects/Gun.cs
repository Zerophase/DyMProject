using Assets.Scripts.Character;
using Assets.Scripts.GameObjects;
using Assets.Scripts.Weapons.Interfaces;
using ModestTree.Zenject;
using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour
{
	[Inject] 
	private IRangeWeapon weapon;

	private Quaternion weaponOrigin;

	[HideInInspector]
	public bool Rotated;

	void Start()
	{
		weaponOrigin = gameObject.transform.rotation;
	}

	

	void Update ()
	{
		Vector3 direction = weaponOrigin.eulerAngles - new Vector3(Input.GetAxis("CameraHorizontalMovement"), Input.GetAxis("CameraVerticalMovement"));

		float rotate = Mathf.Atan2(direction.y, direction.x)*Mathf.Rad2Deg;

		if (direction.x > 0)
		{
			transform.rotation = Quaternion.Euler(0f, 0f, -rotate);
			Rotated = false;
		}
		else
		{
			transform.rotation = Quaternion.Euler(0f, 0f, -rotate + 180);
			Rotated = true;
		}
	}

	void OnCollisionEnter(Collision collision)
	{
		if(this.tag == "EquippedGun")
			return;

		if (collision.gameObject.tag == "Player")
		{
			weapon.PickUp(collision.gameObject.GetComponent<Player>().character);
			Destroy(gameObject);
		}
			
	}
}
