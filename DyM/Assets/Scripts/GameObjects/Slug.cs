using Assets.Scripts.Character.Interfaces;
using Assets.Scripts.MediatorPattern;
using Assets.Scripts.PathFinding;
using Assets.Scripts.StatusEffects;
using ModestTree.Zenject;
using UnityEngine;
using System.Collections;

public class Slug : MovablePhysicsMediator 
{
	Vector3 acceleration = new Vector3(2f, 0f, 0f);
	Vector3 maxPatrolDistance = new Vector3(10f, 0f, 0f);
	private Vector3 target;

	private Vector3 comparisor;

	private PathFinder pathFinder;

	private int health = 10;
	public int Health { get { return health; } }

	private int damage = 2;

	[Inject] private ICharacter character;

	// Use this for initialization
	void Start () 
	{
		pathFinder = new PathFinder();
		pathFinder.UpdateGraph(0, transform.position);
		pathFinder.UpdateGraph(1, transform.position + maxPatrolDistance);

		base.Start();

		for (int i = 0; i < pathFinder.NodeCount; i++)
		{
			pathFinder.AddAllNeighborsToNode(i);
		}

		pathFinder.Initialize();
	}

	void Update ()
	{
		base.Update();

		if (character.StatusEffect == StatusEffect.SLOW_TIME)
		{
			movementMultiplier = 0.5f;
			character.RemoveStatusEffect();
		}
		else
			movementMultiplier = 1.0f;

		if (pathFinder.UpdateDistanceTraveled(transform.position) < 10f || !pathFinder.FirstSearchDone)
			target = pathFinder.CreatePathAStarDistanceSquared(transform.position);

		comparisor = target - transform.position;
		if (Vector3.Dot(comparisor, Vector3.left) > 0f)
		{
			speed = -1f*movementMultiplier;
			transform.Translate(cardinalMovement.Move(speed, acceleration, Time.deltaTime));
		}
			
		else if (Vector3.Dot(comparisor, Vector3.left) < 0f)
		{
			speed = 1f*movementMultiplier;
			transform.Translate(cardinalMovement.Move(speed, acceleration, Time.deltaTime));
		}
		
		// so the slug falls.
		transform.Translate(cardinalMovement.Jump(false, Vector3.zero));

		flip(speed);
	}

	public void TakeDamge(int healthLost)
	{
		health -= healthLost;
	}

	public int DealDamage()
	{
		return damage;
	}
}
