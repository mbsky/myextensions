using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Caching;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace System.Web.Data
{
    public class JsonPersistable<T> : BasePersistable<T>
    {
        static JsonConverter[] defautNewtonConverters = new JsonConverter[] { new JavaScriptDateTimeConverter() };

        static JsonSerializerSettings defautNewtonSerializerSettings = new JsonSerializerSettings
        {
            Converters = defautNewtonConverters
            ,
            DefaultValueHandling = DefaultValueHandling.Ignore
            ,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            ,
            MissingMemberHandling = MissingMemberHandling.Ignore
            ,
            NullValueHandling = NullValueHandling.Ignore
            ,
            ObjectCreationHandling = ObjectCreationHandling.Auto
        };

        protected override void Deserialize(string path, out T _data)
        {
            using (StreamReader reader = new StreamReader(path))
            {
                _data = JsonConvert.DeserializeObject<T>(reader.ReadToEnd(), defautNewtonSerializerSettings);

                reader.Close();
            }
        }

        protected override void Serialize(string path, T _data)
        {
            //serializer.Converters.Add(new JavaScriptDateTimeConverter());
            //serializer.NullValueHandling = NullValueHandling.Ignore;

            //using (StreamWriter sw = new StreamWriter(path))
            //using (JsonWriter writer = new JsonTextWriter(sw))
            //{
            //    serializer.Serialize(writer, _data);
            //}
            //保存对象
            string jsonText = JsonConvert.SerializeObject(_data, Formatting.Indented, defautNewtonSerializerSettings);
            using (StreamWriter sw = new StreamWriter(path, false))
            {
                sw.Write(jsonText);
            }
        }
    }
}
