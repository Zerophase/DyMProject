
using Assets.Scripts.DependencyInjection;
using Assets.Scripts.Projectiles.Projectiles;
using Assets.Scripts.Utilities;
using Assets.Scripts.Weapons.Interfaces;
using Assets.Scripts.Projectiles.Interfaces;
using Assets.Scripts.Utilities.Messaging.Interfaces;
using Assets.Scripts.Character.Interfaces;
using Assets.Scripts.Utilities.Messaging;
using System;
using ModestTree.Zenject;
using UnityEngine;

namespace Assets.Scripts.Weapons.Bases
{
	public abstract class RangeWeaponBase : IRangeWeapon, IComparable<RangeWeaponBase>
	{
		private int order;
		
		public int Order
		{
			get { return order; }
		}

		private float timeSinceLastShot;
		protected float fireRate;

		private IProjectile projectile;
		public IProjectile Projectile { get { return projectile; } }

		private IBulletPool bulletPool;
		
		private IReceiver receiver;
		
		public IReceiver Receiver
		{
			set { receiver = value; }
		}

        private ICharacter character;
        public ICharacter Character 
        { 
            get { return character; } 
            set 
            { 
                projectile.Character = character = value; 
            } 
        }
		private IMessageDispatcher messageDispatcher;
		
		public Vector3 Position { get; set; }

		public RangeWeaponBase()
		{
			receiver.SubScribe();
		}

		public RangeWeaponBase(int order)
		{
			setOrder(order);
		}
		
		public RangeWeaponBase(IReceiver receiver, IMessageDispatcher messageDispatcher,
		                       IProjectile projectile, IBulletPool bulletPool = null)
		{
			this.receiver = receiver;
			this.receiver.Owner = this;
			this.messageDispatcher = messageDispatcher;
			this.bulletPool = bulletPool;
			this.projectile = projectile;

			receiver.SubScribe();
		}
		public void PostCreation()
        {
            if (bulletPool.ProjectileBoundToCharacterType.Count == 0)
				bulletPool.Initialize(this, new Vector3(0f, 0f, 0f), 100);
        }
		public RangeWeaponBase(int order, IProjectile projectile)
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
		
		public int CompareTo(RangeWeaponBase other)
		{
			return this.Order.CompareTo(other.Order);
		}
		
		public IProjectile Fire()
		{
			var projectile = bulletPool.GetPooledProjectile(this).Projectile;
			projectile.Character = character;
			return projectile;
		}

		public virtual bool FireRate(float time)
		{
			bool fire;
			timeSinceLastShot += time;
			if (fireRate <= timeSinceLastShot)
			{
				timeSinceLastShot = 0f;
				fire = true;
			}
			else
			{
				fire = false;
			}

			return fire;
		}

		public void Receive(ITelegram telegram)
		{
            if (bulletPool.ProjectileBoundToCharacterType.Count == 0)
                bulletPool.Initialize(this, new Vector3(0f, 0f, 0f), 100);
		}
	}
}

