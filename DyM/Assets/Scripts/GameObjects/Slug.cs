using Assets.Scripts.MediatorPattern;
using UnityEngine;
using System.Collections;

public class Slug : PhysicsMediator 
{
	Vector3 acceleration = new Vector3(2f, 0f, 0f);
	// Use this for initialization
	void Start () 
	{
		base.Start();
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.Translate(cardinalMovement.Move(1, acceleration, Time.deltaTime));
		
		// so the slug falls.
		transform.Translate(cardinalMovement.Jump(false, -1f));
	}
}
