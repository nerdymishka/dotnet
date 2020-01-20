using System;
using System.Reflection;

namespace Xunit
{
    internal static class ExecutionHelper
    {
        /// <summary>
        /// Gets the substitution token used as assembly name suffix to indicate that the assembly is
        /// a generalized reference to the platform-specific assembly.
        /// </summary>
        public const string SubstitutionToken = ".{Platform}";

        private const string ExecutionAssemblyNamePrefix = "xunit.execution.";
#if NETSTANDARD
        private static readonly string[] PlatformSuffixes = new[] { "dotnet", "MonoAndroid", "iOS-Universal" };
#endif

        private static readonly object Lock = new object();

        private static string platformSuffix = "__unknown__";

        public static string PlatformSuffix
        {
            get
            {
                lock (Lock)
                {
                    if (platformSuffix == "__unknown__")
                    {
                        platformSuffix = null;

#if NETSTANDARD
                        foreach (var suffix in PlatformSuffixes)
                        {
                            try
                            {
                                Assembly.Load(new AssemblyName { Name = ExecutionAssemblyNamePrefix + suffix });
                                platformSuffix = suffix;
                                break;
                            }
#pragma warning disable CA1031 // Do not catch general exception types
                            catch
#pragma warning restore CA1031 // Do not catch general exception types
                            {
                            }
                        }

#else
                        foreach (var name in AppDomain.CurrentDomain.GetAssemblies().Select(a => a?.GetName()?.Name))
                            if (name != null && name.StartsWith(executionAssemblyNamePrefix, StringComparison.Ordinal))
                            {
                                platformSuffix = name.Substring(executionAssemblyNamePrefix.Length);
                                break;
                            }
#endif
                    }
                }

                if (platformSuffix == null)
                    throw new InvalidOperationException($"Could not find any xunit.execution.* assembly loaded in the current context");

                return platformSuffix;
            }
        }
    }
}