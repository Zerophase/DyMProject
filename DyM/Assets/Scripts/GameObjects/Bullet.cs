using Assets.Scripts.GameObjects;
using Assets.Scripts.Projectiles.Interfaces;
using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
	private IProjectile projectile;
	public IProjectile Projectile { set { projectile = value; } }

	private Vector3 shootForward = new Vector3(1f, 0f, 0f);
	void Update ()
	{
		transform.position +=  shootForward.x * Player.GunModel.transform.up;
	}
}
