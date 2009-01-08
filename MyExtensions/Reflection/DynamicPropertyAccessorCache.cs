using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Reflection
{
    public class DynamicPropertyAccessorCache
    {
        private object m_mutex = new object();
        private Dictionary<Type, Dictionary<string, DynamicPropertyAccessor>> m_cache =
            new Dictionary<Type, Dictionary<string, DynamicPropertyAccessor>>();

        public DynamicPropertyAccessor GetAccessor(Type type, string propertyName)
        {
            DynamicPropertyAccessor accessor;
            Dictionary<string, DynamicPropertyAccessor> typeCache;

            if (this.m_cache.TryGetValue(type, out typeCache))
            {
                if (typeCache.TryGetValue(propertyName, out accessor))
                {
                    return accessor;
                }
            }

            lock (m_mutex)
            {
                if (!this.m_cache.ContainsKey(type))
                {
                    this.m_cache[type] = new Dictionary<string, DynamicPropertyAccessor>();
                }

                accessor = new DynamicPropertyAccessor(type, propertyName);
                this.m_cache[type][propertyName] = accessor;

                return accessor;
            }
        }

    }
}
