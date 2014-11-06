using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Projectiles.Interfaces;
using ModestTree.Zenject;
using UnityEditor;

namespace Assets.Scripts.Projectiles
{
	//Projectile held by BulletPool
	public struct PooledProjectile : IPooledProjectile
	{
		private IProjectile projectile;
		private bool active;

		/// <summary>
		/// Set is for tests
		/// </summary>
		public IProjectile Projectile
		{
			get { return projectile; }
			set { projectile = value; }
		}

		/// <summary>
		/// Set is for tests
		/// </summary>
		public bool Active
		{
			get { return active; }
			set { active = value; }
		}

		[Inject]
		public PooledProjectile(IProjectile projectile)
		{
			this.projectile = projectile;
			this.active = false;
		}
	}
}
