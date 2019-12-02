using Play3_Client_Side.Prototype;

namespace Play3_Client_Side.Command
{
    class MoveLeft : ICommand
    {
        Player playerObject;
        int newCoord;
        public MoveLeft(Player playerToMove, int newCoord)
        {
            playerObject = playerToMove;
            this.newCoord = newCoord;
        }
        public void execute()
        {
            playerObject.MoveX(newCoord);
        }
    }
}
