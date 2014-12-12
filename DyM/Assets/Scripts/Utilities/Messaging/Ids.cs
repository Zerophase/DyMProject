using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Utilities.Messaging
{
    public interface IIds
    {
        void CreateId();
        int ObjectId { get; set; }
    }
    public class Ids : IIds
    {
        private static int id;

        private int objectId;
        public int ObjectId { get { return objectId; } set { ObjectId = objectId;} }

        public void CreateId()
        {
            objectId = id++;
        }
    }
}
