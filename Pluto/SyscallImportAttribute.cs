using System;

namespace Pluto
{
    /// <summary>
    /// Indicates that the attributed delegate represents a syscall signature
    /// </summary>
    [AttributeUsage(AttributeTargets.Delegate)]
    public sealed class SyscallImportAttribute : Attribute
    {
        internal string DllName { get; }

        internal string FunctionName { get; }

        /// <summary>
        /// Indicates that the attributed delegate represents a syscall signature
        /// </summary>
        public SyscallImportAttribute(string dllName, string functionName)
        {
            DllName = dllName;

            FunctionName = functionName;
        }
    }
}