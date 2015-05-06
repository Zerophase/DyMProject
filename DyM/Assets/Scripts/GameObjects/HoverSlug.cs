using Assets.Scripts.Character;
using Assets.Scripts.DependencyInjection;
using Assets.Scripts.GameObjects;
using Assets.Scripts.Projectiles;
using Assets.Scripts.Projectiles.Interfaces;
using Assets.Scripts.Utilities.Messaging;
using Assets.Scripts.Weapons;
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

	[Inject]
	private RangeWeaponFactory rangeWeaponFactory;

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


		var test = rangeWeaponFactory.Create(WeaponTypes.SLUG_GUN);
		test.Character = Character;
		messageDispatcher.DispatchMessage(new Telegram(test, null, true));

		Character.CharacterType = CharacterTypes.HOVERSLUG;
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
			messageDispatcher.DispatchMessage(new Telegram(bulletInstance, gunModel.transform));
		}
		base.Update();
	}
}
