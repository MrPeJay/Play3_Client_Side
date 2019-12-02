namespace Play3_Client_Side.Prototype
{
    class PrototypeHolder
    {
        public Player PlayerClonableObject;
        public Food FoodClonableObject;
        public Obstacle ObstacleClonableObject;

        public PrototypeHolder()
        {
            PlayerClonableObject = new Player();
            FoodClonableObject = new Food();
            ObstacleClonableObject = new Obstacle();
        }

        public Player GetPlayerClone()
        {
            return (Player)PlayerClonableObject.Clone();
        }

        public Food GetFoodClone()
        {
            return (Food)FoodClonableObject.Clone();
        }

        public Obstacle GetObstacleClone()
        {
            return (Obstacle)ObstacleClonableObject.Clone();
        }
    }
}
