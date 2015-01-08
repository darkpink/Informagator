using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.HL7Assist
{
    internal static class Extensions
    {
        public static FlexibleList<T> ToFlexibleList<T>(this IEnumerable<T> items)
            where T : new()
        {
            FlexibleList<T> flexList = new FlexibleList<T>();
            flexList.AddRange(items);
            return flexList;
        }
    }
}
