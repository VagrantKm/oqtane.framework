﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using Oqtane.Shared;

// ReSharper disable once CheckNamespace
namespace System.Reflection
{
    public static class AssemblyExtensions
    {
        public static IEnumerable<Type> GetInterfaces<TInterfaceType>(this Assembly assembly)
        {
            if (assembly is null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            return assembly.GetTypes(typeof(TInterfaceType));
        }

        public static IEnumerable<Type> GetTypes(this Assembly assembly, Type interfaceType)
        {
            if (assembly is null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            if (interfaceType is null)
            {
                throw new ArgumentNullException(nameof(interfaceType));
            }

            return assembly.GetTypes()
                //.Where(t => t.GetInterfaces().Contains(interfaceType));
                .Where(x => interfaceType.IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract);
        }
        
        public static IEnumerable<T> GetInstances<T>(this Assembly assembly) where T : class
        {
            if (assembly is null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }
            var type = typeof(T);
            var list = assembly.GetTypes()
                .Where(x => type.IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract && !x.IsGenericType);

            foreach (var type1 in list)
            {
                if (Activator.CreateInstance(type1) is T instance) yield return instance;
            }
        }

        public static bool IsOqtaneAssembly(this Assembly assembly)
        {
            return assembly.FullName != null && (assembly.FullName.Contains("oqtane", StringComparison.OrdinalIgnoreCase));
        }

        public static bool IsOqtaneAssembly(this FileInfo fileInfo)
        {
            return (fileInfo.Name.Contains("oqtane", StringComparison.OrdinalIgnoreCase));
        }

        public static IEnumerable<Assembly> GetOqtaneAssemblies(this AppDomain appDomain)
        {
            return appDomain.GetAssemblies().Where(a => a.IsOqtaneAssembly());
        }
    }
}
