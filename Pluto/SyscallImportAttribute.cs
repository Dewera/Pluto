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

        /// <summary>
        /// Initialises an instance of the <see cref="SyscallImportAttribute"/> class with the DLL name
        /// </summary>
        public SyscallImportAttribute(string dllName)
        {
            DllName = dllName;
        }
    }
}