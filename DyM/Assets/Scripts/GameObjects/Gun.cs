using vc = Assets.Scripts.Utilities.Constants.VectorConstants;
using Assets.Scripts.CustomInputManager;
using Assets.Scripts.Utilities;
using UnityEngine;

public class Gun : MonoBehaviour
{
	private Quaternion weaponOrigin;
    private Quaternion rotationOrigin;
	[HideInInspector]
	public bool Rotated;

    public Vector3 Direction;
    public Vector3 PreviousDirection;

	private readonly Vector3EqualityComparerWithTolerance equalityComparer =
		new Vector3EqualityComparerWithTolerance();

    public void Rotate()
    {
        Direction = InputManager.Aim();
        float rotate = Mathf.Atan2(-Direction.y, -Direction.x)*Mathf.Rad2Deg;

        if (!equalityComparer.Equals(Direction, Vector3.zero))
            transform.rotation = Quaternion.Euler(0f, 0f, -rotate);
	}

	public void Rotate(float dotProduct)
	{
		if (dotProduct > 0.0f)
		{
			transform.eulerAngles = vc.RotationLeft;
		}
		else if (dotProduct < 0.0f)
		{
			transform.eulerAngles = vc.RotationRight;
		}
	}
}
