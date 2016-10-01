using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Tera.Game
{
    public static class Helpers
    {
        public static Func<T, TResult> Memoize<T, TResult>(Func<T, TResult> func)
        {
            var lookup = new ConcurrentDictionary<T, TResult>();
            return x => lookup.GetOrAdd(x, func);
        }

        internal static void On<T>(this object obj, Action<T> callback)
        {
            if (obj is T)
            {
                var castObject = (T) obj;
                callback(castObject);
            }
        }

        internal class ProjectingEqualityComparer<T, TKey> : Comparer<T>
        {
            private readonly Comparer<TKey> _keyComparer = Comparer<TKey>.Default;
            private readonly Func<T, TKey> _projection;

            public ProjectingEqualityComparer(Func<T, TKey> projection)
            {
                _projection = projection;
            }

            public override int Compare(T x, T y)
            {
                return _keyComparer.Compare(_projection(x), _projection(y));
            }
        }

        // Thanks to Natan Podbielski
        //http://internetexception.com/post/2016/08/05/Faster-then-Reflection-Delegates.aspx
        //http://internetexception.com/post/2016/08/16/Faster-than-Reflection-Delegates-Part-2.aspx
        //http://internetexception.com/post/2016/09/02/Faster-than-Reflection-Delegates-Part-3.aspx

        public static TDelegate Contructor<TDelegate>() where TDelegate : class
        {
            var source = typeof(TDelegate).GetGenericArguments().Where(t => !t.IsGenericParameter).ToArray().Last();
            var ctrArgs = typeof(TDelegate).GetGenericArguments().Where(t => !t.IsGenericParameter).ToArray().Reverse().Skip(1).Reverse().ToArray();
            var constructorInfo = GetConstructorInfo(source, ctrArgs);
            if (constructorInfo == null)
            {
                return null;
            }
            var parameters = ctrArgs.Select(Expression.Parameter).ToList();
            return Expression.Lambda(Expression.New(constructorInfo, parameters), parameters).Compile() as TDelegate;
        }

        private static ConstructorInfo GetConstructorInfo(Type source, Type[] types)
        {
            return (source.GetConstructor(BindingFlags.Public, null, types, null) ??
                    source.GetConstructor(BindingFlags.NonPublic, null, types, null)) ??
                   source.GetConstructor(
                       BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null,
                       types, null);
        }
        public static TDelegate InstanceMethod<TDelegate>(object instance, string name, params Type[] typeParameters)
            where TDelegate : class
        {
            var paramsTypes = typeof(TDelegate).GetGenericArguments().Where(t => !t.IsGenericParameter).ToArray();
            var source = instance.GetType();
            var methodInfo = (source.GetMethod(name, BindingFlags.Instance | BindingFlags.Public, null, paramsTypes, null) ??
                     source.GetMethod(name, BindingFlags.Instance | BindingFlags.NonPublic, null, paramsTypes, null)) ??
                    source.GetMethod(name, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public, null,paramsTypes, null);
            if (methodInfo == null)
            {
                return null;
            }
            var expressions = paramsTypes.Select(Expression.Parameter).ToArray();
            Expression returnExpression = Expression.Call(Expression.Constant(instance),
                methodInfo, expressions.Cast<Expression>());
            if (methodInfo.ReturnType != typeof(void) && !methodInfo.ReturnType.IsClass)
            {
                returnExpression = Expression.Convert(returnExpression, typeof(object));
            }
            return Expression.Lambda<TDelegate>(returnExpression, expressions).Compile();
        }
    }
}