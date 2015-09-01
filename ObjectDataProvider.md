# Introduction #

ObjectDataProvider Usage.

# Details #

use ObjectDataProvider with ObjectContext:

Basic Sample:
<pre>
public class MyEntitiesDataProvider : ObjectDataProvider<MyEntities><br>
{<br>
}<br>
</pre>

Functions Include in Your MyEntitiesDataProvider class:

  * **GetEntitySetName**
  * **SetEntityKey**
  * **GetEntityName**
  * **CreateQuery**
  * **EnsureLoaded**
  * **GetMethods**
> > /// Get an EntityObject by an array of EntityKeyMember.
> > <pre>public virtual TEntity Get<TEntity>(params EntityKeyMember[] Keys) where TEntity : EntityObject, new()</pre>
> > /// optional MergeOption
> > <pre>public virtual TEntity Get<TEntity>(MergeOption mergeOption, params EntityKeyMember[] Keys) where TEntity : EntityObject, new()</pre>
> > /// Get an EntityObject by an array of ObjectParameter.
> > <pre>public virtual TEntity Get<TEntity>(params ObjectParameter[] prm) where TEntity : EntityObject, new()</pre>
  * **GetOriginalValue**

Batch Update Functions:
  * **InsertOnSubmit**
  * **UpdateOnSubmit**
  * **MarkToDelete**
  * **ApplyChanges**