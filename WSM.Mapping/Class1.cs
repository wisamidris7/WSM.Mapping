using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace WSM.Mapping
{
    public class WSMMapping
    {
        public static object Map(Type source, Type des, object obj)
        {
            if (obj == null) return null;
            object result;
            result = Activator.CreateInstance(des)!;
            foreach (PropertyInfo sourcePropertyInfo in source.GetType().GetProperties())
            {
                var desPropertyInfo = des.GetProperty(sourcePropertyInfo.Name);
                if (desPropertyInfo != null && desPropertyInfo.PropertyType == sourcePropertyInfo.PropertyType)
                {
                    desPropertyInfo!.SetValue(result, sourcePropertyInfo.GetValue(source));
                }
                else if (desPropertyInfo != null)
                {
                    var desPropertyType = desPropertyInfo.PropertyType;
                    var sourcePropertyType = sourcePropertyInfo.PropertyType;
                    if (desPropertyType.IsGenericType && sourcePropertyType.IsGenericType)
                    {
                        var desGenericType = desPropertyType.GetGenericTypeDefinition();
                        var sourceGenericType = sourcePropertyType.GetGenericTypeDefinition();
                        if (desGenericType == typeof(List<>) && sourceGenericType == typeof(List<>))
                        {
                            var desGenericArgument = desPropertyType.GetGenericArguments()[0];
                            var sourceGenericArgument = sourcePropertyType.GetGenericArguments()[0];
                            var desList = Activator.CreateInstance(desPropertyType);
                            var sourceList = sourcePropertyInfo.GetValue(source);
                            foreach (var sourceListItem in (IEnumerable<object>)sourceList!)
                            {
                                var desListItem = Activator.CreateInstance(desGenericArgument);
                                foreach (var sourceListItemPropertyInfo in sourceListItem.GetType().GetProperties())
                                {
                                    var desListItemPropertyInfo = desGenericArgument.GetProperty(sourceListItemPropertyInfo.Name);
                                    if (desListItemPropertyInfo != null && desListItemPropertyInfo.PropertyType == sourceListItemPropertyInfo.PropertyType)
                                        desListItemPropertyInfo.SetValue(desListItem, sourceListItemPropertyInfo.GetValue(sourceListItem));
                                }
                                desList!.GetType().GetMethod("Add")!.Invoke(desList, new[] { desListItem });
                            }
                            desPropertyInfo.SetValue(result, desList);
                        }
                    }
                    else if (desPropertyType != sourcePropertyType)
                        desPropertyInfo.SetValue(result, Map(sourcePropertyType, desPropertyType, sourcePropertyInfo.GetValue(source)));
                }
            }
            return result;
        }
        public static TDes Map<TDes, TSource>(TSource source) where TDes : class, new()
        {
            return Map(typeof(TSource), typeof(TDes), source) is TDes des ? des : default;
        }
        public static List<TDes> MapList<TDes, TSource>(List<TSource> listSource) where TDes : class, new()
        {
            List<TDes> list = new List<TDes>();
            foreach (TSource source in listSource)
            {
                list.Add(Map<TDes, TSource>(source));
            }
            return list;
        }
    }
}
