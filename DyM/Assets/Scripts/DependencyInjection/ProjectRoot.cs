using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.CameraControl;
using Assets.Scripts.CameraControl.Interfaces;
using Assets.Scripts.ObjectManipulation;
using Assets.Scripts.ObjectManipulation.Interfaces;
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
