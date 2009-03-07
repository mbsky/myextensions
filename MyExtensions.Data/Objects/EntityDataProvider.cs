using System.Data.Objects.DataClasses;

namespace System.Data.Objects
{
    public class EntityDataProvider<TEntity, TContext> : ObjectDataProvider<TContext>
        where TEntity : EntityObject, new()
        where TContext : ObjectContext, new()
    {

        public EntityDataProvider()
            : base(new TContext())
        {
        }

        public EntityDataProvider(TContext customContext)
            : base(customContext)
        {
        }

        #region Get

        public virtual TEntity Get(params EntityKeyMember[] Keys)
        {
            return base.Get<TEntity>(MergeOption.AppendOnly, Keys);
        }

        public virtual TEntity Get(MergeOption mergeOption, params EntityKeyMember[] Keys)
        {

            return base.Get<TEntity>(mergeOption, Keys);
        }

        public virtual TEntity Get(params ObjectParameter[] prm)
        {
            return Get<TEntity>(MergeOption.AppendOnly, prm);
        }

        public virtual TEntity Get(MergeOption mergeOption, params ObjectParameter[] prm)
        {
            return base.Get<TEntity>(mergeOption, prm);
        }

        #endregion

        #region InsertOnSubmit

        public virtual void InsertOnSubmit(params TEntity[] models)
        {
            base.InsertOnSubmit<TEntity>(models);
        }
        #endregion

        #region Update

        public virtual void UpdateOnSubmit(TEntity entity)
        {

            base.UpdateOnSubmit<TEntity>(entity);
        }

        public virtual void UpdateOnSubmit(TEntity entity, TEntity originalEntity)
        {

            base.UpdateOnSubmit<TEntity>(entity, originalEntity);
        }

        #endregion

        #region Delete

        public virtual void MarkToDelete(TEntity model)
        {

            base.MarkToDelete<TEntity>(model);
        }

        #endregion
    }
}
