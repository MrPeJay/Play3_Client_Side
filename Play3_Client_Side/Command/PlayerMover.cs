using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
