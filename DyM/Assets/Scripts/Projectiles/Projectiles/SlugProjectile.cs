using UnityEngine;

namespace Assets.Scripts.Projectiles.Projectiles
{
	public class SlugProjectile : ProjectileBase
	{
		public SlugProjectile() :
			base("SlugProjectile", 30f, "EnemyBullet")
		{
			damage = 1;
		}

		public override Vector3 ProjectilePattern()
		{
			return speed*fireDirection;
		}
	}
}