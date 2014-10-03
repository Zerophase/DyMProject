using UnityEditor;
using UnityEngine;
using System.Collections;

public static class PhysicsFuncts 
{
    public static Vector3 calculateVelocity(Vector3 currentAcceleration, float direction)
    {
        Vector3 result = Vector3.zero;

        result = currentAcceleration*direction;

        return result;
    }

    public static Vector3 calculatePosition(Vector3 currentVelocity)
    {
        Vector3 result = Vector3.zero;

        result = currentVelocity*Time.deltaTime;

        return result;
    }
}
