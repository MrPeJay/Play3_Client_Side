using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Play3_Client_Side.DessignPatterns.Facade
{
    class GunFacade
    {
        private readonly GunLowerRail glr;
        private readonly GunUpperRail gur;
        private readonly GunMuzzle gmuz;
        private readonly GunMagazines gmag;

        public GunFacade()
        {
            glr = new GunLowerRail();
            gur = new GunUpperRail();
            gmuz = new GunMuzzle();
            gmag = new GunMagazines();
        }

        public void CreateCompleteGun()
        {
            Console.WriteLine("---------   Assembling a gun   ---------");
            glr.setLowerRail();
            gur.setUpperRail();
            gmuz.setGunMuzzle();
            gmag.setGunMagazine();

            Console.WriteLine("---------   Gun assembly complete   ---------");
        }
    }
}
