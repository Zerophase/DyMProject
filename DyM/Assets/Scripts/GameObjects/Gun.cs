using System.Linq.Expressions;
using Assets.Scripts.Character;
using Assets.Scripts.CustomInputManager;
using Assets.Scripts.DependencyInjection;
using Assets.Scripts.GameObjects;
using Assets.Scripts.Utilities;
using Assets.Scripts.Weapons;
using Assets.Scripts.Weapons.Interfaces;
using ModestTree.Zenject;
using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour
{
	private Quaternion weaponOrigin;
    private Quaternion rotationOrigin;
	[HideInInspector]
	public bool Rotated;

    public Vector3 Direction;
    public Vector3 PreviousDirection;


    //Mouse Control Variables
    public Vector2 MousePosition;

	private readonly Vector3EqualityComparerWithTolerance equalityComparer =
		new Vector3EqualityComparerWithTolerance();

    public void Rotate()
    {
        if (InputManager.InputType.ToString() == "Assets.Scripts.CustomInputManager.Joystick")
            {
                Direction = new Vector3(InputManager.CameraHorizontalMovement(), InputManager.CameraVerticalMovement());

                float rotate = Mathf.Atan2(-Direction.y, -Direction.x)*Mathf.Rad2Deg;

                if (!equalityComparer.Equals(Direction, Vector3.zero))
                    transform.rotation = Quaternion.Euler(0f, 0f, -rotate);
            }

        else if (InputManager.InputType.ToString() == "Assets.Scripts.CustomInputManager.Keyboard")
        {
            MousePosition = InputManager.MousePositionOnScreen();
            MousePosition.x -= .5f;
            MousePosition.y -= .5f;
            //MousePosition.x = MousePosition.x*-1;
            MousePosition.y = MousePosition.y*-1;
            Direction = new Vector3(MousePosition.x,MousePosition.y);
            float rotate = Mathf.Atan2(-Direction.y, -Direction.x) * Mathf.Rad2Deg;

            if (!equalityComparer.Equals(Direction, Vector3.zero))
                transform.rotation = Quaternion.Euler(0f, 0f, -rotate);
            Debug.Log("X: " + MousePosition.x);
            Debug.Log("Y: " + MousePosition.y);
            
        }
	}
}
