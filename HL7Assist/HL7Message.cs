using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.HL7Assist
{
    public class HL7Message : IEnumerable<HL7MessageSegment>
    {
        private const string CRLF = "\r\n";

        protected List<HL7MessageSegment> Segments { get; set; }

        public HL7Message()
        {
            Segments = new List<HL7MessageSegment>();
        }

        public HL7Message(string message)
            : this()
        {
            Parse(message);
        }

        public HL7Message(Stream message)
            : this()
        {
            using (StreamReader reader = new StreamReader(message))
            {
                Parse(reader.ReadToEnd());
            }
        }

        public HL7Message(byte[] message)
            : this()
        {
            Parse(Encoding.ASCII.GetString(message));
        }

        public HL7MessageSegment this[string segmentId]
        {
            get
            {
                return Segments.FirstOrDefault(seg => String.Compare(seg.Id, segmentId, true) == 0);
            }
        }

        public HL7MessageSegment this[string segmentId, int sequence]
        {
            get
            {
                return Segments.Where(seg => String.Compare(seg.Id, segmentId, true) == 0).Skip(sequence - 1).FirstOrDefault();
            }
        }

        public HL7MessageSegment this[int index]
        {
            get
            {
                return Segments[index];
            }
            set
            {
                Segments[index] = value;
            }
        }

        public int SegmentCount
        {
            get
            {
                return Segments.Count;
            }
        }

        public override string ToString()
        {
            return String.Join(CRLF, Segments);
        }

        public static implicit operator HL7Message(string message)
        {
            return new HL7Message(message);
        }

        private void Parse(string message)
        {
            Segments.AddRange(message.Split(new[] { '\r', '\n'}, StringSplitOptions.RemoveEmptyEntries)
                              .Select(s => new HL7MessageSegment(s))
                              );
        }

        public IEnumerator<HL7MessageSegment> GetEnumerator()
        {
            foreach (HL7MessageSegment segment in Segments)
            {
                yield return segment;
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            foreach (HL7MessageSegment segment in Segments)
            {
                yield return segment;
            }
        }
    }
}
