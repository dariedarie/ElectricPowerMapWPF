using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Predmetnizadatak2
{
    public class Substation : PowerEntity
    {
       

        public Substation()
        {

        }

        public Substation(long id, string name, double x, double y)
        {
            this.Id = id;
            this.Name = name;
            this.X = x;
            this.Y = y;
        }

        public Substation(long id, double x, double y)
        {
            this.Id = id;
            this.X = x;
            this.Y = y;
        }


    }
}
