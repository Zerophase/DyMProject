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
	private float speed = 10.0f;
    private float bulletTimer = 0f;
    private float smoothCurve = 0.0f;
    private float smoothCurveChange = -0.5f;
	private bool setUp = false;
    Vector3 positionStorage;

    static int id = -1;
    private int localID;
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
            id++;
            localID = id;
            transform.position = Player.GunModel.transform.position;

            startPosition = transform.position;
            if (Player.GunModel.GetComponent<Gun>().Rotated)
                fireDirection = -Player.GunModel.transform.up;
            else
                fireDirection = Player.GunModel.transform.up;
            setUp = true;
        }
        bulletTimer += Time.deltaTime;
        Debug.Log("Before: " + smoothCurve);
        positionStorage = PhysicsFuncts.calculateVelocity(speed * fireDirection, Time.deltaTime);

        if (smoothCurve < 1.0f)
        {
            positionStorage.z = smoothCurve = smoothCurveChange * bulletTimer;
            Debug.Log("After: " + smoothCurve);
            
        }
        else if (smoothCurve > 1.0f)
        {
            positionStorage.z = 0.0f;
        }
        
        transform.Translate(positionStorage);

        if (Mathf.Abs((transform.position - startPosition).magnitude) > 3000f)
        {
            smoothCurve = 0f;
            bulletTimer = 0f;
            setUp = false;
            PooledBulletGameObjects.DeactivatePooledBullet(gameObject);
        };
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
