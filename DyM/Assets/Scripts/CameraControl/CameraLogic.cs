﻿using System;
using Assets.Scripts.CameraControl.Interfaces;
using Assets.Scripts.Utilities;
using ModestTree.Zenject;
using UnityEngine;

namespace Assets.Scripts.CameraControl
{
	public class CameraLogic : ICameraLogic
	{
		private float speed = 5f;
		private float totalTime = 0f;

		private const float deadzone = 0.250f;

		private Vector3 originPosition;
		private Vector3 cameraFuturePosition;

		private Vector2 previousStickPosition = new Vector2(0f, 0f);

		private const float maxPositionX = 10f;
		private const float maxPositionYDown = -.5f;
		private const float maxPositionYUp = 2f;

		private Vector3EqualityComparerWithTolerance vector3EqualityComparer =
			new Vector3EqualityComparerWithTolerance();

		private float debugTimer = 0f;

		public Vector3 OriginPosition
		{
			set { originPosition = value; }
		}

		public CameraLogic()
		{
		}

		public Vector3 Move(Vector2 position, Vector3 cameraPosition, float time)
		{
			if (checkForDeadZone(position))
			{
				return bounceCameraBack(cameraPosition, time);
			}
			else
			{
				
				position = lockToAxis(position);
				return move(position, cameraPosition, time);
			}
		}

		private Vector3 move(Vector2 position, Vector3 cameraPosition, float time)
		{
			calculateCameraFuturePosition(position, cameraPosition);

			cameraFuturePosition.x = cameraFuturePosition.x.Clamp(-maxPositionX, maxPositionX);
			cameraFuturePosition.y = cameraFuturePosition.y.Clamp(maxPositionYDown, maxPositionYUp);

			return Vector3.Lerp(cameraPosition, cameraFuturePosition, speed * time);
			
		}

		private void calculateCameraFuturePosition(Vector2 position, Vector3 cameraPosition)
		{
			if (!position.CompareVectors(previousStickPosition))
			{
				previousStickPosition = position;
				cameraFuturePosition =
					new Vector3(cameraPosition.x + position.x*speed,
						cameraPosition.y - position.y*speed, cameraPosition.z);
			}
		}

		private Vector2 lockToAxis(Vector2 position)
		{
			//if (Math.Abs(position.x) < deadzone)
			//	position.x = 0f;
			//else if (Math.Abs(position.y) < deadzone)
			//	position.y = 0f;

			position = position.normalized*((position.magnitude - deadzone)/(1 - deadzone));

			debugTimer += Time.deltaTime;
			if (debugTimer > 1f)
			{
				debugTimer = 0f;
				Debug.Log("Current right stick position: " + position);
			}
			
			return position;
		}

		private Vector3 bounceCameraBack(Vector3 cameraPosition, float time)
		{
			return Vector3.Lerp(cameraPosition, originPosition, (speed / 2) * time);
		}

		private bool checkForDeadZone(Vector2 position)
		{
			return position.magnitude < deadzone;
		}
	}
}
