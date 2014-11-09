using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Projectiles.Interfaces;
using Assets.Scripts.Projectiles.Projectiles;
using ModestTree.Zenject;

namespace Assets.Scripts.DependencyInjection
{
	public class ProjectileFactory
	{
		private IFactory<IProjectile> factory;
		private Instantiator instantiator;

		public ProjectileFactory(Instantiator instantiator)
		{
			this.instantiator = instantiator;
		}

		public IProjectile Create(IProjectile projectile)
		{
			// TODO figure out how to replace, this is slow, but only gets hit at startup.
			return (IProjectile)Activator.CreateInstance(projectile.GetType());
		}
	}
}
