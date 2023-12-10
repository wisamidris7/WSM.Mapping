using System;
using System.Collections.Generic;
using System.Reflection;

namespace WSM.Mapping
{
    public class WSMMapping
    {
        public static object Map(Type source, Type des, object obj)
        {
            if (obj == null) return null;
            if (source == des) return obj;
            object result = Activator.CreateInstance(des);
            foreach (PropertyInfo sourcePropertyInfo in source.GetProperties())
            {
                var desPropertyInfo = des.GetProperty(sourcePropertyInfo.Name);
                if (desPropertyInfo != null)
                {
                    if (IsSimpleType(sourcePropertyInfo.PropertyType))
                    {
                        desPropertyInfo.SetValue(result, sourcePropertyInfo.GetValue(obj));
                    }
                    else
                    {
                        var nestedSource = sourcePropertyInfo.GetValue(obj);
                        var nestedDes = Map(sourcePropertyInfo.PropertyType, desPropertyInfo.PropertyType, nestedSource);
                        desPropertyInfo.SetValue(result, nestedDes);
                    }
                }
            }
            return result;
        }

        public static TDes Map<TDes>(object source) where TDes : class, new()
        {
            return Map(source.GetType(), typeof(TDes), source) as TDes;
        }

        public static List<TDes> MapList<TDes, TSource>(List<TSource> listSource) where TDes : class, new()
        {
            List<TDes> list = new List<TDes>();
            foreach (TSource source in listSource)
            {
                list.Add(Map<TDes>(source));
            }
            return list;
        }

        private static bool IsSimpleType(Type type)
        {
            return type.IsPrimitive || type.IsEnum || type == typeof(string) || type == typeof(decimal) || type == typeof(DateTime);
        }
    }
}