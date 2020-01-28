using System;
using System.Linq;
using System.Reflection;
using Mettle;
using NerdyMishka.Reflection;

public class ReflectionParameterTests
{
    private IAssert assert;

    public ReflectionParameterTests(IAssert assert)
    {
        this.assert = assert;
    }

    [UnitTest]
    public void Ctor_ThrowsArgumentNull()
    {
        assert.Throws<ArgumentNullException>(() =>
        {
            _ = new ReflectionParameter(null);
        });
    }

    [UnitTest]
    public void Ctor()
    {
        var first = ReflectData
            .GetParameters("GetUser")
            .FirstOrDefault();
        var reflectionParameter = new ReflectionParameter(first);

        assert.Equal(first.Name, reflectionParameter.Name);
        assert.Equal(first.ParameterType, reflectionParameter.ClrType);
        assert.Equal(first.Position, reflectionParameter.Position);
        assert.Equal(first.IsOut, reflectionParameter.IsOut);
        assert.Equal(first.IsOptional, reflectionParameter.IsOptional);
        assert.Equal(first.DefaultValue, reflectionParameter.DefaultValue);
        assert.NotNull(reflectionParameter.Attributes);
    }

    [UnitTest]
    public void FindAttribute()
    {
        var first = ReflectData
            .GetParameters("GetUser")
            .FirstOrDefault();
        var reflectionParameter = new ReflectionParameter(first);

        var attr = reflectionParameter.FindAttribute<DecoratorAttribute>();
        assert.NotNull(attr);
    }

    [UnitTest]
    public void FindAttributes()
    {
        var first = ReflectData
            .GetParameters("GetUser")
            .FirstOrDefault();
        var reflectionParameter = new ReflectionParameter(first);

        var attrs = reflectionParameter.FindAttributes<DecoratorAttribute>();
        assert.NotNull(attrs);
        assert.Equal(1, attrs.Count);
    }

    [AttributeUsage(AttributeTargets.Parameter)]
    internal class DecoratorAttribute : System.Attribute
    {
    }

    internal class ReflectData
    {
        public static ParameterInfo[] GetParameters(string methodName)
        {
            var t = typeof(ReflectData);
            var method = t.GetTypeInfo().GetDeclaredMethod(methodName);
            var parameters = method.GetParameters();

            return parameters;
        }

        public void GetUser([Decorator] string firstName, string lastName, DateTime birthdate)
        {
        }
    }
}
