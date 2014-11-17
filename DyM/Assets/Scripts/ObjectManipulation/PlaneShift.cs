using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.ObjectManipulation.Interfaces;
using ModestTree.Zenject;
using UnityEngine;

namespace Assets.Scripts.ObjectManipulation
{

	public enum PlanePosition
	{
		UP = -1,
		CENTER = 0,
		DOWN = 1
	}
	
	public class PlaneShift : IPlaneShift
	{
		private float planeShiftTimer;
		private KeyCode savedKeyPress = KeyCode.None;

		private const int arrayIndexNotFound = -1;

		private Vector3 planeShiftUpVector = new Vector3(0f, 0f, -1f);
		private Vector3 planeShiftDownVector = new Vector3(0f, 0f, 1f);
		private Vector3 PlaneShiftedTo;

		private PlanePosition planePosition;

		private float[] shiftPlanePosition = new float[3]
		{
			-1f, // down
			0f, // center
			1f // up
		};

		public PlaneShift()
		{
			
		}

		[Inject]
		public PlaneShift(Vector3 position)
		{
			planePosition = PlanePosition.CENTER;
			setShiftPlanePosition(position);
		}

		public PlaneShift(Vector3 position, PlanePosition planePosition)
		{
			this.planePosition = planePosition;
			setShiftPlanePosition(position);
		}

		private void setShiftPlanePosition(Vector3 position)
		{
			for (int i = 0; i < shiftPlanePosition.Length; i++)
			{
				shiftPlanePosition[i] += position.z + (float)planePosition;
			}
		}

		public Vector3 ShiftPlane(KeyCode activatePlaneShift, Vector3 currentPosition)
		{
			savedKeyPress = activatePlaneShift;
			resetPlaneShiftTimer();
			setPlaneShiftedTo(currentPosition.z);

			if (activatePlaneShift == KeyCode.Joystick1Button4 &&
				maxShift(currentPosition.z, 1f))
			{
				return planeShiftDownVector;
			}
			else if (activatePlaneShift == KeyCode.Joystick1Button5 &&
				maxShift(currentPosition.z, -1f))
			{
				return planeShiftUpVector;
			}
			else
			{
				return Vector3.zero;
			}
		}

		// needs to round float to nearest whole number
		private bool maxShift(float plane, float direction)
		{
			//TODO fix by correcting for rounding errors.
			float test = (float) Math.Round((plane + direction), 0);
			if (Array.IndexOf(shiftPlanePosition, 
				plane + (float)Math.Round(direction, 0)) > arrayIndexNotFound)
				return true;
			else
			{
				return false;
			}
		}

		private void setPlaneShiftedTo(float plane)
		{
			switch (savedKeyPress)
			{
				case KeyCode.Joystick1Button4:
					PlaneShiftedTo = new Vector3(0f, 0f, plane + 1f);
					break;
				case KeyCode.Joystick1Button5:
					PlaneShiftedTo = new Vector3(0f, 0f, plane - 1f);
					break;
			}

		}

		private void resetPlaneShiftTimer()
		{
			planeShiftTimer = 0f;
		}

		public Vector3 Dodge(Vector3 currentPlane, bool keyIsPressed, float timing)
		{
			if (savedKeyPress != KeyCode.None && keyIsPressed)
				planeShiftTimer += timing;

			Vector3 temp = Vector3.zero;
			if (!keyIsPressed && savedKeyPress != KeyCode.None && planeShiftTimer < .5f )
			{
				if (savedKeyPress == KeyCode.Joystick1Button4 &&
				    maxShift(PlaneShiftedTo.z))
					temp = planeShiftUpVector;
				else if (savedKeyPress == KeyCode.Joystick1Button5 &&
				         maxShift(PlaneShiftedTo.z))
					temp = planeShiftDownVector;

				savedKeyPress = KeyCode.None;
			}

			return temp;
		}

		private bool maxShift(float plane)
		{
			if (Array.IndexOf(shiftPlanePosition, plane) > arrayIndexNotFound)
				return true;
			return false;
		}
	}
}
