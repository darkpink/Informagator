using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acadian.HL7Assist
{
    public class HL7Field
    {
        internal FlexibleList<HL7FieldComponent> Components { get; set; }

        public HL7Field()
        {
            Components = new FlexibleList<HL7FieldComponent>();
        }

        public HL7Field(string value)
            : this()
        {
            if (value != null)
            {
                Components = value.Split(new[] { '^' }, StringSplitOptions.None)
                                  .Select(v => new HL7FieldComponent(v))
                                  .ToFlexibleList();
            }
        }

        public string this[int index]
        {
            get
            {
                return Components[index - 1].ToString();
            }
            set
            {
                Components[index - 1] = value;
            }
        }

        public override string ToString()
        {
            return String.Join("^", Components);
        }

        public static implicit operator String(HL7Field value)
        {
            return value.ToString();
        }

        public static implicit operator HL7Field(string value)
        {
            return new HL7Field(value);
        }
    }
}
