using Play3_Client_Side.Prototype;

namespace Play3_Client_Side.Command
{
    class MoveRight : ICommand
    {
        Player playerObject;
        int newCoord;
        public MoveRight(Player playerToMove, int newCoord)
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
