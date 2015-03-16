using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.DBEntities.Configuration
{
    public partial class Assembly
    {
        public override string ToString()
        {
            return Name + " " + Version;
        }
    }
}
