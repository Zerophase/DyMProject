using Assets.Scripts.Character;
using Assets.Scripts.DependencyInjection;
using Assets.Scripts.GameObjects;
using Assets.Scripts.Weapons;
using Assets.Scripts.Weapons.Interfaces;
using ModestTree.Zenject;
using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour
{
	private Quaternion weaponOrigin;
    private Quaternion rotationOrigin;
	[HideInInspector]
	public bool Rotated;

	void Start()
	{
        rotationOrigin = gameObject.transform.parent.rotation;
        weaponOrigin = gameObject.transform.rotation;
	}

	

	void Update ()
	{
        Vector3 direction = rotationOrigin.eulerAngles - new Vector3(Input.GetAxis("CameraHorizontalMovement"), Input.GetAxis("CameraVerticalMovement"));

		float rotate = Mathf.Atan2(direction.y, direction.x)*Mathf.Rad2Deg;

		//if (direction.x > 0)
		{
            transform.parent.rotation = Quaternion.Euler(0f, 0f, -rotate);
			Rotated = false;
		}
		//else
		{
           // transform.parent.rotation = Quaternion.Euler(0f, 0f, -rotate + 180);
			Rotated = true;
		}
	}
}
