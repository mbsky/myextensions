using System.Linq;

namespace System.Data.Objects.DataClasses {

    public static class EntityObjectExtensions
    {
        /// <summary> 
        /// Loads the entity collection if it hasn't already been loaded 
        /// </summary> 
        /// <typeparam name="T">Type of entity collection</typeparam> 
        /// <param name="entityCollection">Entity collection to potentially load entities into</param> 
        /// <param name="entitySource">The source entity which has the entity collection relationship (modified or unchanged only)</param> 
        public static void EnsureLoaded<T>(this EntityCollection<T> entityCollection, EntityObject entitySource) where T : class, IEntityWithRelationships
        {
            if (entitySource != null && entityCollection != null && !entityCollection.IsLoaded)
            {
                if (entitySource.EntityState == System.Data.EntityState.Modified || entitySource.EntityState == System.Data.EntityState.Unchanged)
                {
                    entityCollection.Load();
                }
            }
        }

        /// <summary> 
        /// Whether or not the entity reference has an entity key with a value present 
        /// </summary> 
        public static bool HasEntityKeyFirstValue<T>(this EntityReference<T> entityReference) where T : class, IEntityWithRelationships
        {
            return entityReference != null && entityReference.EntityKey.HasFirstValue<int>();
        }

        /// <summary> 
        /// Get entity key with a value present 
        /// </summary> 
        public static int GetEntityKeyFirstValue<T>(this EntityReference<T> entityReference) where T : class, IEntityWithRelationships
        {
            if (entityReference != null)
                return entityReference.EntityKey.GetFirstValue<int>();
            return 0;
        }

        /// <summary> 
        /// Gets the first entity key value 
        /// </summary> 
        /// <returns>the first entity key value</returns> 
        public static T GetFirstValue<T>(this EntityKey entityKey)
        {
            if (entityKey != null && entityKey.EntityKeyValues != null && entityKey.EntityKeyValues.Length > 0)
                return (T)entityKey.EntityKeyValues.First().Value;
            return default(T);
        }

        /// <summary> 
        /// Sets the first entity key value 
        /// </summary> 
        public static void SetFirstValue<T>(this EntityKey entityKey, T value)
        {
            if (entityKey != null && entityKey.EntityKeyValues != null && entityKey.EntityKeyValues.Length > 0)
                entityKey.EntityKeyValues.First().Value = value;
            return;
        }

        /// <summary> 
        /// Whether or not the entity key has a first value 
        /// </summary> 
        public static bool HasFirstValue<T>(this EntityKey entityKey)
        {
            var firstValue = GetFirstValue<T>(entityKey);
            var defaultValue = default(T);
            return (!firstValue.Equals(defaultValue));
        }


    }
}
