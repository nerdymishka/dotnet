using System;
using System.Linq;
using System.Reflection;
using Mettle;
using NerdyMishka.Reflection;
#pragma warning disable CS0649
#pragma warning disable CS0414
#pragma warning disable CA1823

public class ReflectionMethodTests
{
    private IAssert assert;

    public ReflectionMethodTests(IAssert assert)
    {
        this.assert = assert;
    }

    [UnitTest]
    public void Ctor_ThrowsArgumentNull()
    {
        assert.Throws<ArgumentNullException>(() =>
        {
            _ = new ReflectionProperty(null);
        });
    }

    [UnitTest]
    public void Ctor()
    {
        var first = ReflectData.GetMethod();
        var reflect = new ReflectionMethod(
            first,
            new ReflectionFactory());

        assert.Equal(first.Name, reflect.Name);
        assert.Equal(first.DeclaringType, reflect.ClrType);
        assert.Equal(first.ReturnParameter, reflect.ReturnParameter.ParameterInfo);
        assert.Equal(false, reflect.IsGenericMethodDefinition);
        assert.Equal(0, reflect.GenericArguments.Count);

        var mods = reflect.ModifierAccess;
        assert.Equal(true, mods.IsPublic);
        assert.Equal(false, mods.IsPrivate);
        assert.Equal(false, mods.IsInternal);
        assert.Equal(false, mods.IsVirtual);
        assert.Equal(false, mods.IsStatic);
        assert.Equal(true, mods.IsInstance);
    }

    [UnitTest]
    public void FindAttribute()
    {
        var first = ReflectData.GetMethod();
        var reflect = new ReflectionMethod(first, new ReflectionFactory());

        var attr = reflect.FindAttribute<DecoratorAttribute>();
        assert.NotNull(attr);
    }

    [UnitTest]
    public void FindAttributes()
    {
        var first = ReflectData.GetMethod();
        var reflect = new ReflectionMethod(first, new ReflectionFactory());

        var attrs = reflect.FindAttributes<DecoratorAttribute>();
        assert.NotNull(attrs);
        assert.Equal(1, attrs.Count);
    }

    [UnitTest]
    public void Invoke()
    {
        var first = ReflectData.GetMethod();
        var reflect = new ReflectionMethod(first, new ReflectionFactory());
        var data = new ReflectData();
        var result = reflect.Invoke(data, "first", "last", DateTime.UtcNow);
        assert.NotNull(result);
        assert.IsType<string>(result);
        assert.Equal("Mike Tyson", result.ToString());
    }

    [AttributeUsage(AttributeTargets.Method)]
    internal class DecoratorAttribute : System.Attribute
    {
    }

    internal class ReflectData
    {
        public static MethodInfo GetMethod(string methodName = "GetUser")
        {
            var t = typeof(ReflectData);
            var method = t.GetTypeInfo().GetDeclaredMethod(methodName);
            return method;
        }

        [Decorator]
        public string GetUser(string firstName, string lastName, DateTime birthdate)
        {
            return "Mike Tyson";
        }
    }
}
