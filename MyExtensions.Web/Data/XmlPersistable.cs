//===============================================================================================
//
// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Permissive License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// All other rights reserved.
//
//===============================================================================================

using System;
using System.Web;
using System.IO;
using System.Xml.Serialization;
using System.Web.Caching;
using System.Reflection;

namespace System.Web.Data
{
    /// <summary>
    /// Generic class for a perssitable object. Encapsulates the logic to load/save data from/to the filesystem. To speed up the acces, caching is used.
    /// </summary>
    /// <typeparam name="T">class or struct with all the data-fields that must be persisted</typeparam>
    public class XmlPersistable<T> : BasePersistable<T>
    {

        //cache the XmlSerializer if you need to use it often

        string getCachedsSerializerKey = typeof(T).FullName + "_XmlPersistable";

        protected virtual XmlSerializer CachedsSerializer
        {
            get
            {
                object o = HttpContext.Current.Cache[getCachedsSerializerKey];
                if (o == null)
                {
                    o = new XmlSerializer(typeof(T));
                    InsertCache(getCachedsSerializerKey, o, null);
                }

                return (XmlSerializer)o;
            }
        }

        protected override void Deserialize(string path, out T _data)
        {
            using (FileStream reader = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                _data = (T)CachedsSerializer.Deserialize(reader);
            }
        }

        protected override void Serialize(string path, T _data)
        {
            using (FileStream writer = File.Create(path))
            {
                CachedsSerializer.Serialize(writer, _data);
            }
        }
    }
}
