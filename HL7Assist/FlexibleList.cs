using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acadian.HL7Assist
{
    internal class FlexibleList<T> : IList<T>
        where T : new()
    {
        protected List<T> InnerList { get; set; }

        public FlexibleList()
        {
            InnerList = new List<T>();
        }

        public int IndexOf(T item)
        {
            int result;

            if (InnerList.Contains(item))
            {
                result = InnerList.IndexOf(item);
            }
            else
            {
                result = -1;
            }

            return result;
        }

        public void Insert(int index, T item)
        {
            EnsureListIndex(index - 1);
            InnerList.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            if (index < InnerList.Count)
            {
                InnerList.RemoveAt(index);
            }
        }

        public T this[int index]
        {
            get
            {
                EnsureListIndex(index);
                return InnerList[index];
            }
            set
            {
                EnsureListIndex(index);  //todo - this potentially creates one more object than we need
                InnerList[index] = value;
            }
        }

        public void Add(T item)
        {
            InnerList.Add(item);
        }

        public void AddRange(IEnumerable<T> items)
        {
            InnerList.AddRange(items);
        }

        public void Clear()
        {
            InnerList.Clear();
        }

        public bool Contains(T item)
        {
            return InnerList.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            InnerList.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return InnerList.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(T item)
        {
            return InnerList.Remove(item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return InnerList.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return InnerList.GetEnumerator();
        }

        protected void EnsureListIndex(int index)
        {
            while (index >= InnerList.Count)
            {
                InnerList.Add(new T());
            }
        }
    }
}
