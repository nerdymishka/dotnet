# Design

## Extension Methods on Common types

Public extension methods for a type like `Array` or `System.String` MUST reside
in a namespace that requires an explicit namespace import statement.

Good.

```csharp
using NerdyMishka.Util.Arrays;
```

Bad.

```csharp
using Nerdmishka.Collections; // common namespace
```
