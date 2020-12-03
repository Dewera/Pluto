## Pluto

![](https://github.com/Dewera/Pluto/workflows/Continuous%20Integration/badge.svg)

A manual syscall library that supports both ntdll.dll and win32u.dll

---

### Getting started

The example below demonstrates a basic implementation of the library

```c#
[SyscallImport("ntdll.dll", "NtTestAlert")]
public delegate NtStatus TestAlert();

var syscall = new Syscall<TestAlert>();

var status = syscall.Method();
```

---

### Syscall Class

Provides the functionality to syscall a function in a DLL

```c#
public sealed class Syscall<T> where T : Delegate
```

### Properties

A delegate wrapping the syscall

```c#
Method
```

---

### SyscallImportAttribute Class

Indicates that the attributed delegate represents a syscall signature

```c#
[AttributeUsage(AttributeTargets.Delegate)]
public sealed class SyscallImportAttribute : Attribute
```

### Constructors

Indicates that the attributed delegate represents a syscall signature

```c#
public SyscallImportAttribute(string, string)
```
