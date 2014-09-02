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

		private void Start()
		{
			camera.OriginPosition = transform.localPosition;
		}

		private void Update()
		{
			//Debug.Log("Right Stick Magnitude: " + new Vector2(Input.GetAxis("CameraHorizontalMovement"),
			//		Input.GetAxis("CameraVerticalMovement")).magnitude);
			transform.localPosition = camera.Move( 
				new Vector2(Input.GetAxis("CameraHorizontalMovement"), 
					Input.GetAxis("CameraVerticalMovement")),
				transform.localPosition, Time.deltaTime);
		}
	}
}
