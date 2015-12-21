using System;
using System.Collections.Generic;

namespace LRUCache
{

    public class LRUCache<K,V>
    {
        private int capacity;
        private readonly IDictionary<K, LinkedListNode<LRUCachedItem<K, V>>> cachedObjects;
        private readonly LinkedList<LRUCachedItem<K, V>> lruList;
        private object locker = new object();

        public LRUCache(int capacity)
        {
            this.capacity = capacity;
            this.cachedObjects = new Dictionary<K, LinkedListNode<LRUCachedItem<K, V>>>(capacity);
            this.lruList = new LinkedList<LRUCachedItem<K, V>>();
        }

        public V Get(K key)
        {
            lock (locker)
            {
                LinkedListNode<LRUCachedItem<K, V>> node;
                if (cachedObjects.TryGetValue(key, out node))
                {
                    V value = node.Value.Value;
                    lruList.Remove(node);
                    lruList.AddLast(node);
                    return value;
                }
                return default(V);
            }
        }


        public ICollection<V> Get(K[] keys)
        {
            ICollection<V> foundValues = new List<V>(keys.Length);
            lock (locker)
            {
                for (var i = 0; i < keys.Length; i++)
                {
                    var key = keys[i];
                    LinkedListNode<LRUCachedItem<K, V>> node;
                    if (cachedObjects.TryGetValue(key, out node))
                    {
                        V value = node.Value.Value;
                        lruList.Remove(node);
                        lruList.AddLast(node);
                        foundValues.Add(value);
                    }
                }
            }
            return foundValues;
        }

        public void Add(K key, V val)
        {
            lock (locker)
            {
                if (cachedObjects.Count >= capacity)
                {
                    RemoveLastUsed();
                }
                LRUCachedItem<K, V> cacheItem = new LRUCachedItem<K, V>(key, val);
                LinkedListNode<LRUCachedItem<K, V>> node = new LinkedListNode<LRUCachedItem<K, V>>(cacheItem);
                lruList.AddLast(node);
                cachedObjects.Add(key, node);
            }
        }

        protected virtual V RemoveLastUsed()
        {
            LinkedListNode<LRUCachedItem<K,V>> node = lruList.First;
            lruList.RemoveFirst();
            cachedObjects.Remove(node.Value.Key);

            return node.Value.Value;
        }
    }

    class LRUCachedItem<K,V>
    {
        public K Key;
        public V Value;

        public LRUCachedItem(K key, V value)
        {
            Key = key;
            Value = value;
        }
    }
}


