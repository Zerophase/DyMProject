using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Projectiles.Interfaces
{
	public interface IBulletPool
	{
		void Initialize(Vector3 startPosition, int count);
		IPooledProjectile Projectile { set; }
		List<IPooledProjectile> Projectiles { get; }
		IPooledProjectile GetPooledProjectile();
		void DeactivatePooledProjectile(IPooledProjectile projectile);
	}
}
