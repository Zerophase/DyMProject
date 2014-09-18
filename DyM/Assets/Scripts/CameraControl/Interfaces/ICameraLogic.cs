using UnityEngine;

namespace Assets.Scripts.CameraControl.Interfaces
{
	public interface ICameraLogic
	{
		Vector3 Move(Vector2 position, Vector3 cameraPosition, float time);
		Vector3 OriginPosition { get; set; }
	}
}
