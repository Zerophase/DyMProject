using System;
using Assets.Scripts.ObjectManipulation;
using Assets.Scripts.ObjectManipulation.Interfaces;
using DyM.UnitTests.Tests.BaseTest;
using NUnit.Framework;
using UnityEngine;

namespace DyM.UnitTests.Tests
{
	[TestFixture]
	public class CardinalMovementTests : CommonVector3TestProperties
	{
		private ICardinalMovement createCardinalMovement()
		{
			return new CardinalMovement();
		}

		private Vector3 simulateInput(Func<float, float, Vector3> methodUnderTest,
			float initialPosition, float timeOrPlayPosition, int maxIterations,
			bool subtractFromMovementModifier = false)
		{
			Vector3 intermediateStep = new Vector3();
			for (int i = 0; i < maxIterations; i++)
			{
				Vector3 preveIntermediateStep = intermediateStep;
				intermediateStep = methodUnderTest(initialPosition, timeOrPlayPosition);

				if (subtractFromMovementModifier &&
					preveIntermediateStep.y > intermediateStep.y)
					timeOrPlayPosition -= .5f;
			}

			return intermediateStep;
		}

		private static float midSpeed = .5f;
		private static float maxSpeed = .75f;
		private static float initialPosition = 1f;
		private static float timeHeldDown = .6f;

		private static object[] cardinalDirectionCases =
		{
			new object[] { new Vector3(midSpeed, 0f, 0f), initialPosition, timeHeldDown, 2 },
			new object[] { new Vector3(-midSpeed, 0f, 0f), -initialPosition, timeHeldDown, 2},
			new object[] {new Vector3(maxSpeed, 0f, 0f), initialPosition, timeHeldDown, 5},
			new object[] { new Vector3(-maxSpeed, 0f, 0f), -initialPosition, timeHeldDown, 5}
		};

		[Test, TestCaseSource("cardinalDirectionCases")]
		public void Move_MoveHorizontally_AtCorrectSpeed(Vector3 expected, float initialPosition, 
			float time, int maxIterations)
		{
			ICardinalMovement cardinalMovement = createCardinalMovement();

			Vector3 actual = simulateInput(cardinalMovement.Move, initialPosition, time, maxIterations);

			Assert.That(actual, Is.EqualTo(expected).Using(vector3EqualityComparerWithTolerance));

		}

		[Test]
		public void Move_StandStill_ReturnsZero()
		{
			ICardinalMovement cardinalMovement = createCardinalMovement();

			Vector3 expected = new Vector3(0f, 0f, 0f);
			Vector3 actual = cardinalMovement.Move(initialPosition, .1f);
			actual = cardinalMovement.Move(0f, .1f);

			Assert.AreEqual(expected, actual);
		}

		private static object[] jumpHeightCases =
		{
			new object[] {new Vector3(0f, .5f, 0f), initialPosition, 1f, 1},
			new object[] {new Vector3(0f, 5f, 0f), initialPosition, 1f, 10}
		};

		[Test, TestCaseSource("jumpHeightCases")]
		public void Jump_DifferentHeightJumps_ReturnsCorrectUpwardsMovement(Vector3 expectedVector3,
			float initialPos, float playerPosition, int maxIterations)
		{
			ICardinalMovement cardinalMovement = createCardinalMovement();

			Vector3 actual = simulateInput(cardinalMovement.Jump, initialPosition, 
				playerPosition, maxIterations);

			Assert.AreEqual(expectedVector3, actual);
		}

		[Test]
		public void Jump_Gravity_LowersPlayerToGround()
		{
			ICardinalMovement cardinalMovement = createCardinalMovement();

			Vector3 expected = new Vector3(0f, 0f, 0f);
			Vector3 actual = cardinalMovement.Jump(initialPosition, 1f);
			actual = cardinalMovement.Jump(0f, 3f);

			Assert.AreEqual(expected, actual);
		}

		private Vector3 simulateJumpHeight(ICardinalMovement cardinalMovement,
			float buttonIsPressed, float playerPosition, int maxIterations)
		{
			Vector3 tempPlayerPos = new Vector3(0f, playerPosition, 0f);
			for (int i = 0; i < maxIterations; i++)
			{
				if (cardinalMovement.Falling)
					break;
				tempPlayerPos += cardinalMovement.Jump(buttonIsPressed, tempPlayerPos.y);
			}

			return tempPlayerPos;
		}

		// test is wrong
		[Test]
		public void Jump_JumpHeightRespectedWhenLessThanZero_ReturnsFiveMoreThanStartingPostion()
		{
			ICardinalMovement cardinalMovement = createCardinalMovement();
			Vector3 startPos = new Vector3(0f, -5f, 0f);
			float noKeyPressed = 0f;

			Vector3 expected = Vector3.zero;
			Vector3 actual = startPos;
			// Normalizes Max Jump based on players current position.
			actual += cardinalMovement.Jump(noKeyPressed, actual.y);
			actual = simulateJumpHeight(cardinalMovement, initialPosition, actual.y, 6);

			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void Jump_JumpHesightChangesWhenPlayerIsOnHigherGround_ReturnsFiveMoreThanPosition()
		{
			ICardinalMovement cardinalMovement = createCardinalMovement();
			Vector3 startPos = Vector3.zero;
			float noKeyPressed = 0f;

			Vector3 expected = new Vector3(0f, 10f, 0f);
			Vector3 actual = cardinalMovement.Jump(noKeyPressed, startPos.y);
			actual = simulateJumpHeight(cardinalMovement, initialPosition, actual.y, 6);
			cardinalMovement.Jump(noKeyPressed, actual.y);
			actual = simulateJumpHeight(cardinalMovement, initialPosition, actual.y, 6);

			Assert.AreEqual(expected, actual);
		}
	}
}
