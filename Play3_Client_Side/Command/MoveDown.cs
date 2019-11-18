﻿using Play3_Client_Side.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Play3_Client_Side.Command
{
    class MoveDown : ICommand
    {
        Player playerObject;
        int newCoord;
        public MoveDown(Player playerToMove, int newCoord)
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