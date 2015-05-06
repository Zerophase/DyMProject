using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Character;
using Assets.Scripts.Weapons.Interfaces;
using Assets.Scripts.Character.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Projectiles.Interfaces
{
	public interface IBulletPool
	{
		void Initialize(IRangeWeapon rangeWeapon, Vector3 startPosition, int count);
        Dictionary<CharacterTypes, List<IPooledProjectile>> ProjectileBoundToCharacterType { get; }
        List<IPooledProjectile> GetProjectiles(ICharacter character);
		IPooledProjectile GetPooledProjectile(IRangeWeapon rangeWeapon);
		void DeactivatePooledProjectile(ICharacter character, IProjectile projectile);
		void ChangeBullet(IRangeWeapon rangeWeapon);
	}
}
