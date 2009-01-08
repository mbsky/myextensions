using System.Collections.Generic;

namespace System.Reflection
{
    public static class TypeExtensions
    {
        #region IsNullableType
        public static bool IsNullableType(this Type theType)
        {
            return (theType.IsGenericType && theType.
              GetGenericTypeDefinition().Equals
              (typeof(Nullable<>)));
        } 
        #endregion

        #region GetOriginalTypeOfNullableType
        public static Type GetOriginalTypeOfNullableType(this Type type)
        {
            Check.Require(type, "type");

            if (type.ToString().StartsWith("System.Nullable`1["))
            {
                return GetType(type.ToString().Substring("System.Nullable`1[".Length).Trim('[', ']'));
            }

            return type;
        } 
        #endregion

        #region DeepGet
        /// <summary>
        /// Deeply get properties of specific types
        /// </summary>
        /// <param name="types">The types</param>
        /// <returns>The property infos</returns>
        public static PropertyInfo[] DeepGetProperties(params Type[] types)
        {
            if (types == null || types.Length == 0)
            {
                return new PropertyInfo[0];
            }
            List<PropertyInfo> list = new List<PropertyInfo>();
            foreach (Type t in types)
            {
                if (t != null)
                {
                    foreach (PropertyInfo pi in t.GetProperties())
                    {
                        list.Add(pi);
                    }

                    if (t.IsInterface)
                    {
                        Type[] interfaceTypes = t.GetInterfaces();

                        if (interfaceTypes != null)
                        {
                            foreach (PropertyInfo pi in DeepGetProperties(interfaceTypes))
                            {
                                bool isContained = false;

                                foreach (PropertyInfo item in list)
                                {
                                    if (item.Name == pi.Name)
                                    {
                                        isContained = true;
                                        break;
                                    }
                                }

                                if (!isContained)
                                {
                                    list.Add(pi);
                                }
                            }
                        }
                    }
                    else
                    {
                        Type baseType = t.BaseType;

                        if (baseType != typeof(object) && baseType != typeof(ValueType))
                        {
                            foreach (PropertyInfo pi in DeepGetProperties(baseType))
                            {
                                bool isContained = false;

                                foreach (PropertyInfo item in list)
                                {
                                    if (item.Name == pi.Name)
                                    {
                                        isContained = true;
                                        break;
                                    }
                                }

                                if (!isContained)
                                {
                                    list.Add(pi);
                                }
                            }
                        }
                    }
                }
            }

            return list.ToArray();
        }

        /// <summary>
        /// Deeply get fields of specific fields
        /// </summary>
        /// <param name="types">The types</param>
        /// <returns>The field infos</returns>
        public static FieldInfo[] DeepGetFields(params Type[] types)
        {
            if (types == null || types.Length == 0)
            {
                return new FieldInfo[0];
            }
            List<FieldInfo> list = new List<FieldInfo>();
            foreach (Type t in types)
            {
                if (t != null)
                {
                    foreach (FieldInfo fi in t.GetFields())
                    {
                        list.Add(fi);
                    }

                    if (t.IsInterface)
                    {
                        Type[] interfaceTypes = t.GetInterfaces();

                        if (interfaceTypes != null)
                        {
                            foreach (FieldInfo fi in DeepGetFields(interfaceTypes))
                            {
                                bool isContained = false;

                                foreach (FieldInfo item in list)
                                {
                                    if (item.Name == fi.Name)
                                    {
                                        isContained = true;
                                        break;
                                    }
                                }

                                if (!isContained)
                                {
                                    list.Add(fi);
                                }
                            }
                        }
                    }
                    else
                    {
                        Type baseType = t.BaseType;

                        if (baseType != typeof(object) && baseType != typeof(ValueType))
                        {
                            foreach (FieldInfo fi in DeepGetFields(baseType))
                            {
                                bool isContained = false;

                                foreach (FieldInfo item in list)
                                {
                                    if (item.Name == fi.Name)
                                    {
                                        isContained = true;
                                        break;
                                    }
                                }

                                if (!isContained)
                                {
                                    list.Add(fi);
                                }
                            }
                        }
                    }
                }
            }

            return list.ToArray();
        }

        /// <summary>
        /// Deeply get property info from specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        public static PropertyInfo DeepGetProperty(this Type type, string propertyName)
        {
            foreach (PropertyInfo pi in DeepGetProperties(type))
            {
                if (pi.Name == propertyName)
                {
                    return pi;
                }
            }

            return null;
        }

        /// <summary>
        /// Deeps the get field from specific type.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="includePublic"></param>
        /// <param name="includeNonPublic"></param>
        /// <param name="isStatic"></param>
        /// <returns></returns>
        public static FieldInfo DeepGetField(this Type type, string name, bool includePublic, bool includeNonPublic, bool isStatic)
        {
            Type t = type;

            if (t != null)
            {
                BindingFlags flags = BindingFlags.Instance;
                if (includePublic)
                    flags |= BindingFlags.Public;
                if (includeNonPublic)
                    flags |= BindingFlags.NonPublic;
                if (isStatic)
                    flags |= BindingFlags.Static;
                FieldInfo fi = t.GetField(name, flags);
                if (fi != null)
                {
                    return fi;
                }

                if (t.IsInterface)
                {
                    Type[] interfaceTypes = t.GetInterfaces();

                    if (interfaceTypes != null)
                    {
                        foreach (Type interfaceType in interfaceTypes)
                        {
                            fi = DeepGetField(interfaceType, name, includePublic, includeNonPublic, isStatic);
                            if (fi != null)
                            {
                                return fi;
                            }
                        }
                    }
                }
                else
                {
                    Type baseType = t.BaseType;

                    if (baseType != typeof(object) && baseType != typeof(ValueType))
                    {
                        return DeepGetField(baseType, name, includePublic, includeNonPublic, isStatic);
                    }
                }
            }
            return null;
        }
        #endregion

        #region GetType

        /// <summary>
        /// Gets a type in all loaded assemblies of current app domain.
        /// </summary>
        /// <param name="fullName">The full name.</param>
        /// <returns></returns>
        public static Type GetType(string fullName)
        {
            if (string.IsNullOrEmpty(fullName))
            {
                return null;
            }

            Type t = null;

            if (fullName.StartsWith("System.Nullable`1["))
            {
                string genericTypeStr = fullName.Substring("System.Nullable`1[".Length).Trim('[', ']');
                if (genericTypeStr.Contains(","))
                {
                    genericTypeStr = genericTypeStr.Substring(0, genericTypeStr.IndexOf(",")).Trim();
                }
                t = typeof(Nullable<>).MakeGenericType(GetType(genericTypeStr));

                if (t != null)
                {
                    return t;
                }
            }

            try
            {
                t = Type.GetType(fullName);
            }
            catch
            {
            }

            if (t == null)
            {
                try
                {
                    if (fullName.Contains(","))
                    {
                        string[] classNameAssembly = fullName.Split(',');
                        Assembly ass = Assembly.LoadFrom(classNameAssembly[1]);
                        if (ass != null)
                            t = ass.GetType(classNameAssembly[0]);
                    }
                    else
                    {
                        Assembly[] asses = AppDomain.CurrentDomain.GetAssemblies();

                        for (int i = asses.Length - 1; i >= 0; i--)
                        {
                            Assembly ass = asses[i];
                            try
                            {
                                t = ass.GetType(fullName);
                            }
                            catch
                            {
                            }

                            if (t != null)
                            {
                                break;
                            }
                        }
                    }
                }
                catch
                {
                }
            }

            return t;
        }


        #endregion
    }
}
