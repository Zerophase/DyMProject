using System;
using Assets.Scripts.CameraControl.Interfaces;
using Assets.Scripts.Character;
using Assets.Scripts.GameObjects;
using Assets.Scripts.Utilities;
using ModestTree.Zenject;
using UnityEngine;

namespace Assets.Scripts.CameraControl
{
	public class CameraLogic : ICameraLogic
	{
		private float speed = 5f;
		public float Speed
		{
			get { return speed; }
			set { speed = value; }
		}

		private float totalTime = 0f;

		private const float deadzone = 0.30f;

		private Vector3 originPosition;
		private Vector3 cameraFuturePosition;
        
		private Vector3 previousPlayerPosition = new Vector3(0f,0f,0f);
        
		private float debugTimer = 0f;

        private Vector3 screenBounds = new Vector3(Screen.width, Screen.height, 0);
	    private Vector3 screenCenter;

		public Vector3 OriginPosition
		{
			get { return originPosition; }
			set { originPosition = value; }
		}

		public CameraLogic()
		{
            screenCenter = new Vector3(screenBounds.x / 2, screenBounds.y / 2, 0);  
		}

		public Vector3 Move(Vector3 playerPosition, Vector3 playerVelocity, Vector3 cameraPosition, float time)
		{
		    Debug.Log("Idle? " + checkForIdle(playerPosition));

			if (checkForIdle(playerPosition))
			{
				return bounceCameraBack(cameraPosition, playerPosition, time);
			}
			else
			{
			    return moveCamera(cameraPosition, playerVelocity, time);
			}
		}

	    private Vector3 moveCamera(Vector3 cameraPos, Vector3 playerVelocity, float time)
	    {
	        Vector3 newPosition = Vector3.zero;
	        newPosition = cameraPos + playerVelocity*time;
	        newPosition = newPosition*1.5f;
	        return newPosition;
	    }

		private Vector3 bounceCameraBack(Vector3 cameraPosition, Vector3 playerPosition, float time)
		{
            return playerPosition;
		}

		private bool checkForIdle(Vector3 position)
		{
            
            //Calculates difference in position from previous to current frame.
		    float tempX = Mathf.Abs(position.x - previousPlayerPosition.x);
            float tempY = Mathf.Abs(position.y - previousPlayerPosition.y);
            float tempZ = Mathf.Abs(position.z - previousPlayerPosition.z);

            //If idle, all differences are 0. Leaving z out for the time being.
		    bool isIdle = tempX == 0 && tempY == 0;

            //Sets the current frame to previous position.
            previousPlayerPosition = new Vector3(position.x, position.y, position.z);

            //Return result.
		    return isIdle;
		}


		
	}
}
