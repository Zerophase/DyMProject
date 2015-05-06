﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Weapons;
using Assets.Scripts.Weapons.Bases;
using Assets.Scripts.Weapons.Guns;
using Assets.Scripts.Weapons.Interfaces;
using ModestTree.Zenject;

namespace Assets.Scripts.DependencyInjection
{
	public class WeaponPickUpFactory
	{
		private Instantiator instantiator;
		[Inject]
		private RangeWeaponFactory rangeWeaponFactory;

		[Inject] private IMeleeWeapon meleeWeapon;

		public WeaponPickUpFactory(Instantiator instantiator)
		{
			this.instantiator = instantiator;
		}

		public WeaponPickUp Create(WeaponTypes weaponTypes)
		{
			switch (weaponTypes)
			{
				case WeaponTypes.MACHINE_GUN:
					return instantiator.Instantiate<WeaponPickUp>(
						rangeWeaponFactory.Create(weaponTypes));
				case WeaponTypes.FORK_LIGHTNING:
					return instantiator.Instantiate<WeaponPickUp>(
						rangeWeaponFactory.Create(weaponTypes));
				case WeaponTypes.SLUG_GUN:
					return instantiator.Instantiate<WeaponPickUp>(
						rangeWeaponFactory.Create(weaponTypes));
				case WeaponTypes.SWORD:
					return instantiator.Instantiate<WeaponPickUp>(meleeWeapon);
			}

			return null;
		}
	}
}
