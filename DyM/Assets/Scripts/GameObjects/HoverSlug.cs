using Assets.Scripts.GameObjects;
using Assets.Scripts.Projectiles;
using Assets.Scripts.Projectiles.Interfaces;
using Assets.Scripts.Utilities.Messaging;
using UnityEngine;
using Assets.Scripts.Weapons.Interfaces;
using ModestTree.Zenject;
using Assets.Scripts.Weapons.Bases;
using Assets.Scripts.Weapons.Guns;

public class HoverSlug : Slug
{
	private Gun gun;

	private GameObject gunModel;

	[Inject]
	public IRangeWeapon slugGun;

	[Inject] public IPooledGameObjects PooledBulletGameObjects;
	protected override void Start()
	{
		for (int i = 0; i < transform.childCount; i++)
		{
			if (transform.GetChild(i).name == "hoverslug01Mesh")
			{
				var temp = transform.GetChild(i);
				for (int j = 0; j < temp.childCount; j++)
				{
					if(temp.GetChild(j).tag == "EquippedGun")
					{
						gunModel = temp.GetChild(j).gameObject;
						break;
					}
				}
			}
		}

        // TODO remove hacky solution to get refactoring working.
        messageDispatcher.DispatchMessage(new Telegram(new MachineGun(), null, true));

        Character.AddWeapon((RangeWeaponBase)slugGun);
        Character.Equip(slugGun);
		gun = gameObject.GetComponentInChildren<Gun>();
		base.Start();
	}

	protected override void Update()
	{
		gun.Rotate(Vector3.Dot(comparisor, Vector3.left));

		if (slugGun.FireRate(Time.deltaTime))
		{
            
			IProjectile bullet = slugGun.Fire();
            bullet.ShotDirection = -gun.transform.forward;
			var bulletInstance = PooledBulletGameObjects.GetPooledBullet(Character).GetComponent<Bullet>();
			bulletInstance.Projectile = bullet;
			bulletInstance.Initialize();
			messageDispatcher.DispatchMessage(new Telegram(bulletInstance, gunModel.transform));
		}
		base.Update();
	}
}
