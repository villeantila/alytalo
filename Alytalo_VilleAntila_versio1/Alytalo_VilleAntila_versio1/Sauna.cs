using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alytalo_VilleAntila_versio1
{
    public class Sauna
    {
        public bool Switched { get; set; }
        public int SaunaTemperature { get; set; }
        public void Kytke()
        {
            if (Switched == true)
            {
                Switched = false;
            }
            else
            {
                Switched = true;
            }
        }
    }
}
