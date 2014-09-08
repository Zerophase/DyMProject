using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Character.Interfaces;
using Assets.Scripts.Projectiles.Interfaces;
using Assets.Scripts.Utilities.Messaging;
using Assets.Scripts.Utilities.Messaging.Interfaces;
using Assets.Scripts.Weapons.Interfaces;
using UnityEngine;

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

		private IReceiver receiver;

		private IMessageDispatcher messageDispatcher;

		public Vector3 Position { get; set; }

		public BaseWeapon(int order)
		{
			setOrder(order);
		}

		public BaseWeapon(IReceiver receiver, IMessageDispatcher messageDispatcher)
		{
			this.receiver = receiver;
			this.receiver.Owner = this;
			this.messageDispatcher = messageDispatcher;
			receiver.SubScribe();
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


		public void PickUp(ICharacter character)
		{
			if(character.Position == this.Position)
				messageDispatcher.DispatchMessage(new Telegram(character, this));
		}


		public void Receive(Telegram telegram)
		{
			throw new NotImplementedException();
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

		public TestWeapon(IReceiver receiver, IMessageDispatcher messageDispatcher) 
			: base(receiver, messageDispatcher)
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
