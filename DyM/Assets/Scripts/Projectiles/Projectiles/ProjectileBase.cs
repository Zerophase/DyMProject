using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.GameObjects;
using Assets.Scripts.Projectiles.Interfaces;
using UnityEngine;
using Assets.Scripts.Character.Interfaces;

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
        private Vector3 shotDirection;
        public Vector3 ShotDirection { set { shotDirection = value; } }

		protected float speed = 20.0f;
		private float deactivationDistance;

		protected int damage;

        private ICharacter character;
        public ICharacter Character { get { return character; } set { character = value; } }

		public ProjectileBase(string matterialName, float deactivationDistance)
		{
			material = Resources.Load<Material>("Materials/" + matterialName);
			//TODO once meshes are made replace with direct reference to mesh.
			GameObject go;
			if(!GameObject.Find("Bullet" + "(Clone)"))
				go = GameObject.Instantiate(Resources.Load<GameObject>("Meshes/" + "Bullet")) as GameObject;
			else
			{
				go = GameObject.Find("Bullet" + "(Clone)");
			}

			mesh = go.GetComponent<MeshFilter>().mesh;
			this.deactivationDistance = deactivationDistance;
		}

		public virtual void SetUpProjectile(Vector3 position)
		{
			startPosition = position;

			fireDirection = shotDirection;
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
