using Assets.Editor;
using Assets.Scripts.GameObjects;
using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(CameraControl))]
public class FloatEditor : Editor
{
	private CameraControl instance;
	private PropertyField[] fields;

	public void OnEnable()
	{
		instance = target as CameraControl;
		fields = ExposeProperties.GetProperties(instance);
	}

	public override void OnInspectorGUI()
	{
		if(instance == null)
			return;

		this.DrawDefaultInspector();

		ExposeProperties.Expose(fields);
	}
}
