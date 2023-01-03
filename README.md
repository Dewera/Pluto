## Pluto

A manual system call library that supports functions from both ntdll.dll and win32u.dll

---

### Notable features

- Dynamic resolution of syscall indices from disk
- WOW64 and x64 support

---

### Getting started

The example below demonstrates a basic implementation of the library

```c#
[SyscallImport("ntdll.dll")]
public delegate NtStatus NtClose(nint handle);

var handle = -1;

var syscall = new Syscall<NtClose>();
var status = syscall.Method(handle); 
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
