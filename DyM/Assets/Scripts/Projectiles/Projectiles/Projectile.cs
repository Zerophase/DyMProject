using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Projectiles.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Projectiles.Projectiles
{
	public class Projectile : IProjectile
	{
		private Material material;
		public Material GetMaterial { get { return material; } }
		Projectile()
		{
			
		}


		public Mesh GetMesh
		{
			get { throw new NotImplementedException(); }
		}
	}
}
