using System.Collections.Generic;
using System.Linq;

namespace Play3_Client_Side.Prototype_Template_Composite
{
    class ObjectGroup : ObjectComponent
    {
        public List<ObjectComponent> objectComponents = new List<ObjectComponent>();

        public string GroupName { get; }
        public string GroupDescription { get; }

        public ObjectGroup(string groupName, string description)
        {
            GroupName = groupName;
            GroupDescription = description;
        }

        public override void AddObject(ObjectComponent newObjectComponent)
        {
            objectComponents.Add(newObjectComponent);
        }

        public override void RemoveObject(ObjectComponent objectComponentToRemove)
        {
            objectComponents.Remove(objectComponentToRemove);
        }

        public override string GetComponentInfo()
        {
            var componentInfo = GroupName + "\n" + GroupDescription + "\n";

            return objectComponents.Aggregate(componentInfo, (current, component) => current + component.GetComponentInfo());
        }

        public override bool isLeaf()
        {
            return false;
        }

        public override List<ObjectComponent> GetLeafObjects()
        {
            return objectComponents.Where(component => isLeaf()).ToList();
        }
    }
}
