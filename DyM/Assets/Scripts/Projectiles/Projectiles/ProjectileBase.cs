using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.GameObjects;
using Assets.Scripts.Projectiles.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Projectiles.Projectiles
{
	public abstract class ProjectileBase : IProjectile
	{
		private Material material;
		public Material GetMaterial { get { return material; } }

		private Mesh mesh;
		public Mesh GetMesh { get { return mesh; } }

		private bool setUp;
		public bool IsProjectileSetup { get { return setUp; } }

		private Vector3 startPosition;
		protected Vector3 fireDirection;

		protected float speed = 10.0f;
		private float deactivationDistance;

		protected int damage;
		public ProjectileBase(string matterialName, string gameObjectName, float deactivationDistance)
		{
			material = Resources.Load<Material>("Materials/" + matterialName);
			//TODO once meshes are made replace with direct reference to mesh.
			GameObject go;
			if(!GameObject.Find(gameObjectName + "(Clone)"))
				go = GameObject.Instantiate(Resources.Load<GameObject>("Meshes/" + gameObjectName)) as GameObject;
			else
			{
				go = GameObject.Find(gameObjectName + "(Clone)");
			}
			mesh = go.GetComponent<MeshFilter>().mesh;
			this.deactivationDistance = deactivationDistance;
		}

		public virtual void SetUpProjectile(Vector3 position)
		{
			startPosition = position;

			fireDirection = -Player.GunModel.transform.right;
			fireDirection.z = 0f;

			setUp = true;
		}

		public virtual Vector3 ProjectilePattern()
		{
			throw new NotImplementedException();
		}

		public bool ShouldProjectileDeactivate(Vector3 position)
		{
			return Mathf.Abs((position - startPosition).magnitude) > deactivationDistance;
		}

		public virtual void DeactivateProjectile()
		{
			setUp = false;
		}

		public override bool Equals(object obj)
		{
			if(obj != null)
				return  this.GetType() == obj.GetType();
			else
			{
				return false;
			}
		}

		public int DealDamage()
		{
			return damage;
		}
	}
}
