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

		private Vector3 cameraPositionUp;
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
			camera.OriginPosition = transform.localPosition;
			cameraPositionUp = camera.OriginPosition - player.transform.localPosition;
            
        }

        private void Update()
        { 
            Vector3 playerVelocity = player.GetComponent<Player>().velocity;
            Vector3 tempVector = Vector3.zero;
            tempVector = camera.Move(player.transform.localPosition, playerVelocity, transform.position,
                Time.deltaTime);
            transform.localPosition = new Vector3(tempVector.x, tempVector.y + cameraPositionUp.y, camera.OriginPosition.z);

        }
    }
}