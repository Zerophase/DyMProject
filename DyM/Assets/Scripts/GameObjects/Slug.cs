using Assets.Scripts.MediatorPattern;
using Assets.Scripts.PathFinding;
using UnityEngine;
using System.Collections;

public class Slug : PhysicsMediator 
{
	Vector3 acceleration = new Vector3(2f, 0f, 0f);
	Vector3 maxPatrolDistance = new Vector3(10f, 0f, 0f);
	private Vector3 target;

	private PathFinder pathFinder;
	// Use this for initialization
	void Start () 
	{
		pathFinder = new PathFinder();
		pathFinder.UpdateGraph(0, transform.position);
		pathFinder.UpdateGraph(1, transform.position + maxPatrolDistance);

		for (int i = 0; i < pathFinder.NodeCount; i++)
		{
			pathFinder.AddAllNeighborsToNode(i);
		}
		base.Start();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (pathFinder.UpdateDistanceTraveled(transform.position) < 10f || !pathFinder.FirstSearchDone)
			target = pathFinder.CreatePathAStarDistanceSquared(transform.position);

		if(target.sqrMagnitude > transform.position.sqrMagnitude)
			transform.Translate(cardinalMovement.Move(-1, acceleration, Time.deltaTime));
		else if(target.sqrMagnitude < transform.position.sqrMagnitude)
			transform.Translate(cardinalMovement.Move(1, acceleration, Time.deltaTime));
		
		// so the slug falls.
		transform.Translate(cardinalMovement.Jump(false, -1f));
	}
}
