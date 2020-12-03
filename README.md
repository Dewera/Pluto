## Pluto

![](https://github.com/Dewera/Pluto/workflows/Continuous%20Integration/badge.svg)

A manual syscall library that supports both ntdll.dll and win32u.dll

---

### Getting started

The example below demonstrates a basic implementation of the library

```c#
[SyscallImport("ntdll.dll", "NtWriteVirtualMemory")]
public delegate NtStatus WriteProcessMemory(SafeProcessHandle processHandle, IntPtr address, in byte bytes, int size, out int bytesWritten);

var syscall = new Syscall<WriteProcessMemory>();

var processHandle = Process.GetProcessesByName("")[0].SafeHandle;

var bytes = new byte[0];

var status = syscall.Method(processHandle, IntPtr.Zero, in bytes[0], bytes.Length, out _);
```

---

### Syscall Class

Provides the functionality to syscall a function in a DLL

```c#
public sealed class Syscall<T> where T : Delegate
```

### Constructors

Initialises an instance of the class with the syscall delegate

```C#
public Syscall()
```

### Properties

A delegate wrapping the syscall

```c#
public T Method { get; }
```

---

### SyscallImportAttribute Class

Indicates that the attributed delegate represents a syscall signature

```c#
[AttributeUsage(AttributeTargets.Delegate)]
public sealed class SyscallImportAttribute : Attribute
```

### Constructors

Initialises an instance of the SyscallImportAttribute class with the DLL name and function name

```c#
public SyscallImportAttribute(string, string)
```
