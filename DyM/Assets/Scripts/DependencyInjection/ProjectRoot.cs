using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Abilities;
using Assets.Scripts.Abilities.PlayerSkills;
using Assets.Scripts.Abilities.Interfaces;
using Assets.Scripts.CameraControl;
using Assets.Scripts.CameraControl.Interfaces;
using Assets.Scripts.Character;
using Assets.Scripts.Character.Interfaces;
using Assets.Scripts.GameObjects;
using Assets.Scripts.ObjectManipulation;
using Assets.Scripts.ObjectManipulation.Interfaces;
using Assets.Scripts.Projectiles;
using Assets.Scripts.Projectiles.Interfaces;
using Assets.Scripts.Projectiles.Projectiles;
using Assets.Scripts.Utilities.Messaging;
using Assets.Scripts.Utilities.Messaging.Interfaces;
using Assets.Scripts.Weapons;
using Assets.Scripts.Weapons.Guns;
using Assets.Scripts.Weapons.Interfaces;
using ModestTree.Zenject;
using UnityEngine;

namespace Assets.Scripts.DependencyInjection
{
	public class ProjectRoot : MonoInstaller
	{
		public static GameObjectInstantiator Instantiator;
		public override void InstallBindings()
		{
			dependencyFrameworkBindings();
			 
			factoryBindings();

			movementBindings();
			
			cameraBindings();

			messengerBindings();

			rangeWeaponBindings();
			meleeWeaponBindings();

			characterBindings();

			abilityBindings();

			Instantiator = new GameObjectInstantiator(_container);
		}

		private void abilityBindings()
		{
			_container.Bind<IAbility>().ToTransient<NullAbiliity>();
		}

		private void rangeWeaponBindings()
		{
			_container.Bind<IProjectile>().ToTransient<MachineGunProjectile>().
				WhenInjectedInto<MachineGun>();
			_container.Bind<IProjectile>().ToTransient<LightningGunProjectile>().
				WhenInjectedInto<ForkLightningGun>();
			_container.Bind<IBulletPool>().ToSingle<BulletPool>();
			_container.Bind<IPooledGameObjects>().ToSingle<PooledGameobjects>();
			_container.Bind<IPickUp>().ToTransient<WeaponPickUp>();

			_container.Bind<IRangeWeapon>().ToTransient<SlugGun>();
			_container.Bind<IProjectile>().ToTransient<LightningGunProjectile>().WhenInjectedInto<SlugGun>();
		}

		private void meleeWeaponBindings()
		{
			_container.Bind<IMeleeWeapon>().ToTransient<TestMeleeWeapon>();
		}

		private void characterBindings()
		{
			_container.Bind<ICharacter>().ToTransient<PlayerCharacter>().
				WhenInjectedInto<Player>();
			_container.Bind<ICharacter>().ToTransient<EnemyCharacter>().
				WhenInjectedInto<Slug>();
		}

		private void messengerBindings()
		{
            _container.Bind<IIds>().ToTransient<Ids>();
            _container.Bind<IEntityManager>().ToSingle<EntityManager>();
			_container.Bind<IMessageDispatcher>().ToSingle<MessageDispatcher>();
			_container.Bind<IReceiver>().ToTransient<Receiver>();
			
			_container.Instantiate<Ids>().ResetId();
		}

		private void cameraBindings()
		{
			_container.Bind<ICameraLogic>().ToTransient<CameraLogic>();
		}

		private void movementBindings()
		{
			_container.Bind<ICardinalMovement>().ToTransient<CardinalMovement>();
		}

		private void factoryBindings()
		{
			_container.Bind<PlaneShiftFactory>().ToSingle();
			_container.Bind<RangeWeaponFactory>().ToSingle();
			_container.Bind<ProjectileFactory>().ToSingle();
			_container.Bind<PooledProjectileFactory>().ToSingle();
			_container.Bind<WeaponPickUpFactory>().ToSingle();

			_container.Bind<AbilityFactory>().ToSingle();
			_container.Bind<AbilityPickUpFactory>().ToSingle();
		}

		private void dependencyFrameworkBindings()
		{
			_container.Bind<IInstaller>().ToSingle<StandardUnityInstaller>();
			_container.Bind<IDependencyRoot>().ToSingle<DependencyRootStandard>();
		}
	}
}
