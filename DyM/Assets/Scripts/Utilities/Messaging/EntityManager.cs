using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Utilities.Messaging.Interfaces;

namespace Assets.Scripts.Utilities.Messaging
{
    public interface IEntityManager
    {
        void Add(Entities entityType, int value, IOwner owner);
        void Remove(Entities entityType, int value);
        IOwner GetEntityFromID(Entities key, int id);
    }

    public class EntityManager : IEntityManager
    {
        private Dictionary<Entities, EntityIds> entities = new Dictionary<Entities, EntityIds>();
        private EntityIds ids = new EntityIds();

        public EntityManager()
        {

        }

        public void Add(Entities entityType, int value, IOwner owner)
        {
            if(!entities.ContainsKey(entityType))
                entities.Add(entityType, ids);
            
            ids.ObjectIds.Add(value, owner);
        }

        public void Remove(Entities entityType, int value)
        {
            entities.Remove(entityType);
        }

        public IOwner GetEntityFromID(Entities key, int id)
        {
            return entities[key].ObjectIds[id];
        }
    }
}
