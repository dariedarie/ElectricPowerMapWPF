using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Predmetnizadatak2
{
    public class Node : PowerEntity
    {
       
        public Node()
        {

        }

        public Node(long id, string name, double x, double y)
        {
            this.Id = id;
            this.Name = name;
            this.X = x;
            this.Y = y;
        }

        public Node(long id, double x, double y)
        {
            this.Id = id;
            this.X = x;
            this.Y = y;
        }

       
        


    }
}
