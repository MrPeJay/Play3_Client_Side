namespace Play3_Client_Side.Command
{
    // This is an Invoker class
    class PlayerMover
    {
        public PlayerMover() { }

        public void performMove(ICommand moveCommand)
        {
            moveCommand.execute();
        }
    }
}
