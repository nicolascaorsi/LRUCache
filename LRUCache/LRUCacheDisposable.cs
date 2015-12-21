using System;

namespace LRUCache
{
    public class LRUCacheDisposable<K, V> : LRUCache<K,V> where V: class, IDisposable
    {
        public LRUCacheDisposable(int capacity)
            : base(capacity)
        {
        }

        protected override V RemoveLastUsed()
        {
            var removed = base.RemoveLastUsed();
            try
            {
                removed.Dispose();
                // Analysis disable once EmptyGeneralCatchClause
            }
            catch
            {
                // Ignores if cant dispose object
            }
            return removed;
        }
    }
}

