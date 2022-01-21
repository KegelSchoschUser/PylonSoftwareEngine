using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PylonGameEngine.Utilities
{
    public class GameDictionary<T> : IEnumerable<T> where T : class
    {
        public List<T> List = new List<T>();
        public object sync = new object();

        public void Add(T value)
        {
            lock (sync)
            {
                List.Add(value);
                //Console.WriteLine("ITEM ITEM ITEM: " + List.IndexOf(value));
            }
        }

        public void Add(List<T> Values)
        {
            foreach (T value in Values)
            {
                lock (sync)
                {
                    List.Add(value);
                }
            }
        }

        public T this[int index]
        {
            get
            {
                lock (sync)
                {
                    for (int i = index; i < Count - index; i++)
                    {
                        if (List[i] is null)
                        {
                            continue;
                        }

                        else
                        {
                            return List[i];
                        }

                    }

                    return null;
                }
            }
            set
            {
                lock (sync)
                {
                    for (int i = index; i < Count - index; i++)
                    {
                        if (List[i] is null)
                        {
                            continue;
                        }

                        else
                        {
                            List[i] = value;
                        }

                    }
                }
            }
        }

        public int GetIndex(T value)
        {
            dynamic val = value;
            for (int i = 0; i < Count; i++)
            {
                if (List[i] == val)
                {
                    return i;
                }
            }

            return -1;
        }

        public void Remove(T value)
        {
            lock (sync)
            {
                dynamic val = value;
                T item = List.First(kvp => kvp == val);
                item = null;
            }
        }



        public void RemoveByIndex(int index)
        {
            lock (sync)
            {
                for (int i = index; i < List.Count; i++)
                {
                    if (List[i] is null)
                    {

                        continue;
                    }

                    else
                    {
                        //Console.WriteLine(Count);
                        //Console.WriteLine("will be deleted: " + i + "          " + List[i]);
                        List[i] = null;
                        //Console.WriteLine(Count);
                        return;
                    }

                }

            }
        }

        public T Last
        {
            get
            {
                lock (sync)
                {
                    return List.Last();
                }

            }
        }

        public int LastIndex
        {
            get
            {
                lock (sync)
                {
                    return Count - 1;
                }

            }
        }

        public bool Contains(T item)
        {
            lock (sync)
            {
                return List.Contains(item);
            }
        }

        public T Find(Predicate<T> pred)
        {
            lock (sync)
            {
                try
                {
                    return List.Find(pred);
                }
                catch
                {
                    return null;
                }
            }
        }

        public int Count
        {
            get
            {
                lock (sync)
                {
                    int count = 0;
                    foreach (T item in List)
                    {
                        if (!(item == null))
                        {
                            count++;
                        }
                    }

                    return count;
                }
            }
        }



        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("GameDictionary: {");
            for (int i = 0; i < Count; i++)
            {
                sb.AppendLine(i.ToString() + ": ");
                if (List[i] != null)
                {
                    sb.AppendLine("     " + List[i].ToString());
                }
                else
                {
                    sb.AppendLine("     " + "[EMPTY]");
                }
            }
            sb.Append("}");

            return sb.ToString();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new MyEnumerator(List);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new MyEnumerator(List);
        }

        public class MyEnumerator : IEnumerator<T>
        {
            private int nIndex;
            private List<T> collection = new List<T>();
            public MyEnumerator(List<T> coll)
            {
                collection = coll;
                nIndex = -1;
            }

            public object Current => collection[nIndex];

            T IEnumerator<T>.Current => collection[nIndex];

            public void Dispose()
            {
                collection = null;
            }

            public bool MoveNext()
            {
                nIndex++;
                return (nIndex < collection.Count);
            }

            public void Reset()
            {
                throw new System.NotImplementedException();
            }
        }
    }
}