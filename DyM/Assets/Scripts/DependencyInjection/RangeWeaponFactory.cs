using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Weapons;
using Assets.Scripts.Weapons.Bases;
using Assets.Scripts.Weapons.Guns;
using ModestTree.Zenject;

namespace Assets.Scripts.DependencyInjection
{
	public class RangeWeaponFactory
	{
		private IFactory<RangeWeaponBase> factory;
		private Instantiator instantiator;

		public RangeWeaponFactory(Instantiator instantiator)
		{
			this.instantiator = instantiator;
		}

		public RangeWeaponBase Create(WeaponTypes weaponTypes)
		{
			switch (weaponTypes)
			{
				case WeaponTypes.MACHINE_GUN:
					return instantiator.Instantiate<MachineGun>();
				case WeaponTypes.FORK_LIGHTNING:
					return instantiator.Instantiate<ForkLightningGun>();
				case WeaponTypes.SLUG_GUN:
					return instantiator.Instantiate<SlugGun>();
			}

			return null;
		}
	}
}
