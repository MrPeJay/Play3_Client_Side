//using Play3_Client_Side.DessignPatterns.Facade;
using System;
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

            //var facade = new GunFacade();
            //facade.CreateCompleteGun();
            Console.ReadLine();
        }
    }
}
