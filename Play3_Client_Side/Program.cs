using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Play3_Client_Side
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new GameWindow());

            var facade = new Play3_Client_Side.DessignPatterns.Facade.GunFacade();
            facade.CreateCompleteGun();
            Console.ReadLine();
        }
    }
}
