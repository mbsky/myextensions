using System.Collections.Specialized;

namespace System.Configuration
{
    /// <summary>
    /// AppSettings Helper (made in Oxite : http://www.codeplex.com/Oxite)
    /// </summary>
    public class AppSettingsHelper
    {
        private NameValueCollection settings;

        public AppSettingsHelper() : this(ConfigurationManager.AppSettings) { }

        public AppSettingsHelper(NameValueCollection settings)
        {
            this.settings = settings;
        }

        public string this[string name]
        {
            get { return settings[name]; }
        }

        public string this[int index]
        {
            get { return settings[index]; }
        }

        public string Get(string key)
        {
            return Get(key, null);
        }

        public string Get(string key, string defaultValue)
        {
            string val = settings[key];

            if (val == null)
            {
                if (defaultValue == null)
                    throw new ArgumentException(string.Format("AppSetting '{0}' was not found.", key));
                else
                    return defaultValue;
            }
            else
                return val;
        }

        public byte GetByte(string key)
        {
            return GetByte(key, null);
        }

        public byte GetByte(string key, byte defaultValue)
        {
            return GetByte(key, (byte?)defaultValue);
        }

        private byte GetByte(string key, byte? defaultValue)
        {
            if (!defaultValue.HasValue)
                return byte.Parse(Get(key));
            else
            {
                string value = Get(key, "");

                if (value == "")
                    return defaultValue.Value;
                else
                    return byte.Parse(value);
            }
        }

        public short GetInt16(string key)
        {
            return GetInt16(key, null);
        }

        public short GetInt16(string key, short defaultValue)
        {
            return GetInt16(key, (short?)defaultValue);
        }

        private short GetInt16(string key, short? defaultValue)
        {
            if (!defaultValue.HasValue)
                return short.Parse(Get(key));
            else
            {
                string value = Get(key, "");

                if (value == "")
                    return defaultValue.Value;
                else
                    return short.Parse(value);
            }
        }

        public int GetInt32(string key)
        {
            return GetInt32(key, null);
        }

        public int GetInt32(string key, int defaultValue)
        {
            return GetInt32(key, (int?)defaultValue);
        }

        private int GetInt32(string key, int? defaultValue)
        {
            if (!defaultValue.HasValue)
                return int.Parse(Get(key));
            else
            {
                string value = Get(key, "");

                if (value == "")
                    return defaultValue.Value;
                else
                    return int.Parse(value);
            }
        }

        public long GetInt64(string key)
        {
            return GetInt64(key, null);
        }

        public long GetInt64(string key, long defaultValue)
        {
            return GetInt64(key, (long?)defaultValue);
        }

        private long GetInt64(string key, long? defaultValue)
        {
            if (!defaultValue.HasValue)
                return long.Parse(Get(key));
            else
            {
                string value = Get(key, "");

                if (value == "")
                    return defaultValue.Value;
                else
                    return long.Parse(value);
            }
        }

        public bool GetBoolean(string key)
        {
            return GetBoolean(key, null);
        }

        public bool GetBoolean(string key, bool defaultValue)
        {
            return GetBoolean(key, (bool?)defaultValue);
        }

        private bool GetBoolean(string key, bool? defaultValue)
        {
            if (!defaultValue.HasValue)
            {
                return bool.Parse(Get(key));
            }
            else
            {
                string returnValue = Get(key, "");

                if (returnValue == "")
                    return defaultValue.Value;
                else
                    return bool.Parse(returnValue);
            }
        }

        public string[] GetArray(string key)
        {
            return GetArray(key, (string[])null);
        }

        public string[] GetArray(string key, params char[] delimeter)
        {
            return GetArray(key, null, delimeter);
        }

        public string[] GetArray(string key, string[] defaultValue)
        {
            return GetArray(key, defaultValue, ',');
        }

        public string[] GetArray(string key, string[] defaultValue, params char[] delimeter)
        {
            if (defaultValue == null)
            {
                /// ToDo: Caching

                return Get(key).Split(delimeter);
            }
            else
            {
                string returnValue = Get(key, "");

                if (returnValue == "")
                    return defaultValue;
                else
                    //TODO: (erikpo) Add "caching"
                    return returnValue.Split(delimeter);
            }
        }

        public Guid GetGuid(string key)
        {
            return GetGuid(key, Guid.Empty);
        }

        public Guid GetGuid(string key, Guid defaultValue)
        {
            return GetGuid(key, (Guid?)defaultValue);
        }

        public Guid GetGuid(string key, Guid? defaultValue)
        {
            string strValue = Get(key, "");
            Guid returnValue = Guid.Empty;

            if (!strValue.IsNullOrEmpty())
            {
                if (strValue.IsGuid(out returnValue))
                {
                    return returnValue;
                }
            }

            if (Guid.Empty == returnValue)
            {
                if (defaultValue.HasValue)
                {
                    return defaultValue.Value;
                }
            }

            return returnValue;

        }
    }
}
