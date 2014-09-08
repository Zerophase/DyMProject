using System;
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

		private const float deadzone = 0.50f;

		private Vector3 originPosition;
		private Vector3 cameraFuturePosition;

		private Vector2 previousStickPosition = new Vector2(0f, 0f);

        //private const float maxPositionX = 10f;
        //private const float maxPositionYDown = -.5f;
        //private const float maxPositionYUp = 2f;

        //FANCY DESIGNER CIRCLE VARIABLES
        private int maxRadius = 1;
        private double radius = 0f;

		private Vector3EqualityComparerWithTolerance vector3EqualityComparer =
			new Vector3EqualityComparerWithTolerance();

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
                //position = lockToAxis(position);
                double posXSquared = position.x * position.x;
                double posYSquared = position.y * position.y;
                radius = Math.Sqrt(posXSquared + posYSquared);
                if (Math.Abs(radius) < maxRadius)
                {
                    return move(position, cameraPosition, time);
                }
                return cameraPosition;
			}
		}

		private Vector3 move(Vector2 position, Vector3 cameraPosition, float time)
		{
			calculateCameraFuturePosition(position, cameraPosition);

			//cameraFuturePosition.x = cameraFuturePosition.x.Clamp(-maxPositionX, maxPositionX);
			//cameraFuturePosition.y = cameraFuturePosition.y.Clamp(maxPositionYDown, maxPositionYUp);

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

        //private Vector2 lockToAxis(Vector2 position)
        //{
        //    double posXSquared = position.x * position.x;
        //    double posYSquared = position.y * position.y;
        //    radius = Math.Sqrt(posXSquared + posYSquared);
        //    if(Math.Abs(radius) < 1)
        //        { 
        //            return position; 
        //        }
            
        //}
        //{
        //    if (Math.Abs(position.x) < deadzone)
        //        position.x = 0f;
        //    else if (Math.Abs(position.y) < deadzone)
        //        position.y = 0f;
        //    return position;
        //}

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
