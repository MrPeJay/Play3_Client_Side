using Play3_Client_Side.Prototype;

namespace Play3_Client_Side.Command
{
    class MoveUp : ICommand
    {
        Player playerObject;
        int newCoord;
        public MoveUp(Player playerToMove, int newCoord)
        {
            playerObject = playerToMove;
            this.newCoord = newCoord;
        }
        public void execute()
        {
            playerObject.MoveY(newCoord);
        }
    }
}
