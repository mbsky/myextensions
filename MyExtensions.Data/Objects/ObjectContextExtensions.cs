using System.Collections;
using System.ComponentModel;
using System.Data.Common;
using System.Data.EntityClient;
using System.Data.Metadata.Edm;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Reflection;

namespace System.Data.Objects
{
    public static class ObjectContextExtensions
    {
        public static string GetEntitySetName(this ObjectContext context, Type entityType)
        {
            return EntityHelper.GetEntitySetName(entityType, context);
        }

        public static void SetEntityKey(this ObjectContext context, EntityObject entity)
        {
            entity.EntityKey = context.CreateEntityKey(EntityHelper.GetEntitySetName(entity.GetType(), context), entity);
        }

        public static void AttachAsModified(this ObjectContext context, EntityObject current, EntityObject original)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (current == null)
            {
                throw new ArgumentNullException("current");
            }
            if (original == null)
            {
                throw new ArgumentNullException("original");
            }
            if (current.EntityState != EntityState.Detached)
            {
                context.Detach(current);
            }
            Type entityType = current.GetType();
            EntityType type = EntityHelper.GetEntityType(context, entityType);
            PropertyInfo[] source = type.Properties.Select<EdmProperty, PropertyInfo>(delegate(EdmProperty p)
            {
                return entityType.GetProperty(p.Name);
            }).Where<PropertyInfo>(delegate(PropertyInfo p)
            {
                return (p != null);
            }).ToArray<PropertyInfo>();
            PropertyDescriptor[] descriptorArray = type.NavigationProperties.Select<NavigationProperty, PropertyDescriptor>(delegate(NavigationProperty p)
            {
                return TypeDescriptor.GetProperties(entityType)[p.Name];
            }).Where<PropertyDescriptor>(delegate(PropertyDescriptor p)
            {
                return !typeof(IEnumerable).IsAssignableFrom(p.PropertyType);
            }).ToArray<PropertyDescriptor>();
            PropertyDescriptor[] descriptorArray2 = descriptorArray.Select<PropertyDescriptor, PropertyDescriptor>(delegate(PropertyDescriptor p)
            {
                return EntityHelper.GetReferenceProperty(p);
            }).ToArray<PropertyDescriptor>();
            object[] objArray = source.Select<PropertyInfo, object>(delegate(PropertyInfo p)
            {
                return p.GetValue(current, null);
            }).ToArray<object>();
            EntityKey[] keyArray = descriptorArray2.Select<PropertyDescriptor, EntityReference>(delegate(PropertyDescriptor p)
            {
                return (EntityReference)p.GetValue(current);
            }).Select<EntityReference, EntityKey>(delegate(EntityReference p)
            {
                if (p == null)
                {
                    return null;
                }
                return p.EntityKey;
            }).ToArray<EntityKey>();
            for (int i = 0; i < source.Length; i++)
            {
                source[i].SetValue(current, source[i].GetValue(original, null), null);
            }
            for (int j = 0; j < descriptorArray2.Length; j++)
            {
                EntityReference reference = (EntityReference)descriptorArray2[j].GetValue(current);
                EntityReference reference2 = (EntityReference)descriptorArray2[j].GetValue(original);
                EntityKey entityKey = reference2.EntityKey;
                EntityObject obj2 = (EntityObject)descriptorArray[j].GetValue(current);
                if ((obj2 != null) && (obj2.EntityKey != entityKey))
                {
                    descriptorArray[j].SetValue(current, null);
                }
                reference.EntityKey = entityKey;
            }
            context.Attach(current);
            for (int k = 0; k < source.Length; k++)
            {
                PropertyInfo info2 = source[k];
                object objA = objArray[k];
                object objB = info2.GetValue(original, null);
                if (!object.Equals(objA, objB))
                {
                    info2.SetValue(current, objA, null);
                }
            }
            for (int m = 0; m < descriptorArray2.Length; m++)
            {
                EntityReference reference3 = (EntityReference)descriptorArray2[m].GetValue(current);
                EntityKey key2 = keyArray[m];
                if (!object.Equals(reference3.EntityKey, key2))
                {
                    reference3.EntityKey = key2;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectContext"></param>
        /// <param name="command"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static int ExecuteCommand(this ObjectContext objectContext,
                                string command, CommandType commandType)
        {
            DbConnection connection = ((EntityConnection)objectContext.Connection).StoreConnection;
            bool opening = (connection.State == ConnectionState.Closed);
            if (opening)
                connection.Open();

            DbCommand cmd = connection.CreateCommand();
            cmd.CommandText = command;
            cmd.CommandType = commandType;
            try
            {
                return cmd.ExecuteNonQuery();
            }
            finally
            {
                if (opening && connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="esqlQuery">sample : "SELECT VALUE AVG(p.ListPrice) FROM AdventureWorksEntities.Product as p" </param>
        /// <param name="act"></param>
        public static void ExecuteReader(this ObjectContext context, string esqlQuery, Action<IExtendedDataRecord> act)
        {

            EntityConnection conn = (EntityConnection)context.Connection;

            conn.Open();

            using (EntityCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = esqlQuery;
                // Execute the command.
                using (EntityDataReader rdr =
                    cmd.ExecuteReader(CommandBehavior.SequentialAccess))
                {
                    // Start reading results.
                    while (rdr.Read())
                    {
                        act(rdr as IExtendedDataRecord);
                    }
                }
            }

            conn.Close();
        }
    }
}
