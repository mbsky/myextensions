using System.Collections.Generic;
using System.Reflection.Emit;

namespace System.Reflection
{
    public delegate object CreateInstanceHandler(object[] parameters);

    public class ObjectFactory
    {
        public static T CreateInstance<T>()
        {
            return CreateInstance<T>(null);
        }

        static Dictionary<string, CreateInstanceHandler> mHandlers = new Dictionary<string, CreateInstanceHandler>();

        public static T CreateInstance<T>(params object[] parameters)
        {
            Type objtype = typeof(T);
            Type[] ptypes = GetParameterTypes(parameters);
            string key = typeof(T).FullName + "_" + GetKey(ptypes);
            if (!mHandlers.ContainsKey(key))
            {
                CreateHandler(objtype, key, ptypes);
            }
            return (T)mHandlers[key](parameters);

        }

        static void CreateHandler(Type objtype, string key, Type[] ptypes)
        {
            lock (typeof(ObjectFactory))
            {
                if (!mHandlers.ContainsKey(key))
                {
                    DynamicMethod dm = new DynamicMethod(key, typeof(object), new Type[] { typeof(object[]) }, typeof(ObjectFactory).Module);
                    ILGenerator il = dm.GetILGenerator();
                    ConstructorInfo cons = objtype.GetConstructor(ptypes);

                    il.Emit(OpCodes.Nop);
                    for (int i = 0; i < ptypes.Length; i++)
                    {
                        il.Emit(OpCodes.Ldarg_0);
                        il.Emit(OpCodes.Ldc_I4, i);
                        il.Emit(OpCodes.Ldelem_Ref);
                        if (ptypes[i].IsValueType)
                        {
                            il.Emit(OpCodes.Unbox_Any, ptypes[i]);
                        }
                        else
                        {
                            il.Emit(OpCodes.Castclass, ptypes[i]);
                        }


                    }
                    il.Emit(OpCodes.Newobj, cons);
                    il.Emit(OpCodes.Ret);
                    CreateInstanceHandler ci = (CreateInstanceHandler)dm.CreateDelegate(typeof(CreateInstanceHandler));
                    mHandlers.Add(key, ci);

                }
            }
        }
        static Type[] GetParameterTypes(params object[] parameters)
        {
            if (parameters == null)
                return new Type[0];
            Type[] values = new Type[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                values[i] = parameters[i].GetType();
            }
            return values;
        }
        static string GetKey(params Type[] types)
        {
            if (types == null || types.Length == 0)
                return "null";
            return string.Concat(types);
        }

    }
}
