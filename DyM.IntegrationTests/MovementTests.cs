using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.ObjectManipulation.Interfaces;
using NUnit.Framework;
using Assets.Scripts.ObjectManipulation;
using UnityEngine;

namespace DyM.UnitTests
{
	/// <summary>
	/// Tests are broken need to update Acceleration values.
	/// </summary>
	[TestFixture]
    public class MovementTests
	{
		private float midSpeed = .5f;
		private float maxSpeed = .75f;

		private IMovement createMovement()
		{
			IPlaneShift planeShift = new PlaneShift();
			ICardinalMovement cardinalMovement = new CardinalMovement();
			return new Movement(planeShift, cardinalMovement);
		}

		private Vector3 simulateInput(Func<float, Vector3, float, Vector3> methodUnderTest, 
			float initialPosition, Vector3 acceleration, float movementModifier, int loops, 
			bool subtractFromMovementModifier = false)
		{
			Vector3 intermediateStep = new Vector3();
			for (int i = 0; i < loops; i++)
			{
				Vector3 preveIntermediateStep = intermediateStep;
				intermediateStep = methodUnderTest(initialPosition, acceleration, movementModifier);
				
				if (subtractFromMovementModifier && 
					preveIntermediateStep.y > intermediateStep.y)
					movementModifier -= .5f;
			}

			return intermediateStep;
		}

		#region MoveTests
		[Test]
		public void Move_MoveRight_AtMidSpeed()
		{
			IMovement movement = createMovement();
			float initialPos = 1f;
			Vector3 acceleration = Vector3.zero;

			Vector3 expected = new Vector3(midSpeed, 0f, 0f);
			Vector3 actual = simulateInput(movement.Move, initialPos, acceleration, .6f, 2);

			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void Move_MoveLeft_AtMidSpeed()
		{
			IMovement movement = createMovement();
			float initialPos = -1f;
			Vector3 acceleration = Vector3.zero;

			Vector3 expected = new Vector3(-midSpeed, 0f, 0f);
			Vector3 actual = simulateInput(movement.Move, initialPos, acceleration, .6f, 2);

			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void Move_MoveRight_AtMaxSpeed()
		{
			IMovement movement = createMovement();
			float initialPos = 1f;
			Vector3 acceleration = Vector3.zero;

			Vector3 expected = new Vector3(maxSpeed, 0f, 0f);
			Vector3 actual = simulateInput(movement.Move, initialPos, acceleration, .6f, 5);

			Assert.LessOrEqual(expected.y, actual.y);
		}

		[Test]
		public void Move_MoveLeft_AtMaxSpeed()
		{
			IMovement movement = createMovement();
			float initialPos = -1f;
			Vector3 acceleration = Vector3.zero;

			Vector3 expected = new Vector3(-maxSpeed, 0f, 0f);
			Vector3 actual = simulateInput(movement.Move, initialPos, acceleration, .6f, 5);

			Assert.GreaterOrEqual(expected.y, actual.y);
		}

		[Test]
		public void Move_StandStill_ReturnsZero()
		{
			IMovement movement = createMovement();
			float initalPos = 1f;
			Vector3 acceleration = Vector3.zero;

			Vector3 expected = new Vector3(0f, 0f, 0f);
			Vector3 actual = movement.Move(initalPos, acceleration, .1f);
			actual = movement.Move(0f, acceleration, .1f);

			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void Jump_SmallJump_ReturnsUpwardsMovement()
		{
			IMovement movement = createMovement();
			float initialPos = 1f;

			Vector3 expected = new Vector3(0f, .5f, 0f);
			Vector3 actual = movement.Jump(true, 1f);

			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void Jump_Gravity_LowersPlayerToGround()
		{
			IMovement movement = createMovement();
			float initialPos = 1f;

			Vector3 expected = new Vector3(0f, 0f, 0f);
			Vector3 actual = movement.Jump(true, 1f);
			actual = movement.Jump(true, 3f);

			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void Jump_JumpMax_ReturnsApexJumpValue()
		{
			IMovement movement = createMovement();
			float initialPos = 1f;

			Vector3 expected = new Vector3(0f, 5f, 0f);
			Vector3 actual = Vector3.zero;//simulateInput(movement.Jump, initialPos, 1f, 10);

			Assert.AreEqual(expected, actual);
		}
		#endregion

		#region PlaneShiftTests
		[Test]
		public void SwitchPlanes_ShiftDownAPlane_ReturnsLowerPlane()
		{
			IMovement movement = createMovement();
			Vector3 currentPlane = Vector3.zero;

			Vector3 expected = new Vector3(0f, 0f, 1f);
			Vector3 actual = movement.ShiftPlane(KeyCode.Joystick1Button4, currentPlane);

			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void SwitchPlane_ShiftUpAPlane_ReturnsUpperPlane()
		{
			IMovement movement = createMovement();
			Vector3 currentPlane = Vector3.zero;

			Vector3 expected = new Vector3(0f, 0f, -1f);
			Vector3 actual = movement.ShiftPlane(KeyCode.Joystick1Button5, currentPlane);

			Assert.AreEqual(expected, actual);
		}

		/// <summary>
		/// KeyCode.None might not be the same as no keypress event firing.
		/// Also might not be neccesary with how the code is used in a Monobehaviour.
		/// KeyPress is checked for first, and then the KeyCode gets passed in.
		/// </summary>
		[Test]
		public void SwitchPlane_StayOnSamePlaneWhenNoInput_ReturnsZero()
		{
			IMovement movement = createMovement();
			Vector3 currentPlane = Vector3.zero;

			Vector3 expected = Vector3.zero;
			Vector3 actual = movement.ShiftPlane(KeyCode.None, currentPlane);

			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void SwitchPlane_PlaneShiftUpButtonPressedThreeTime_ReturnsNoMoreThanMaxPlane()
		{
			IMovement movement = createMovement();
			Vector3 currentPlane = new Vector3(0f, 0f, -1f);

			Vector3 expected = new Vector3(0f, 0f, 1f);
			for (int i = 0; i < 3; i++)
			{
				currentPlane += movement.ShiftPlane(KeyCode.Joystick1Button4, currentPlane);
			}
			Vector3 actual = currentPlane;

			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void SwitchPlane_PlaneShiftDownButtonPressedThreeTimes_ReturnsNoMoreThanMaxPlane()
		{
			IMovement movement = createMovement();
			Vector3 currentPlane = new Vector3(0f, 0f, 1f);

			Vector3 expected = new Vector3(0f, 0f, -1f);
			for (int i = 0; i < 3; i++)
			{
				currentPlane += movement.ShiftPlane(KeyCode.Joystick1Button5, currentPlane);
			}
			Vector3 actual = currentPlane;

			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void SwitchPlane_ShiftPlanePositionStandardizedForCurrentPlayerPosition_ReturnsCorrectValue()
		{
			Vector3 currentPlane = new Vector3(0f, 0f, 1f);
			IMovement movement = new Movement(currentPlane);

			Vector3 expected = new Vector3(0f, 0f, 2f);
			for (int i = 0; i < 3; i++)
			{
				currentPlane += movement.ShiftPlane(KeyCode.Joystick1Button4, currentPlane);
			}
			Vector3 actual = currentPlane;

			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void SwitchPlane_DodgeOnShortPress_ShiftsPlaneUpAndGoBack()
		{
			Vector3 currentPlane = new Vector3(0f, 0f, 0f);
			IMovement movement = new Movement(new PlaneShift(), new CardinalMovement());

			Vector3 expected = new Vector3(0f, 0f, 0f);
			Vector3 actual = movement.ShiftPlane(KeyCode.Joystick1Button4, currentPlane);
			actual += movement.Dodge(actual, false, .1f);

			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void SwitchPlane_DodgeOnShortPress_ShiftsPlaneDownAndGoBack()
		{
			Vector3 currentPlane = Vector3.zero;
			IMovement movement = new Movement(new PlaneShift(), new CardinalMovement());

			Vector3 expected = Vector3.zero;
			Vector3 actual = movement.ShiftPlane(KeyCode.Joystick1Button5, currentPlane);
			actual += movement.Dodge(actual, false, .1f);

			Assert.AreEqual(expected, actual);
		}
		#endregion
	}
}
