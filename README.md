## Pluto

![](https://github.com/Dewera/Pluto/workflows/Continuous%20Integration/badge.svg)

A manual system call library that supports functions from both ntdll.dll and win32u.dll

---

### Notable features

- Dynamic resolution of syscall indices from disk
- x86 and x64 support

---

### Getting started

The example below demonstrates a basic implementation of the library

```c#
[SyscallImport("ntdll.dll")]
public delegate NtStatus NtFlushInstructionCache(SafeProcessHandle processHandle, IntPtr address, int bytes);

var syscall = new Syscall<NtFlushInstructionCache>();

var processHandle = Process.GetProcessesByName("")[0].SafeHandle;

var status = syscall.Method(processHandle, IntPtr.Zero, 0); 
```

---

### Syscall Class

Provides the functionality to syscall a function in a DLL

```c#
public sealed class Syscall<T> where T : Delegate
```

### Constructors

Initialises an instance of the `Syscall<T>` class with the syscall delegate

```C#
public Syscall();
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

Initialises an instance of the `SyscallImportAttribute` class with the DLL name

```c#
public SyscallImportAttribute(string);
```
