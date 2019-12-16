using Play3_Client_Side.Prototype;

namespace Play3_Client_Side.Memento
{
    class Memento
    {
        private TwoDimensionalPosition position;
        private string id;

        public Memento(TwoDimensionalPosition position, string id)
        {
            this.position = position;
            this.id = id;
        }

        public TwoDimensionalPosition GetSavedPosition(string id)
        {
            return !id.Equals(this.id) ? null : position;
        }
    }
}
