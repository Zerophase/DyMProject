using UnityEngine;
using Assets.Scripts.Character.Interfaces;
using Assets.Scripts.GameObjects;

namespace Assets.Scripts.Projectiles.Interfaces
{
    public interface IPooledGameObjects
    {
        void Initialize(ICharacter character);

        Bullet GetPooledBullet(ICharacter character);
        void DeactivatePooledBullet(GameObject bullet,
            ICharacter character, IProjectile projectile);
    }
}
