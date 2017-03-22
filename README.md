# Unit test file generator

Lightweight library for generating unit test files.

The time used for writing test setup can now be used for thinking better testing scenarios !

### Info

The library uses [Code Dom](https://msdn.microsoft.com/en-us/library/y2k85ax6(v=vs.110).aspx) for generating unit test files.
It takes the absolute path to the dll as input, and outputs the tests in the current directory.
Test file names contain the full namespace of the files tested.

### Usage

The utility aids in reducing the repetitive work when writing unit tests by auto-generating the setup.

- [x] Declares class dependencies
- [x] Declares test methods for all public methods
- [x] Constructs the class under test instance
- [x] Uses AAA syntax
- [x] Implements a default call for the method under test
- [x] Keeps tests consistent with code style naming
- [x] Adds random values for value types/ initializes reference type parameters

```
C:\TestsGenerator\bin\Debug>TestsGenerator.exe "C:\Projects\bin\Debug\Project.dll"
```