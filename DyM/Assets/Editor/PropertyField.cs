using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using System.Collections;
using Assets.Scripts.Utilities.CustomEditor;

namespace Assets.Editor
{
	public class PropertyField
	{
		private System.Object instance;
		private PropertyInfo info;
		private SerializedPropertyType type;

		private MethodInfo getter;
		private MethodInfo setter;

		public SerializedPropertyType Type
		{
			get { return type; }
		}

		public string Name
		{
			get { return ObjectNames.NicifyVariableName(info.Name); }
		}

		public PropertyField(System.Object instance, PropertyInfo info, SerializedPropertyType type)
		{
			this.instance = instance;
			this.info = info;
			this.type = type;

			getter = this.info.GetGetMethod();
			setter = this.info.GetSetMethod();
		}

		public System.Object GetValue()
		{
			return getter.Invoke(instance, null);
		}

		public void SetValue(System.Object value)
		{
			setter.Invoke(instance, new System.Object[] { value });
		}

		public static bool GetPropertyType(PropertyInfo info, out SerializedPropertyType propertyType)
		{
			propertyType = SerializedPropertyType.Generic;

			Type type = info.PropertyType;

			if(type == typeof(int))
			{
				propertyType = SerializedPropertyType.Integer;
				return true;
			}

			if(type == typeof(float))
			{
				propertyType = SerializedPropertyType.Float;
				return true;
			}

			if(type == typeof(bool))
			{
				propertyType = SerializedPropertyType.Boolean;
				return true;
			}

			if(type == typeof(string))
			{
				propertyType = SerializedPropertyType.String;
				return true;
			}

			if(type == typeof(Vector2))
			{
				propertyType = SerializedPropertyType.Vector2;
				return true;
			}

			if(type == typeof(Vector3))
			{
				propertyType = SerializedPropertyType.Vector3;
				return true;
			}

			if(type.IsEnum)
			{
				propertyType = SerializedPropertyType.Enum;
				return true;
			}

			return false;
		}
	}
}
