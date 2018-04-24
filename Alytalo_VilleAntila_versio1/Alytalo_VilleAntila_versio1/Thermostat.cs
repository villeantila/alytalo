using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alytalo_VilleAntila_versio1
{
    public class Thermostat
    {
        public int Temperature { get; set; }
        public void SetTemperature(int apu)
        {
            Temperature = apu;
        }
    }
}
