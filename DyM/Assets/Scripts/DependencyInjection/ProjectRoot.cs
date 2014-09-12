using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.CameraControl;
using Assets.Scripts.CameraControl.Interfaces;
using Assets.Scripts.Character;
using Assets.Scripts.Character.Interfaces;
using Assets.Scripts.ObjectManipulation;
using Assets.Scripts.ObjectManipulation.Interfaces;
using Assets.Scripts.Projectiles;
using Assets.Scripts.Projectiles.Interfaces;
using Assets.Scripts.Utilities.Messaging;
using Assets.Scripts.Utilities.Messaging.Interfaces;
using Assets.Scripts.Weapons;
using Assets.Scripts.Weapons.Interfaces;
using ModestTree.Zenject;
using UnityEngine;

namespace Assets.Scripts.DependencyInjection
{
	public class ProjectRoot : MonoInstaller
	{
		public override void InstallBindings()
		{
			dependencyFrameworkBindings();
			movementBindings();
			cameraBindings();

			messengerBindings();

			characterBindings();
			
			weaponBindings();
		}

		private void weaponBindings()
		{
			_container.Bind<IWeapon>().ToTransient<TestWeapon>();
			_container.Bind<IProjectile>().ToTransient<Projectile>();
		}

		private void characterBindings()
		{
			_container.Bind<ICharacter>().ToTransient<TestCharacter>();
		}

		private void messengerBindings()
		{
			_container.Bind<IMessageDispatcher>().ToSingle<MessageDispatcher>();
			_container.Bind<IReceiver>().ToTransient<Receiver>();
		}

		private void cameraBindings()
		{
			_container.Bind<ICameraLogic>().ToTransient<CameraLogic>();
		}

		private void movementBindings()
		{
			_container.Bind<ICardinalMovement>().ToTransient<CardinalMovement>();
			_container.Bind<IPlaneShift>().ToTransient<PlaneShift>();
			_container.Bind<IMovement>().ToTransient<Movement>();
		}

		private void dependencyFrameworkBindings()
		{
			_container.Bind<IInstaller>().ToSingle<StandardUnityInstaller>();
			_container.Bind<IDependencyRoot>().ToSingle<DependencyRootStandard>();
		}
	}
}
