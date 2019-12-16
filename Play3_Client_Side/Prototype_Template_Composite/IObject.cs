using System;
using System.Windows.Forms;

namespace Play3_Client_Side.Prototype
{
    public interface IObject : ICloneable
    {
        int xCoord { get; set; }
        int yCoord { get; set; }
        string Uuid { get; set; }
        int size { get; set; }
        Control objectControl { get; set; }
    }
}

public enum ObjectType
{
    Player,
    Food,
    Obstacle,
    TimeTravel
}
