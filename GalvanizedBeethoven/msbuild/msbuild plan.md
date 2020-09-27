## Partition
### Beethoven.Core
Shared code and definitions.
### Beethoven.Build
Contains msbuild task and compile-time code.
### Beethoven.Runtime
Contains code needed to do runtime compilation.
### User.Interfaces
Interfaces defined by user.
### User.Definitions
Contains user supplied custom definitions.
### User.Generated
User main assembly. 
Contains both definition class and generated code.

## Order
1. Interfaces
1. Beethoven.Core
1. _Beethoven.Generators_
1. Beethoven.Definitions
1. _Imported generators_
1. Imported definitions
1. _Definitions from generator assembly_
1. Generated code
1. Compile output assembly

## References
### Build
1. _Definition assembly_
1. Imported definitions
1. _Imported generators_
1. Beethoven.Generators
1. Beethoven.Definitions
1. Beethoven.Core
1. Interfaces

### Runtime
1. Compile output assembly
1. Imported definitions
1. Beethoven.Definitions
1. Beethoven.Core
1. Interfaces

## Factory pattern
### Placeholder factory method
Return null before code is generated.
Actual factory is:
* Injected compile time
* Loaded runtime
* Linked compile time
### IoC interface
Using an interface IFactory:
``` csharp
public interface IFactory
{
  T Create<T>(params object[] parameters);
}
```
or: 
``` csharp
public interface IFactory<T>
{
  T Create(params object[] parameters);
}
```
Registration code is generated compile time.


