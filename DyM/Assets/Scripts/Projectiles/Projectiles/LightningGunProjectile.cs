using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Projectiles.Interfaces;
using ModestTree.Zenject;
using UnityEngine;

namespace Assets.Scripts.Projectiles.Projectiles
{
	public class LightningGunProjectile : IProjectile
	{
		private Material material;
		public Material GetMaterial { get { return material; } }

		private Mesh mesh;
		public Mesh GetMesh {get { return mesh; }}

		public LightningGunProjectile()
		{
			material = Resources.Load<Material>("Materials/LightningGunBullet");
			//TODO once meshes are made replace with direct reference to mesh.
			GameObject go = GameObject.Instantiate(Resources.Load<GameObject>("Meshes/Cube")) as GameObject;
			mesh = go.GetComponent<MeshFilter>().mesh;
		}
	}
}
