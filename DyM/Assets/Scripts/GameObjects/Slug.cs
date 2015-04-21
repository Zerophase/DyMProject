using Assets.Scripts.Character.Interfaces;
using Assets.Scripts.MediatorPattern;
using Assets.Scripts.PathFinding;
using Assets.Scripts.StatusEffects;
using ModestTree.Zenject;
using UnityEngine;
using System.Collections;

public class Slug : MovablePhysicsMediator 
{
	protected Vector3 acceleration = new Vector3(2f, 0f, 0f);
	protected Vector3 maxPatrolDistance = new Vector3(10f, 0f, 0f);

	private Vector3 target;
	protected Vector3 comparisor;
	private PathFinder pathFinder;

	protected int health = 10;
	public int Health { get { return health; } }

	private int damage = 2;

	[Inject] private ICharacter character;

	// Use this for initialization
	protected virtual void Start () 
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

	protected virtual void Update ()
	{
        statusEffect();

		if (pathFinder.UpdateDistanceTraveled(transform.position) < 10f || !pathFinder.FirstSearchDone)
			target = pathFinder.CreatePathAStarDistanceSquared(transform.position);

		comparisor = target - transform.position;

		var modifiedAccel = acceleration*movementMultiplier;
		if (Vector3.Dot(comparisor, Vector3.left) > 0f)
		{
			speed = -1f;
			UpdateVelocity(cardinalMovement.Move(speed, modifiedAccel, Time.deltaTime));
		}
			
		else if (Vector3.Dot(comparisor, Vector3.left) < 0f)
		{
			speed = 1f;
			UpdateVelocity(cardinalMovement.Move(speed, modifiedAccel, Time.deltaTime));
		}

		flip(speed);
	}

	private void statusEffect()
	{
		
		var statusEffect = character.StatusEffect;
		if (statusEffect == StatusEffect.NONE)
			movementMultiplier = 1.0f;
		else if((statusEffect & StatusEffect.SLOW_TIME) == 
			StatusEffect.SLOW_TIME)
		{
			movementMultiplier = 0.5f;
		}
		else if((statusEffect & StatusEffect.BOOST_TIME) ==
			StatusEffect.BOOST_TIME)
		{
			movementMultiplier = 1.5f;
		}

		if (character.StatusEffect != StatusEffect.NONE)
		{
			character.RemoveStatusEffect();
		}
			
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
