using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;

namespace SyntaxEditor.Utils
{
    public static class ObservableExtension
    {
        public static ObservableCollection<TSource> ToObservable<TSource>(this IEnumerable<TSource> source)
        {
            return new ObservableCollection<TSource>(source.ToList());
        }
    }

    public static class TypeExtension
    {
        public static bool IsInstanceOfGenericType(this object instance, Type genericType)
        {
            Type type = instance.GetType();
            while (type != null)
            {
                if (type.IsGenericType && type.GetGenericTypeDefinition() == genericType.GetGenericTypeDefinition())
                {
                    return true;
                }
                type = type.BaseType;
            }
            return false;
        }
    }

    public static class FuncExtension
    {
        public static O InvokeGeneric<I, O>(this Expression<Func<I, O>> fun, object arg)
        {
            if (fun.Body is MethodCallExpression mc)
            {
                var aType = arg.GetType().GetGenericArguments()[0];
                var gen = mc.Method.GetGenericMethodDefinition();
                var del = gen.MakeGenericMethod(aType);
                var obj = (ConstantExpression)mc.Object;
                return (O)del.Invoke(obj.Value, new[] { arg });
            }
            return default;
        }
    }
}
