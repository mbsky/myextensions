using System.Collections.Generic;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Reflection.Emit;

namespace System.Data.Objects
{
    public class ObjectDataProvider<TContext> : IDisposable
        where TContext : ObjectContext, new()
    {
        #region ObjectContext
        /// <summary>
        /// 
        /// </summary>
        public virtual TContext ObjectContext { get; private set; }

        public ObjectDataProvider()
            : this(new TContext())
        {
        }

        public ObjectDataProvider(TContext customContext)
        {
            if (null != customContext)
            {
                this.ObjectContext = customContext;
            }
        }
        #endregion

        #region GetEntitySetName

        public virtual string GetEntitySetName<TEntity>() where TEntity : EntityObject, new()
        {
            return EntityHelper.GetEntitySetName(typeof(TEntity), this.ObjectContext);
        }

        #endregion

        #region SetEntityKey

        private void SetEntityKey<TEntity>(TEntity entity) where TEntity : EntityObject, new()
        {
            entity.EntityKey = this.ObjectContext.CreateEntityKey(GetEntitySetName<TEntity>(), entity);
        }
        #endregion

        #region GetEntityName
        public virtual string GetEntityName<TEntity>() where TEntity : EntityObject, new()
        {
            return typeof(TEntity).Name;
        }
        #endregion

        #region CreateQuery

        protected virtual ObjectQuery<TEntity> innerCreateQuery<TEntity>() where TEntity : EntityObject, new()
        {
            ObjectQuery<TEntity> qryMain = this.ObjectContext.CreateQuery<TEntity>("[" + GetEntityName<TEntity>() + "]");

            return qryMain;
        }

        public ObjectQuery<TEntity> CreateQuery<TEntity>() where TEntity : EntityObject, new()
        {
            return CreateQuery<TEntity>(MergeOption.AppendOnly);
        }

        public ObjectQuery<TEntity> CreateQuery<TEntity>(MergeOption mergeOption) where TEntity : EntityObject, new()
        {
            ObjectQuery<TEntity> qryMain = innerCreateQuery<TEntity>();

            qryMain.MergeOption = mergeOption;

            return qryMain;
        }

        #endregion

        #region EnsureLoaded
        private T LoadByKey<T>(object entityKey)
        {
            if (entityKey == null)
                throw new ArgumentNullException("Supplied entity key is null, unable to load entity", "entityKey");
            // make sure the object is loaded in the object context 

            EntityKey key = (EntityKey)entityKey;
            ObjectStateEntry entry;

            if (!this.ObjectContext.ObjectStateManager.TryGetObjectStateEntry(entityKey, out entry) || entry.Entity == null)
            {
                return (T)ObjectContext.GetObjectByKey(key);
            }
            return (T)entry.Entity;
        }

        /// <summary> 
        /// Loads the entity reference or its value if it hasn't already been loaded. 
        /// </summary> 
        /// <typeparam name="T">Type of entity reference</typeparam> 
        /// <param name="entitySource">The source entity which has the entity reference relationship (added, modified or unchanged only)</param> 
        public void EnsureLoaded<T>(EntityReference<T> entityReference, EntityObject entitySource) where T : class, IEntityWithRelationships
        {
            if (entitySource != null && entityReference != null && !entityReference.IsLoaded && entityReference.EntityKey != null)
            {
                if (entitySource.EntityState == System.Data.EntityState.Added) // add the value directly as load will throw 
                {
                    if (entityReference.Value == null)
                        entityReference.Value = LoadByKey<T>(entityReference.EntityKey);
                }
                else if (entitySource.EntityState == System.Data.EntityState.Modified || entitySource.EntityState == System.Data.EntityState.Unchanged)
                {
                    entityReference.Load();
                }
            }
        }
        #endregion

        #region Get

        public virtual TEntity Get<TEntity>(params EntityKeyMember[] Keys) where TEntity : EntityObject, new()
        {
            return Get<TEntity>(MergeOption.AppendOnly, Keys);
        }

        public virtual TEntity Get<TEntity>(MergeOption mergeOption, params EntityKeyMember[] Keys) where TEntity : EntityObject, new()
        {

            List<ObjectParameter> plst = new List<ObjectParameter>(Keys.Length);

            foreach (EntityKeyMember k in Keys)
            {
                ObjectParameter par = new ObjectParameter(k.Key, k.Value);

                plst.Add(par);
            }

            return Get<TEntity>(mergeOption, plst.ToArray());
        }

        public virtual TEntity Get<TEntity>(params ObjectParameter[] prm) where TEntity : EntityObject, new()
        {
            return Get<TEntity>(MergeOption.AppendOnly, prm);
        }

        public virtual TEntity Get<TEntity>(MergeOption mergeOption, params ObjectParameter[] prm) where TEntity : EntityObject, new()
        {
            string where = string.Empty;

            string tpl = "it.{0}=@{0}";

            foreach (ObjectParameter pm in prm)
            {
                if (where != string.Empty)
                {
                    where += " and ";
                }

                where += string.Format(tpl, pm.Name);
            }

            ObjectQuery<TEntity> qry = CreateQuery<TEntity>(mergeOption);

            return qry.Where(where, prm).FirstOrDefault();
        }

        #endregion

        #region GetOriginalValue
        /// <summary> 
        /// Gets the original value for a modified entity object's property 
        /// </summary> 
        /// <returns>the value before the property was modified</returns> 
        public T GetOriginalValue<T>(EntityObject entityObject, string propertyName)
        {
            if (entityObject == null)
                return default(T);

            if (entityObject.EntityState == EntityState.Modified)
            {

                ObjectStateEntry stateEntry = null;

                this.ObjectContext.ObjectStateManager.TryGetObjectStateEntry(entityObject, out stateEntry);

                if (stateEntry != null)
                    return (T)stateEntry.OriginalValues.GetValue(stateEntry.OriginalValues.GetOrdinal(propertyName));

            }

            // return the value of the property 

            // TODO: DynamicMethod
            return (T)entityObject.GetType().GetProperty(propertyName).GetValue(entityObject, null);
        }
        #endregion

        #region InsertOnSubmit
        public virtual void InsertOnSubmit<TEntity>(TEntity model) where TEntity : EntityObject, new()
        {
            this.ObjectContext.AddObject(GetEntityName<TEntity>(), model);
        }

        public virtual void InsertOnSubmit<TEntity>(params TEntity[] models) where TEntity : EntityObject, new()
        {
            foreach (TEntity entity in models)
            {
                this.InsertOnSubmit(entity);
            }
        }
        #endregion

        #region Update

        public virtual void UpdateOnSubmit<TEntity>(TEntity entity) where TEntity : EntityObject, new()
        {
            if (null == entity.EntityKey)
                SetEntityKey(entity);

            TEntity OriginalEntity = (TEntity)this.ObjectContext.GetObjectByKey(entity.EntityKey);

            UpdateOnSubmit<TEntity>(entity, OriginalEntity);
        }

        public virtual void UpdateOnSubmit<TEntity>(TEntity entity, TEntity originalEntity) where TEntity : EntityObject, new()
        {
            if (null == entity.EntityKey)
                SetEntityKey(entity);

            if (null != originalEntity && originalEntity.EntityState != EntityState.Detached)
            {
                this.ObjectContext.Detach(originalEntity);
            }

            this.ObjectContext.AttachAsModified(entity, originalEntity);
        }

        #endregion

        #region Delete

        public virtual void MarkToDelete<TEntity>(TEntity model) where TEntity : EntityObject, new()
        {
            if (null == model.EntityKey)

                SetEntityKey(model);

            TEntity toDelete = (TEntity)this.ObjectContext.GetObjectByKey(model.EntityKey);

            if (null != toDelete)
            {
                this.ObjectContext.DeleteObject(toDelete);
            }
        }

        #endregion

        #region ApplyChanges

        public virtual int ApplyChanges()
        {
            return ApplyChanges(RefreshMode.StoreWins);
        }

        public virtual int ApplyChanges(RefreshMode refreshMode)
        {
            int i = 0;
            try
            {
                i = this.ObjectContext.SaveChanges();
            }
            catch (OptimisticConcurrencyException ex)
            {
                foreach (var state in ex.StateEntries)
                {
                    this.ObjectContext.Refresh(refreshMode, state.Entity);
                }

                i = this.ObjectContext.SaveChanges(false);

            }

            return i;
        }
        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            if (null != this.ObjectContext)
            {
                ObjectContext.Dispose();

                ObjectContext = null;
            }
        }

        #endregion

    }
}
