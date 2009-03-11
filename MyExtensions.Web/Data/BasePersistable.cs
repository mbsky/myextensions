using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Web;
using System.IO;
using System.Web.Caching;

namespace System.Web.Data
{
    public abstract class BasePersistable<T>
    {
        private String _path;

        protected T _data;

        public T Data
        {
            get
            {
                return _data;
            }
        }

        /// <summary>
        /// Creates a instance of Persistable. Also creates a instance of T
        /// </summary>
        public BasePersistable()
        {
            _data = ObjectFactory.CreateInstance<T>();
        }

        /// <summary>
        /// Creates a instance of Persistable.
        /// </summary>
        public BasePersistable(bool loadFirst)
        {
            if (loadFirst)
                LoadData();

            if (null == _data)
                _data = ObjectFactory.CreateInstance<T>();
        }

        public BasePersistable(T data)
        {
            this._data = data;
        }

        protected abstract void Deserialize(string path, out T _data);

        /// <summary>
        /// Loads the data from the filesystem. For deserialization a XmlSeralizer is used.
        /// </summary>
        protected void LoadData()
        {
            _path = HttpContext.Current.Server.MapPath(GetDataFilename());
            lock (_path)
            {
                //first check, if the object is maybe already in the cache
                object o = HttpContext.Current.Cache[_path];
                if (o != null)
                {
                    _data = (T)o;
                }
                else
                {
                    if (File.Exists(_path))
                    {
                        //if nothing was found in the cache, the data must be loaded from the disk
                        //load and deserialize the data from the filesystem
                        //using (FileStream reader = File.Open(_path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                        //{
                        //    XmlSerializer serializer = new XmlSerializer(typeof(T));
                        //    _data = (T)serializer.Deserialize(reader);
                        //}

                        Deserialize(_path, out _data);

                        InsertCache();
                    }
                    else
                    {
                        if (_data == null)

                            _data = ObjectFactory.CreateInstance<T>();

                        this.SaveData();
                    }
                }
            }
        }

        private void InsertCache()
        {
            InsertCache(_path, _data, new CacheDependency(_path));
        }

        protected virtual void InsertCache(string key, object data, CacheDependency cd)
        {
            HttpContext.Current.Cache.Insert(key, data, cd, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(15), CacheItemPriority.Normal, null);
        }

        protected abstract void Serialize(string path, T _data);

        /// <summary>
        /// Persists the data back to the filesystem
        /// </summary>
        public void SaveData()
        {
            _path = HttpContext.Current.Server.MapPath(GetDataFilename());
            lock (_path)
            {
                try
                {
                    //if the given path does not exist yet, create it
                    if (!Directory.Exists(Path.GetDirectoryName(_path)))
                        Directory.CreateDirectory(Path.GetDirectoryName(_path));

                    //serialize and store the data to the filesystem

                    //using (FileStream writer = File.Create(_path))
                    //{
                    //    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    //    serializer.Serialize(writer, _data);
                    //}

                    Serialize(_path, _data);

                    //insert the data into the cache
                    InsertCache();

                }
                catch (Exception  /*UnauthorizedAccessException*/ ex)
                {
                    throw ex;
                }
            }
        }

        //Deletes the data from the cache and filesystem
        public virtual bool Delete()
        {
            bool success = true;

            if (File.Exists(_path))
            {
                lock (_path)
                {
                    try
                    {
                        File.Delete(_path);
                        HttpContext.Current.Cache.Remove(_path);
                    }
                    catch { success = false; }
                }
            }
            return success;
        }

        public virtual string GetDataFilename()
        {
            return string.Format("~/App_Data/{0}.config", typeof(T).Name);
        }
    }
}
