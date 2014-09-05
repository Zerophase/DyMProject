using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Projectiles.Interfaces;

namespace Assets.Scripts.Weapons.Interfaces
{
	public interface IWeapon
	{
		IProjectile Fire();
	}
}
