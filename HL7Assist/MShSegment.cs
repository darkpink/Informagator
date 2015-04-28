using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.HL7Assist
{
    public class MSHSegment : HL7MessageSegment
    {
        internal MSHSegment()
            : base("MSH")
        {
        }

        internal MSHSegment(string segment)
            : base(segment)
        {
        }

        public override HL7Field this[int fieldIndex]
        {
            get
            {
                return base[fieldIndex - 1];
            }
            set
            {
                base[fieldIndex - 1] = value;
            }
        }
    }
}
