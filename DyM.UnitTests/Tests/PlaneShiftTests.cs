using Assets.Scripts.ObjectManipulation;
using Assets.Scripts.ObjectManipulation.Interfaces;
using DyM.UnitTests.Tests.BaseTest;
using NUnit.Framework;
using UnityEngine;

namespace DyM.UnitTests.Tests
{
	[TestFixture]
	public class PlaneShiftTests : CommonVector3TestProperties
	{
		private IPlaneShift createPlaneShift()
		{
			return new PlaneShift();
		}

		private Vector3 simulatesMultiplePlaneShifts(Vector3 currentPlane, IPlaneShift planeShift, KeyCode buttonPressed,
			int timesButtonPressed)
		{
			for (int i = 0; i < timesButtonPressed; i++)
			{
				currentPlane += planeShift.ShiftPlane(buttonPressed, currentPlane);
			}
			return currentPlane;
		}

		private static object[] planeShiftCases =
		{
			new object[] { new Vector3(0f, 0f, 1f), KeyCode.Joystick1Button4 },
			new object[] { new Vector3(0f, 0f, -1f), KeyCode.Joystick1Button5 },
			
			// KeyCode.None might not be the same as no keypress event firing.
			// Also might not be neccesary with how the code is used in a Monobehaviour.
			// KeyPress is checked for first, and then the KeyCode gets passed in.
			new object[] { Vector3.zero, KeyCode.None }
		};

		[Test, TestCaseSource("planeShiftCases")]
		public void SwitchPlanes_HandlesPlaneShiftsCorrectly_ReturnsRightPlaneBasedOnButton(Vector3 expected,
			KeyCode buttonPressed)
		{
			IPlaneShift planeShift = createPlaneShift();
			Vector3 currentPlane = Vector3.zero;

			Vector3 actual = planeShift.ShiftPlane(buttonPressed, currentPlane);

			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void SwitchPlane_PlaneShiftUpButtonPressedThreeTime_ReturnsNoMoreThanMaxPlane()
		{
			IPlaneShift planeShift = createPlaneShift();
			Vector3 currentPlane = new Vector3(0f, 0f, -1f);

			Vector3 expected = new Vector3(0f, 0f, 1f);
			Vector3 actual = simulatesMultiplePlaneShifts(currentPlane, planeShift, KeyCode.Joystick1Button4, 3);

			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void SwitchPlane_PlaneShiftDownButtonPressedThreeTimes_ReturnsNoMoreThanMaxPlane()
		{
			IPlaneShift planeShift = createPlaneShift();
			Vector3 currentPlane = new Vector3(0f, 0f, 1f);

			Vector3 expected = new Vector3(0f, 0f, -1f);
			Vector3 actual = simulatesMultiplePlaneShifts(currentPlane, planeShift, KeyCode.Joystick1Button5, 3);

			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void SwitchPlane_ShiftPlanePositionStandardizedForCurrentPlayerPosition_ReturnsCorrectUpValue()
		{
			Vector3 currentPlane = new Vector3(0f, 0f, 1f);
			IPlaneShift planeShift = new PlaneShift(currentPlane);

			Vector3 expected = new Vector3(0f, 0f, 2f);
			Vector3 actual = simulatesMultiplePlaneShifts(currentPlane, planeShift, KeyCode.Joystick1Button4, 3);

			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void SwitchPlane_ShiftPlanePositionStandardizedForCurrentPlayePosition_ReturnsCorrectDownValue()
		{
			Vector3 currentPlane = new Vector3(0f, 0f, 1f);
			IPlaneShift planeShift = new PlaneShift(currentPlane);

			Vector3 expected = Vector3.zero;
			Vector3 actual = simulatesMultiplePlaneShifts(currentPlane, planeShift, KeyCode.Joystick1Button5, 3);

			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void SwitchPlane_DodgeOnShortPress_ShiftsPlaneUpAndGoBack()
		{
			Vector3 currentPlane = new Vector3(0f, 0f, 0f);
			IPlaneShift planeShift = new PlaneShift();

			Vector3 expected = new Vector3(0f, 0f, 0f);
			Vector3 actual = planeShift.ShiftPlane(KeyCode.Joystick1Button4, currentPlane);
			actual += planeShift.Dodge(actual, false, .1f);

			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void SwitchPlane_DodgeOnShortPress_ShiftsPlaneDownAndGoBack()
		{
			Vector3 currentPlane = Vector3.zero;
			IPlaneShift planeShift = new PlaneShift();

			Vector3 expected = Vector3.zero;
			Vector3 actual = planeShift.ShiftPlane(KeyCode.Joystick1Button5, currentPlane);
			actual += planeShift.Dodge(actual, false, .1f);

			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void SwitchPlane_StayOnNewPlaneWithLongButtonPress_ShiftsUpPlane()
		{
			Vector3 currentPlane = Vector3.zero;
			IPlaneShift planeShift = createPlaneShift();
			float keyHeldAboveTimedPeriod = .6f;
			float keyHoldTimerReset = 0f;

			Vector3 expected = new Vector3(0f, 0f, -1f);
			Vector3 actual = planeShift.ShiftPlane(KeyCode.Joystick1Button5, currentPlane);
			actual += planeShift.Dodge(actual, true, keyHeldAboveTimedPeriod);
			actual += planeShift.Dodge(actual, false, keyHoldTimerReset);
			actual += planeShift.Dodge(actual, true, .1f);
			actual += planeShift.Dodge(actual, false, .1f);

			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void SwitchPlane_StayOnNewPlaneWIthLongButtonPress_ShiftsDownPlane()
		{
			Vector3 currentPlane = Vector3.zero;
			IPlaneShift planeShift = createPlaneShift();
			float keyHeldAboveTimedPeriod = .6f;
			float keyHoldTimerReset = 0f;

			Vector3 expected = new Vector3(0f, 0f, 1f);
			Vector3 actual = planeShift.ShiftPlane(KeyCode.Joystick1Button4, currentPlane);
			actual += planeShift.Dodge(actual, true, keyHeldAboveTimedPeriod);
			actual += planeShift.Dodge(actual, false, keyHoldTimerReset);
			actual += planeShift.Dodge(actual, true, .1f);
			actual += planeShift.Dodge(actual, false, .1f);

			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void SwitchPlane_AdjustedForSpawnedPosition_NormalDownLimitsAccountedFor()
		{
			Vector3 currentPlane = new Vector3(0f, 0f, -1f);
			IPlaneShift planeShift = new PlaneShift(currentPlane, PlanePosition.DOWN);

			Vector3 expected = new Vector3(0f, 0f, -1f);
			Vector3 actual = currentPlane;
			actual += planeShift.ShiftPlane(KeyCode.Joystick1Button5, actual);

			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void SwitchPlane_AdjustedForSpawnedPosition_NormalUpLimitsAccountedFor()
		{
			Vector3 currentPlane = new Vector3(0f, 0f, -1f);
			IPlaneShift planeShift = new PlaneShift(currentPlane, PlanePosition.UP);

			Vector3 expected = new Vector3(0f, 0f, -1f);
			Vector3 actual = currentPlane;
			actual += planeShift.ShiftPlane(KeyCode.Joystick1Button4, actual);

			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void SwitchPlane_DodgeShiftsPlayerBackAfterShiftingToTerminal_ReturnsFormerPosition()
		{
			Vector3 currentPlane = new Vector3(0f, 0f, 0f);
			IPlaneShift planeShift = createPlaneShift();

			Vector3 expected = new Vector3(0f, 0f, 0f);
			Vector3 actual = planeShift.ShiftPlane(KeyCode.Joystick1Button4, currentPlane);
			actual += planeShift.Dodge(actual, true, .6f);
			actual += planeShift.ShiftPlane(KeyCode.Joystick1Button5, actual);
			actual += planeShift.Dodge(actual, true, .6f);
			actual += planeShift.ShiftPlane(KeyCode.Joystick1Button4, actual);
			actual += planeShift.Dodge(actual, true, .1f);
			actual += planeShift.Dodge(actual, false, .1f);

			Assert.AreEqual(expected, actual);
		}
	}
}
