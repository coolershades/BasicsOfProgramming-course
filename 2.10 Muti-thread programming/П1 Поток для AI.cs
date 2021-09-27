using System;
using System.Collections.Generic;
using System.Linq;

namespace rocket_bot
{
    public class Channel<T> where T : class
    {
        private readonly List<T> channel = new List<T>();
        
        public T this[int index]
        {
            get
            {
                lock (channel)
                {
                    if (index < 0 || index >= channel.Count) return null;
                    return channel[index];
                }
            }
            set
            {
                lock (channel)
                {
                    if (index == channel.Count)
                        channel.Add(value);
                    else
                    {
                        channel.RemoveRange(index, channel.Count - index);
                        channel.Add(value);
                    }
                }
            }
        }

        public T LastItem()
        {
            lock (channel)
            {
                return channel.Count == 0 ? null : channel.Last();
            }
        }
        
        public void AppendIfLastItemIsUnchanged(T item, T knownLastItem)
        {
            lock (channel)
            {
                if (Equals(knownLastItem, LastItem()))
                    channel.Add(item);
            }
        }

        public int Count
        {
            get
            {
                lock (channel)
                {
                    return channel.Count;
                }
            }
        }
    }
}