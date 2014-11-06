using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Projectiles.Interfaces
{
	public interface IProjectile
	{
		Material GetMaterial { get; }
		Mesh GetMesh { get; }
	}
}
