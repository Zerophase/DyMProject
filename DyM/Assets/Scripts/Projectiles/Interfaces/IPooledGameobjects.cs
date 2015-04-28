using UnityEngine;
using Assets.Scripts.Character.Interfaces;

namespace Assets.Scripts.Projectiles.Interfaces
{
    public interface IPooledGameObjects
    {
        void Initialize(ICharacter character);

        GameObject GetPooledBullet(ICharacter character);
        void DeactivatePooledBullet(GameObject bullet,
            ICharacter character, IProjectile projectile);
    }
}
