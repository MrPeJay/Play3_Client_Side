using Play3_Client_Side.Prototype_Template_Composite;

namespace Play3_Client_Side.Prototype
{
    class PrototypeHolder
    {
        public Player PlayerClonableObject;
        public Food FoodClonableObject;
        public Obstacle ObstacleClonableObject;
        public TimeTravel TimeTravelClonableObject;

        public PrototypeHolder()
        {
            PlayerClonableObject = new Player();
            FoodClonableObject = new Food();
            ObstacleClonableObject = new Obstacle();
            TimeTravelClonableObject = new TimeTravel();
        }

        public Player GetPlayerClone()
        {
            return (Player) PlayerClonableObject.Clone();
        }

        public Food GetFoodClone()
        {
            return (Food) FoodClonableObject.Clone();
        }

        public Obstacle GetObstacleClone()
        {
            return (Obstacle) ObstacleClonableObject.Clone();
        }

        public TimeTravel GetTimeTravelClone()
        {
            return (TimeTravel) TimeTravelClonableObject.Clone();
        }
    }
}
