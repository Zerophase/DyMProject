﻿using Assets.Scripts.Character;
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

		Character.CharacterType = CharacterTypes.ENEMY;
        Character.AddWeapon((RangeWeaponBase)slugGun);
        Character.Equip(slugGun);
		gun = gameObject.GetComponentInChildren<Gun>();

		
		base.Start();
	}

	private IProjectile bullet;
	private Bullet bulletInstance;
	private Telegram telegram = new Telegram();
	protected override void Update()
	{
		gun.Rotate(Vector3.Dot(comparisor, Vector3.left));

		if (slugGun.FireRate(Time.deltaTime))
		{

			bullet = slugGun.Fire();
			bullet.CharacterType = CharacterTypes.PLAYER;
			bulletInstance = PooledBulletGameObjects.GetPooledBullet(Character);
			bulletInstance.Projectile = bullet;
			telegram.Receiver = bulletInstance;
			telegram.Message = gunModel;
			messageDispatcher.DispatchMessage(telegram);
		}
		base.Update();
	}
}
