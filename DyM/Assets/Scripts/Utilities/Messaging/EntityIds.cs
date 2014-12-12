using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Utilities.Messaging.Interfaces;

namespace Assets.Scripts.Utilities.Messaging
{
    public class EntityIds
    {
        private Dictionary<int, IOwner> objectIds = new Dictionary<int, IOwner>();
        public Dictionary<int, IOwner> ObjectIds { get { return objectIds; } }
    }
}
