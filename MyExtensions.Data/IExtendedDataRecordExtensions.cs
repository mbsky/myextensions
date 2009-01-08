using System.Data.Metadata.Edm;

namespace System.Data
{
    public static class IExtendedDataRecordExtensions
    {
        public static void RefTypeVisitRecord(this IExtendedDataRecord record, Action<EntityKeyMember> act)
        {
            // For RefType the record contains exactly one field.
            int fieldIndex = 0;

            // If the field is flagged as DbNull, the shape of the value is undetermined.
            // An attempt to get such a value may trigger an exception.
            if (record.IsDBNull(fieldIndex) == false)
            {
                BuiltInTypeKind fieldTypeKind = record.DataRecordInfo.FieldMetadata[fieldIndex].
                    FieldType.TypeUsage.EdmType.BuiltInTypeKind;
                //read only fields that contain PrimitiveType
                if (fieldTypeKind == BuiltInTypeKind.RefType)
                {
                    // Ref types are surfaced as EntityKey instances. 
                    // The containing record sees them as atomic.
                    EntityKey key = record.GetValue(fieldIndex) as EntityKey;
                    // Get the EntitySet name.

                    // Get the Name and the Value information of the EntityKey.
                    foreach (EntityKeyMember keyMember in key.EntityKeyValues)
                    {
                        act(keyMember);
                    }
                }
            }
        }
    }
}
