using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Projectiles.Interfaces;
using Assets.Scripts.Utilities;
using UnityEngine;

namespace Assets.Scripts.Projectiles.Projectiles
{
	public class MachineGunProjectile : ProjectileBase
	{
		public MachineGunProjectile() :
			base("MachineGunBullet", "Sphere", 30f)
		{
			
		}

		public override Vector3 ProjectilePattern()
		{
			return PhysicsFuncts.calculateVelocity(
				speed*fireDirection, Time.deltaTime);
		}
	}
}
