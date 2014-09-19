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

	private Vector3 shootForward = new Vector3(1f, 0f, 0f);
	[Inject] 
	public IPooledGameObjects PooledBulletGameObjects;

	private Vector3 startPosition;

	void Start()
	{
		startPosition = transform.position;
	}

	void Update ()
	{
		transform.position +=  shootForward.x * Player.GunModel.transform.up;

		if (Mathf.Abs((startPosition + transform.position).x) > 5f)
		{
			PooledBulletGameObjects.DeactivatePooledBullet(gameObject);
		}
	}
}
