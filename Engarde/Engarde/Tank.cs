using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engarde
{
    public class Tank : Character
    {
        public Tank(string Name) : base(Name, hp) 
        {
            Name = this.Name;
            hp = 200;
        }
    }
}
