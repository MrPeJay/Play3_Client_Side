using System.Collections.Generic;

namespace Play3_Client_Side.Prototype_Template_Composite
{
    abstract class ObjectComponent
    {
        public abstract void AddObject(ObjectComponent newObjectComponent);

        public abstract void RemoveObject(ObjectComponent objectComponentToRemove);

        public abstract string GetComponentInfo();

        public abstract bool isLeaf();

        public abstract List<ObjectComponent> GetLeafObjects();
    }
}
