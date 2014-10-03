using Assets.Scripts.GameObjects;
using Assets.Scripts.Projectiles;
using Assets.Scripts.Projectiles.Interfaces;
using ModestTree.Zenject;
using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
	// TODO see if can remove these and just use IPooledGameObjects
	private IProjectile projectile;
	public IProjectile Projectile { set { projectile = value; } }

	[Inject] 
	public IPooledGameObjects PooledBulletGameObjects;

	private Vector3 startPosition;
	private Vector3 fireDirection;

	private bool setUp = false;

	void Update ()
	{
		if (gameObject.activeInHierarchy && !setUp)
		{
			transform.position = (Player.GunModel.transform.position + new Vector3(0f, Player.GunModel.transform.lossyScale.y, 0f));
			startPosition = transform.position;
			fireDirection = Player.GunModel.transform.up;
			setUp = true;
		}

		if(!Player.GunModel.GetComponent<Gun>().Rotated)
			transform.position += fireDirection * Time.deltaTime;
		else if (Player.GunModel.GetComponent<Gun>().Rotated)
			transform.position -= fireDirection * Time.deltaTime;

		if (Mathf.Abs((transform.position - startPosition).magnitude) > 5f)
		{
			setUp = false;
			PooledBulletGameObjects.DeactivatePooledBullet(gameObject);
		}
	}
}
