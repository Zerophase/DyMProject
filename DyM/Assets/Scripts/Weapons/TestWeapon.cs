using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Projectiles.Interfaces;
using Assets.Scripts.Weapons.Interfaces;

namespace Assets.Scripts.Weapons
{
	public abstract class BaseWeapon : IWeapon, IComparable<BaseWeapon>
	{
		private int order;

		public int Order
		{
			get { return order; }
		}

		private IProjectile projectile;

		public BaseWeapon(int order)
		{
			setOrder(order);
		}

		public BaseWeapon(int order, IProjectile projectile)
		{
			setOrder(order);
			setProjectile(projectile);
		}

		private void setProjectile(IProjectile projectile)
		{
			this.projectile = projectile;
		}

		private void setOrder(int order)
		{
			this.order = order;
		}

		public int CompareTo(BaseWeapon other)
		{
			return this.Order.CompareTo(other.Order);
		}

		public IProjectile Fire()
		{
			return projectile;
		}
	}

	public class TestWeapon : BaseWeapon
	{
		
		public TestWeapon() : base(0)
		{
		}

		public TestWeapon(IProjectile projectile) : base(0, projectile)
		{
		}
	}

	public class TestWeapon2 : BaseWeapon
	{
		public TestWeapon2() : base(1)
		{
		}
	}

	public class TestWeapon3 : BaseWeapon
	{
		public TestWeapon3() : base(2)
		{
		}
	}
}
