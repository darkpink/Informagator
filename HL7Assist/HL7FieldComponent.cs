using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.HL7Assist
{
    public class HL7FieldComponent
    {
        public string Value { get; set; }

        public HL7FieldComponent()
        {
        }

        public HL7FieldComponent(string value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value;
        }

        public static implicit operator String(HL7FieldComponent value)
        {
            return value.ToString();
        }

        public static implicit operator HL7FieldComponent(string value)
        {
            return new HL7FieldComponent(value);
        }
    }
}
