using UnityEngine;

namespace Assets.Scripts.CameraControl.Interfaces
{
	public interface ICameraLogic
	{
		Vector3 Move(Vector3 position, Vector3 playerVelocity, Vector3 cameraPosition, float time);
		Vector3 OriginPosition { get; set; }
		float Speed { get; set; }
	}
}
