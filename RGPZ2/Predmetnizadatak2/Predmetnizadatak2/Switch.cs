using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Predmetnizadatak2
{
    public class Switch : PowerEntity
    {
        
        private string status;

        public Switch()
        {

        }

        public Switch(long id, string name, double x, double y, string status)
        {
            this.Id = id;
            this.Name = name;
            this.X = x;
            this.Y = y;
            this.status = status;
        }

        public Switch(long id,  double x, double y)
        {
            this.Id = id;
            this.X = x;
            this.Y = y;
          
        }


        public string Status
        {
            get
            {
                return status;
            }

            set
            {
                status = value;
            }
        }

    }
}
