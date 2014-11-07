using Assets.Scripts.GameObjects;
using Assets.Scripts.Projectiles;
using Assets.Scripts.Projectiles.Interfaces;
using Assets.Scripts.Projectiles.Projectiles;
using ModestTree.Zenject;
using UnityEngine;
using System.Collections;
using Assets.Scripts.Utilities;

public class Bullet : MonoBehaviour
{
	// TODO see if can remove these and just use IPooledGameObjects
	private IProjectile projectile;
	public IProjectile Projectile { set { projectile = value; } }

	[Inject] 
	public IPooledGameObjects PooledBulletGameObjects;

	private Vector3 startPosition;
	private Vector3 fireDirection;
	private float speed = 20.0f;
	
	
	private bool setUp = false;

	private float totalTime = 0f;

	void Update ()
	{
		if(projectile is MachineGunProjectile)
			machineGunBullet();
		else if (projectile is LightningGunProjectile)
			lightningGunBullet();
	}

	private void lightningGunBullet()
	{
		//There was a bug that caused the bullets to explode by using: 
		//fireDirection = -Player.GunModel.transform.up + new Vector3(0f, 0f, 0.5f);
		//might be interesting to make a gun that shoots outwards and then explodes in
		//4 directions
		if (gameObject.activeInHierarchy && !setUp)
		{
			transform.position = (Player.GunModel.transform.position + new Vector3(0f, Player.GunModel.transform.lossyScale.y, 0f));
			startPosition = transform.position;
			if (Player.GunModel.GetComponent<Gun>().Rotated)
				fireDirection = -Player.GunModel.transform.up;
			else
				fireDirection = Player.GunModel.transform.up;
			setUp = true;
		}
		Vector3 temp;
		if (Util.compareEachFloat(transform.position.magnitude, (startPosition + new Vector3(-1f, 0f, -1f)).magnitude))
			temp = Vector3.Slerp(startPosition, startPosition + new Vector3(-1f, 0f, -1f), totalTime);
		else
			temp = transform.position + fireDirection;

		totalTime += Time.deltaTime / 100;
		if (totalTime >= 1f)
			totalTime = 0f;

		transform.position = temp;
	}

	private void machineGunBullet()
	{
		if (gameObject.activeInHierarchy && !setUp)
		{
			transform.position = Player.GunModel.transform.position;

			startPosition = transform.position;
			if (Player.GunModel.GetComponent<Gun>().Rotated)
				fireDirection = -Player.GunModel.transform.up;
			else
				fireDirection = Player.GunModel.transform.up;
			setUp = true;
		}

		transform.Translate(PhysicsFuncts.calculateVelocity(speed*fireDirection, Time.deltaTime));

		if (Mathf.Abs((transform.position - startPosition).magnitude) > 10f)
		{
			setUp = false;
			PooledBulletGameObjects.DeactivatePooledBullet(gameObject);
		}
	}
}
