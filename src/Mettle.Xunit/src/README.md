# ⚒ Mettle.Xunit ⚗

Mettle is a custom Test Framework for Xunit.net that extends and overrides
XUnit to enable:

- DI for constructors and test methods.
- Custom test attributes and automatic traits based on those attributes.
- Provides an IAssert class that enables the use of extension methods
  and injection into the test method parameter or test class constructor.

## Rationale

Besides being lazy, I wrote mettle to be more productive in writing tests. Xunit
does provide DI through the use of IClassFixture, but its cumbersome to use
and who want to create a ton of base classes for testing?

.NET Core and the Microsoft.Extensions libraries have really underscored the
use of DI in creating libraries and applications.  

JUnit and other testing frameworks like Qunit.  XUnit

## Custom Attributes

- `[MettleXunitFramework]` an attribute that instructs the Xunit test runner
   to use the Mettle test framework and runner for a given assembly.
- `[TestCase]` an abstract attribute that inherits from `[Fact]`. Any attribute
  that inherits from `TestCaseAttribute` will enable dependency injection for
  test methods and test class constructors. The attributes adds the
  following properties:
  - **Ticket** - The url to a ticket for this test.
  - **Id** - The ticket id or an identifier for filtering purposes.
  - **Tags** - Annotates the test with tags delmited by semicolon `;`.
  - **SkipReason** - A clearer version of Skip.
- `[UnitTest]` a drop in replacement for `[Fact]` that tags the test with 'unit'
  for the tag and category traits.
- `[IntegrationTest]` tags the test with 'integration' for the tag and category
   traits.
- `[Functional]` tags the test with 'functional' for the tag and category
  traits.
- `[ServiceProviderFactory]` an attribute that can be applied at the assembly,
  class, or method level that specifies a type that implements `Mettle.IServiceProviderFactory`



## Sample Service Provider Implementation

Note: Any DI framework that implements `IServiceProvider` can replace
the code in the `CreateProvider()` method.

```csharp
[assembly:MettleXunitFramework]
[assembly:ServiceProviderFactory(typeof(Mettle.SimpleServiceFactory))]

using System;
using System.Collections.Concurrent;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Mettle
{
    public class SimpleServiceFactory : IServiceProviderFactory
    {
        public IServiceProvider CreateProvider()
        {
            return new SimpleServiceProvider();
        }
    }

    public class SimpleServiceProvider : IServiceProvider
    {
        private ConcurrentDictionary<Type, Func<IServiceProvider, object>>
            factories =
            new ConcurrentDictionary<Type, Func<IServiceProvider, object>>();

        public SimpleServiceProvider()
        {
            factories.TryAdd(typeof(IAssert), (s) => {
                return AssertImpl.Current; });

            factories.TryAdd(typeof(ITestOutputHelper), (s) => {
                return new TestOutputHelper(); });
        }

        public void AddSingleton(Type type, object instance)
        {
            this.factories.TryAdd(type, (s) => instance);
        }

        public void AddTransient(Type type)
        {
            this.factories.TryAdd(type, (s) => Activator.CreateInstance(type));
        }

        public void AddTransient(Type type, Func<IServiceProvider, object> activator)
        {
            this.factories.TryAdd(type, activator);
        }

        public object GetService(Type type)
        {
            if(this.factories.TryGetValue(type,
                out Func<IServiceProvider, object> factory))
                return factory(this);

            if(type.IsValueType)
                return Activator.CreateInstance(type);

            return null;
        }
    }
}
```

```csharp
using Mettle;

namespace Tests
{
    public class UnitTest1
    {
        private IAssert Assert;

        public UnitTest1(IAssert assert)
        {
            this.Assert = assert;
        }

        [UnitTest]
        public void Test1(IAssert assert)
        {
            assert.Ok("a" == "a");
            Console.Write("Test");
        }

        [UnitTest]
        [ServiceProviderFactory(typeof(ServiceBuilder2))]
        public void Test3(UnitTestData data)
        {
            var assert = AssertImpl.Current;
            assert.NotNull(data);
        }

        [Fact]
        public void Test2()
        {
            Console.Write("Test");
        }
    }
}
```


[Main README](../../../README.md)

[LICENSE](../../../LICENSE)