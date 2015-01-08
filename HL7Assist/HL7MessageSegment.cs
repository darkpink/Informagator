using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.HL7Assist
{
    public class HL7MessageSegment
    {
        public string Id {
            get
            {
                return Fields[0];
            }
            set
            {
                Fields[0] = value;
            }
        }

        internal FlexibleList<HL7Field> Fields { get; set; }

        public HL7MessageSegment()
        {
            Fields = new FlexibleList<HL7Field>();
        }

        public HL7MessageSegment(string segment)
            :this()
        {
            Fields.AddRange(segment.Split('|').Select(s => new HL7Field(s)));
        }

        public HL7Field this[int fieldIndex]
        {
            get
            {
                HL7Field field = Fields.Skip(fieldIndex).FirstOrDefault();
                if (field == null)
                {
                    field = new HL7Field();
                    Fields[fieldIndex] = field;
                }
                return field;
            }
            set
            {
                Fields[fieldIndex] = value;
            }
        }

        public override string ToString()
        {
            return String.Join("|", Fields);
        }

        public static implicit operator String(HL7MessageSegment segment)
        {
            return segment == null ? null : segment.ToString();
        }

        public static implicit operator HL7MessageSegment(string segment)
        {
            return new HL7MessageSegment(segment);
        }
    }
}
