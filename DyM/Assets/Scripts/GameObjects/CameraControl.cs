using Assets.Scripts.CameraControl;
using Assets.Scripts.CameraControl.Interfaces;
using ModestTree.Zenject;
using UnityEngine;
using System.Collections;

namespace Assets.Scripts.GameObjects
{
	public class CameraControl : MonoBehaviour
	{
		[Inject]
		private ICameraLogic camera;
        public float cameraSpeed = 0f;

        private Vector3 cameraCurrentPosition;
		private Vector3 displaceMentVector;

		private const float cameraMidOffset = 2f;

        public Transform player;
        public int XBoundary = 5;
        public int YBoundary = 3;

		private void Start()
		{
			camera.OriginPosition = transform.position;
     	}

		private void Update()
		{
			//Debug.Log("Right Stick Magnitude: " + new Vector2(Input.GetAxis("CameraHorizontalMovement"),
			//		Input.GetAxis("CameraVerticalMovement")).magnitude);

            //As long as the player is within the box set in the check function,
            //the camera should remain still.
            //HOWEVER, since the camera is attached to the parent character game object,
            //Unity moves the camera regardless.
            if(!checkCenterBoundary())
            {
				//Debug.Log("Camera should be still");
            }
            else
            {
                FollowPlayer();                
            }

            //Debug.Log("Boundary Check: " + checkCenterBoundary());
            //Debug.Log("Character Position: " + new Vector3(transform.parent.position.x, transform.parent.position.y));
            //transform.localPosition = camera.Move(
            //    new Vector2(Input.GetAxis("CameraHorizontalMovement"),
            //        Input.GetAxis("CameraVerticalMovement")),
            //    transform.localPosition, Time.deltaTime);


		}

        private bool checkCenterBoundary()
        {
            float displacementX, displacementY;

            //Sets the current player position
            cameraCurrentPosition = new Vector3(transform.position.x, transform.position.y, camera.OriginPosition.z);

            //Finds the distance between the camera and player on both axes.
            displacementX = cameraCurrentPosition.x - player.position.x;
	        displacementY = cameraCurrentPosition.y - (player.position.y + cameraMidOffset);
            displaceMentVector = new Vector3(displacementX, displacementY, camera.OriginPosition.z);

            return (Mathf.Abs(displacementX) > XBoundary || Mathf.Abs(displacementY) > YBoundary);
        }

        private void FollowPlayer()
        {
	        Camera.main.transform.position = Vector3.Lerp(cameraCurrentPosition, cameraCurrentPosition - displaceMentVector,
		        Time.deltaTime);
				//new Vector3(player.position.x, player.position.y, camera.OriginPosition.z);
        }

	}
}
