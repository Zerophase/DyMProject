using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Character.Interfaces;
using Assets.Scripts.Projectiles;
using Assets.Scripts.Projectiles.Interfaces;
using Assets.Scripts.Utilities.Messaging;
using Assets.Scripts.Utilities.Messaging.Interfaces;
using Assets.Scripts.Weapons.Interfaces;
using ModestTree.Zenject;
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

		private IBulletPool bulletPool;

		private IReceiver receiver;

		public IReceiver Receiver
		{
			set { receiver = value; }
		}

		private IMessageDispatcher messageDispatcher;

		public Vector3 Position { get; set; }

		public BaseWeapon(int order)
		{
			setOrder(order);
		}

		public BaseWeapon(IReceiver receiver, IMessageDispatcher messageDispatcher,
			IBulletPool bulletPool = null)
		{
			this.receiver = receiver;
			this.receiver.Owner = this;
			this.messageDispatcher = messageDispatcher;
			this.bulletPool = bulletPool;
			bulletPool.Initialize(new Vector3(0f, 0f, 0f), 50);

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
			return bulletPool.GetPooledProjectile().Projectile;
		}

		public void PickUp(ICharacter character)
		{
			messageDispatcher.DispatchMessage(new Telegram(character, this));
		}

		public void Receive(ITelegram telegram)
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

		[Inject]
		public TestWeapon(IReceiver receiver, IMessageDispatcher messageDispatcher,
			IBulletPool bulletPool = null) 
			: base(receiver, messageDispatcher, bulletPool)
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
