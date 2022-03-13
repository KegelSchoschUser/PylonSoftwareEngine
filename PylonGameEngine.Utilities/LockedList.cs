using System;
using System.Collections;
using System.Collections.Generic;

namespace PylonGameEngine.Utilities
{
    [Serializable]
    public class LockedList<T> : IList<T> where T : UniqueNameInterface
    {
        public static object GLOBALLOCK = new object();
        public List<T> List { get; private set; }
        public object Lock;

        public LockedList()
        {
            List = new List<T>();
            Lock = GLOBALLOCK;
        }
        public LockedList(ref object _Lock)
        {
            List = new List<T>();
            Lock = _Lock;
        }

        public T this[int index]
        {
            get
            {
                lock (Lock)
                {
                    return List[index];
                }
            }
            set
            {
                lock (Lock)
                {
                    List[index] = value;
                }
            }
        }

        public void AddRange(IEnumerable<T> collection)
        {
            lock (Lock)
            {
                List.AddRange(collection);
            }
        }

        public void Reverse()
        {
            lock (Lock)
            {
                List.Reverse();
            }
        }

        public int Count
        {
            get
            {
                lock (Lock)
                {
                    return List.Count;
                }
            }
        }

        public bool IsReadOnly => false;

        public void Add(T item)
        {
            lock (Lock)
            {
                List.Add(item);
            }
        }

        public void Clear()
        {
            lock (Lock)
            {
                List.Clear();
            }
        }

        public bool Contains(T item)
        {
            lock (Lock)
            {
                return List.Contains(item);
            }
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            lock (Lock)
            {
                List.CopyTo(array, arrayIndex);
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            lock (Lock)
            {
                return List.GetEnumerator();
            }
        }

        public int IndexOf(T item)
        {
            lock (Lock)
            {
                return List.IndexOf(item);
            }
        }

        public void Insert(int index, T item)
        {
            lock (Lock)
            {
                List.Insert(index, item);
            }
        }

        public bool Remove(T item)
        {
            lock (Lock)
            {
                return List.Remove(item);
            }
        }

        public T? Find(Predicate<T> match)
        {
            lock (Lock)
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
            lock (Lock)
            {
                List.RemoveAt(index);
            }
        }

        public void RemoveRange(int index, int count)
        {
            lock (Lock)
            {
                List.RemoveRange(index, count);
            }
        }

        /// <summary>
        /// Gets <T> from Unique Name
        /// </summary>
        /// <param name="Name">The Name of the item</param>
        /// <returns></returns>
        public T? Get(string Name)
        {
            lock (Lock)
            {
                return this.Find(x => x.Name == Name);
            }
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            lock (Lock)
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