using Informagator.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.HL7Assist
{
    public class HL7Message : IMessage, IEnumerable<HL7MessageSegment>
    {
        private const string CRLF = "\r\n";

        protected List<HL7MessageSegment> Segments { get; set; }

        public HL7Message()
        {
            Segments = new List<HL7MessageSegment>();
            Segments.Add(new MSHSegment());
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

        public HL7MessageSegment AddSegment(string segment)
        {
            HL7MessageSegment result = new HL7MessageSegment(segment);
            
            Segments.Add(result);
            
            return result;
        }

        public HL7MessageSegment[] AddSegments(params string[] segments)
        {
            HL7MessageSegment[] result = segments.Select(s => new HL7MessageSegment(s)).ToArray();
            Segments.AddRange(result);
            return result;
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

        private void Parse(string message)
        {
            Segments.AddRange(message.Split(new[] { '\r', '\n'}, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.StartsWith("MSH") ? new MSHSegment() :  new HL7MessageSegment(s))
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

        public Guid Id
        {
            get { throw new NotImplementedException(); }
        }

        public byte[] BinaryData
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public IDictionary<string, string> Attributes
        {
            get { throw new NotImplementedException(); }
        }

        public IList<string> ProcessingTrail
        {
            get { throw new NotImplementedException(); }
        }
    }
}
