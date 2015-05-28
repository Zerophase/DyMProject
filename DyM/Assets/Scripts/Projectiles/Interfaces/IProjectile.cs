using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Character;
using UnityEngine;
using Assets.Scripts.Character.Interfaces;

namespace Assets.Scripts.Projectiles.Interfaces
{
	public interface IProjectile
	{
		Material GetMaterial { get; }
		Mesh GetMesh { get; }

		bool IsProjectileSetup { get; }
		void SetUpProjectile(Vector3 position);
		Vector3 ProjectilePattern();

		bool ShouldProjectileDeactivate(Vector3 position);
		void DeactivateProjectile();
		int DealDamage();

        Vector3 ShotDirection { set; }

		GameObject GameObject { get; }

        ICharacter Character { get; set; }
		CharacterTypes CharacterType { get; set; }
	}
}
