using System;
using System.Collections;
using System.Collections.Generic;

namespace PylonGameEngine.Utils
{
    [Serializable]
    public class LockedList<T> : IList<T> where T : UniqueNameInterface
    {
        public List<T> List { get; private set; }
        public object LOCK;

        public LockedList(ref object Lock)
        {
            List = new List<T>();
            LOCK = Lock;
        }

        public T this[int index]
        {
            get
            {
                lock (LOCK)
                {
                    return List[index];
                }
            }
            set
            {
                lock (LOCK)
                {
                    List[index] = value;
                }
            }
        }

        public void AddRange(IEnumerable<T> collection)
        {
            lock (LOCK)
            {
                List.AddRange(collection);
            }
        }

        public void Reverse()
        {
            lock (LOCK)
            {
                List.Reverse();
            }
        }

        public int Count
        {
            get
            {
                lock (LOCK)
                {
                    return List.Count;
                }
            }
        }

        public bool IsReadOnly => false;

        public void Add(T item)
        {
            lock (LOCK)
            {
                List.Add(item);
            }
        }

        public void Clear()
        {
            lock (LOCK)
            {
                List.Clear();
            }
        }

        public bool Contains(T item)
        {
            lock (LOCK)
            {
                return List.Contains(item);
            }
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            lock (LOCK)
            {
                List.CopyTo(array, arrayIndex);
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            lock (LOCK)
            {
                return List.GetEnumerator();
            }
        }

        public int IndexOf(T item)
        {
            lock (LOCK)
            {
                return List.IndexOf(item);
            }
        }

        public void Insert(int index, T item)
        {
            lock (LOCK)
            {
                List.Insert(index, item);
            }
        }

        public bool Remove(T item)
        {
            lock (LOCK)
            {
                return List.Remove(item);
            }
        }

        public T? Find(Predicate<T> match)
        {
            lock (LOCK)
            {
                try
                {
                    return List.Find(match);
                }
                catch
                {
                    return null;
                }
            }
        }

        public void RemoveAt(int index)
        {
            lock (LOCK)
            {
                List.RemoveAt(index);
            }
        }

        public void RemoveRange(int index, int count)
        {
            lock (LOCK)
            {
                List.RemoveRange(index, count);
            }
        }

        /// <summary>
        /// Gets <T> from Unique Name
        /// </summary>
        /// <param name="Name">The Name of the item</param>
        /// <returns></returns>
        public T Get(string Name)
        {
            lock (LOCK)
            {

                return this.Find(x => x.Name == Name);
            }
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            lock (LOCK)
            {
                return List.GetEnumerator();
            }
        }

        public T[] ToArray()
        {
            return List.ToArray();
        }
    }
}