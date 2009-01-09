using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.IO;

namespace System.Web.Mvc
{

    public enum JsonProvider
    {
        Microsoft,
        Newtonsoft
    }

    /// <summary>
    /// JsonResultEx is a Custom JsonResult with Selectable Serializer and SerializerSettings.
    /// </summary>
    public class JsonResultEx : JsonResult
    {

        // TODO : JsonProvider From AppSetting

        public JsonResultEx()
            : this(JsonProvider.Microsoft)
        {
        }

        public JsonResultEx(JsonProvider provider)
            : this(JsonProvider.Microsoft, null)
        {
        }

        public JsonResultEx(JsonProvider provider, JsonSerializerSettings settings)
        {
            this.Provider = provider;

            if (null == settings)
            {
                this.SerializerSettings = defautNewtonSerializerSettings;
            }
            else
            {
                this.SerializerSettings = settings;
            }
        }

        public virtual JsonProvider Provider { get; private set; }

        public virtual JsonSerializerSettings SerializerSettings { get; private set; }

        /// <summary>
        /// Microsoft  Serializer
        /// </summary>
        static JavaScriptSerializer serializer = new JavaScriptSerializer();

        static JsonConverter[] defautNewtonConverters = new JsonConverter[] { new JavaScriptDateTimeConverter() };

        // TODO: defautNewtonSerializerSettings From AppSetting

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

        protected virtual string GetJsonString(object data)
        {
            if (data != null)
            {
                switch (Provider)
                {
                    default:
                        return serializer.Serialize(data);
                    case JsonProvider.Newtonsoft:
                        return JsonConvert.SerializeObject(data, Formatting.Indented, this.SerializerSettings);
                }
            }

            return null;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            HttpResponseBase response = context.HttpContext.Response;

            if (!String.IsNullOrEmpty(ContentType))
            {
                response.ContentType = ContentType;
            }
            else
            {
                response.ContentType = "application/json";
            }
            if (ContentEncoding != null)
            {
                response.ContentEncoding = ContentEncoding;
            }

            if (Data != null)
            {
                string jsonData = GetJsonString(Data);
                response.Write(jsonData);
            }

        }
    }
}
