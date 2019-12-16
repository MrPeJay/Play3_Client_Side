using System.Collections.Generic;

namespace Play3_Client_Side.Memento
{
    class CareTaker
    {
        private List<Memento> SavedPositions = new List<Memento>();

        public void AddMemento(Memento memento)
        {
            SavedPositions.Add(memento);
        }

        public Memento GetMemento(int index)
        {
            return SavedPositions[index];
        }

        public int GetMementoCount()
        {
            return SavedPositions.Count;
        }
    }
}
