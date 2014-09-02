using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.CameraControl;
using Assets.Scripts.CameraControl.Interfaces;
using DyM.UnitTests.Tests.BaseTest;
using NUnit.Framework;
using UnityEngine;

namespace DyM.UnitTests.Tests
{
	[TestFixture]
	public class CameraTests : CommonVector3TestProperties
	{
		private ICameraLogic createCameraLogic()
		{
			return new CameraLogic();
		}

		private object[] cameraInputCases =
		{
			new object[] { new Vector3(5f, 0f, 0f), new Vector2(1f, 0f) },
			new object[] { new Vector3(-5f, 0f, 0f), new Vector2(-1f, 0f) },
			new object[] { new Vector3(0f, -0.5f, 0f), new Vector2(0f, 1f) },
			new object[] { new Vector3(0f, 2f, 0f), new Vector2(0f, -1f) },
			new object[] { new Vector3(2.5f, 0f, 0f ), new Vector2(.5f, 0f) }
		};

		[TestCaseSource("cameraInputCases")]
		public void Move_MoveCameraHorizontally_ReturnsMaxCameraInfluence(
			Vector3 position, Vector2 direction)
		{
			ICameraLogic camera = createCameraLogic();
			Vector3 startPosition = Vector3.zero;

			Vector3 expected = position;
			Vector3 actual = camera.Move(direction, startPosition, 1f);

			Assert.That(actual, Is.EqualTo(expected).Using(vector3EqualityComparerWithTolerance));
		}

		[Test]
		public void Move_CameraBouncesBackToOrigin_ReturnsOriginPosition()
		{
			ICameraLogic camera = createCameraLogic();
			camera.OriginPosition = new Vector3(10f, 10f, 10f); // random values.
			Vector3 randomPositionToMoveTo = Vector3.zero;

			Vector3 expected = new Vector3(10f, 10f, 10f);
			Vector3 actual = camera.Move(new Vector2(1f, 1f), expected, 1f);
			actual = camera.Move(new Vector2(0f, 0f), randomPositionToMoveTo, 1f);

			Assert.That(actual, Is.EqualTo(expected).Using(vector3EqualityComparerWithTolerance));
		}

		// Only test movement to the right currently
		[Test]
		public void Move_CameraBounderiesRespected_CameraStopsOnBoundaries()
		{
			ICameraLogic camera = createCameraLogic();
			Vector3 startPosition = Vector3.zero;
			Vector2 stickPosition = new Vector2(1f, 0f);

			Vector3 expected = new Vector3(10f, 0f, 0f);
			Vector3 actual = startPosition;
			for (int i = 0; i < 3; i++)
			{
				actual = camera.Move(stickPosition, actual, 1f);
			}
			
			Assert.That(actual, Is.EqualTo(expected).Using(vector3EqualityComparerWithTolerance));
		}
	}
}
