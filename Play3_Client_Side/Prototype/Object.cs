using System.Windows.Forms;

namespace Play3_Client_Side.Prototype
{
    abstract class Object : IObject
    { 
        public int xCoord { get; set; }
        public int yCoord { get; set; }
        public string Uuid { get; set; }
        public int size { get; set; }
        public Control objectControl { get; set; }

        public abstract object Clone();
        public abstract void SetName(string name);
        public abstract void SetLocation(int x, int y);
        public abstract void SetSize(int size);

        //Update template method.
        public void Update(string name, int x, int y, int size)
        {
            SetName(name);
            SetLocation(x, y);
            SetSize(size);
        }
    }
}
