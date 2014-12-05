﻿using Assets.Scripts.CameraControl;
using Assets.Scripts.CameraControl.Interfaces;
using Assets.Scripts.Utilities.CustomEditor;
using ModestTree.Zenject;
using UnityEngine;
using System.Collections;
using Assets.Scripts.GameObjects;

namespace Assets.Scripts.GameObjects
{
	public class CameraControl : MonoBehaviour
	{
		[Inject]
		private ICameraLogic camera;

        private Vector3 cameraCurrentPosition;

        public GameObject player;
		public float cameraSpeed = 0f;
        public int XBoundary = 5;
        public int YBoundary = 3;

		[ExposeProperty]
		public float Speed
		{
			get
			{
				if(Application.isPlaying)
					return camera.Speed;
				return 0f;
			}
			set
			{
				if(Application.isPlaying)
					camera.Speed = value;
			}
		}

		private void Start()
		{
			camera.OriginPosition = transform.position;
     	}

		private void Update()
		{
		
		if(checkDistance())
		{
			moveCamera();
		}
		
		//Old camera style 12/5/14
//			checkCenterBoundary();
//         	FollowPlayer();     

            //Debug.Log("Boundary Check: " + checkCenterBoundary());
            //Debug.Log("Character Position: " + new Vector3(transform.parent.position.x, transform.parent.position.y));
            //transform.localPosition = camera.Move(
            //    new Vector2(Input.GetAxis("CameraHorizontalMovement"),
            //        Input.GetAxis("CameraVerticalMovement")),
            //    transform.localPosition, Time.deltaTime);


		}
		private bool checkDistance()
		{
			float displacementX, displacementY;
			if(player != null)
			{
				displacementX = transform.position.x - player.transform.position.x;
				displacementY = transform.position.y - player.transform.position.y;
				if(displacementX > 30f)
				{
					return false;
				}
				if(displacementX < -30f)
				{
					return false;
				}
				return true;
			}
			return false;
		}
		
		private void moveCamera()
		{
			Vector3 playerVelocity = player.GetComponent<Player>().velocity;
			float tempX = transform.position.x + playerVelocity.x*0.8f*Time.deltaTime*2.5f;
			float tempY = transform.position.y + playerVelocity.y*0.145f*Time.deltaTime*2.5f;
			Vector3 newPosition = new Vector3(tempX,tempY,camera.OriginPosition.z);
			Camera.main.transform.position = newPosition;
		}
	//Old Camera Style 12/5/14
//        private bool checkCenterBoundary()
//        {
//            float displacementX, displacementY;
//
//	        if (player != null)
//	        {
//				//Sets the current player position
//				transform.position = new Vector3(transform.position.x, transform.position.y, camera.OriginPosition.z);
//
//				//Finds the distance between the camera and player on both axes.
//				displacementX = cameraCurrentPosition.x - player.position.x;
//				displacementY = cameraCurrentPosition.y - (player.position.y + cameraMidOffset);
//				displaceMentVector = new Vector3(displacementX, displacementY, camera.OriginPosition.z);
//
//				return (Mathf.Abs(displacementX) > XBoundary || Mathf.Abs(displacementY) > YBoundary);
//	        }
//	        return false;
//        }
//
//        private void FollowPlayer()
//        {
//			Vector3 temp = cameraCurrentPosition - displaceMentVector;
//			temp.z = cameraCurrentPosition.z;
//
//	        Camera.main.transform.position = Vector3.Lerp(cameraCurrentPosition, temp,
//		        Time.deltaTime*2.5f);
//				//new Vector3(player.position.x, player.position.y, camera.OriginPosition.z);
//        }

	}
}
