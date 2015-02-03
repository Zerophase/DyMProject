using Assets.Scripts.CameraControl;
using Assets.Scripts.CameraControl.Interfaces;
using Assets.Scripts.Utilities.CustomEditor;
using ModestTree.Zenject;
using UnityEngine;
using System.Collections;
using Assets.Scripts.GameObjects;
using Assets.Scripts.Utilities;

namespace Assets.Scripts.GameObjects
{
    public class CameraControl : MonoBehaviour
    {
        [Inject] private ICameraLogic camera;

        private Vector3 cameraCurrentPosition;

        public GameObject player;
        public float cameraSpeed = 0f;
        public int XBoundary = 5;
        public int YBoundary = 3;

        public Vector3 screenBounds;
        public Vector3 screenCenter;
        public Vector3 playerPosOnScreen;

        [ExposeProperty]
        public float Speed
        {
            get
            {
                if (Application.isPlaying)
                    return camera.Speed;
                return 0f;
            }
            set
            {
                if (Application.isPlaying)
                    camera.Speed = value;
            }
        }

        private void Start()
        {
            camera.OriginPosition = transform.position;
            
        }

        private void Update()
        { 
            Vector3 playerPosOnScreen = Camera.main.WorldToScreenPoint(player.transform.position);
            Vector3 playerVelocity = player.GetComponent<Player>().velocity;
            Vector3 tempVector = Vector3.zero;
            tempVector = camera.Move(player.transform.position, playerVelocity, transform.position,
                Time.deltaTime);
            transform.localPosition = new Vector3(tempVector.x, tempVector.y, camera.OriginPosition.z);
            Debug.Log("Camera Pos: " + transform.localPosition);
            //moveCamera(checkDistance());


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

    }

    //private bool checkDistance()
    //{
    //    Vector3 playerVelocity = player.GetComponent<Player>().velocity;
    //    float displacementX, displacementY;
    //    if(player != null)
    //    {
    //        displacementX = transform.position.x - player.transform.position.x;
    //        displacementY = transform.position.y - player.transform.position.y;
    //        if(displacementX > 25f && playerVelocity.checkDirection() == 1)
    //        {
    //            return false;
    //        }
    //        if(displacementX < -30f && playerVelocity.checkDirection() == -1)
    //        {
    //            return false;
    //        }
    //        return true;
    //    }
    //    return false;
    //}

    //private void moveCamera(bool cameraAtBoundary)
    //{
    //    Vector3 playerVelocity = player.GetComponent<Player>().velocity;
    //    float tempX, tempY;
    //    if(cameraAtBoundary == false)
    //    {
    //        tempX = transform.position.x + playerVelocity.x*.75f*Time.deltaTime;
    //        tempY = transform.position.y + playerVelocity.y*0.145f*Time.deltaTime;
    //    }
    //    else
    //    {
    //        tempX = transform.position.x + playerVelocity.x;
    //        tempY = transform.position.y + playerVelocity.y;
    //    }
    //    Vector3 newPosition = new Vector3(tempX,tempY,camera.OriginPosition.z);
    //    Camera.main.transform.position = newPosition;
    //}
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