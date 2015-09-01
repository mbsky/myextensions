# Introduction #

Get Methods in ObjectDataProvider.


# Details #

They are:

  * Get an EntityObject by an array of EntityKeyMember.
> > <pre>public virtual TEntity Get<TEntity>(params EntityKeyMember[] Keys) where TEntity : EntityObject, new()</pre>
  * optional MergeOption
> > <pre>public virtual TEntity Get<TEntity>(MergeOption mergeOption, params EntityKeyMember[] Keys) where TEntity : EntityObject, new()</pre>
  * Get an EntityObject by an array of ObjectParameter.
> > <pre>public virtual TEntity Get<TEntity>(params ObjectParameter[] prm) where TEntity : EntityObject, new()</pre>