using System;
using System.Linq;
using System.Reflection;
using Mettle;
using NerdyMishka.Reflection;
#pragma warning disable CS0649
#pragma warning disable CS0414
#pragma warning disable CA1823

public class ReflectionPropertyTests
{
    private IAssert assert;

    public ReflectionPropertyTests(IAssert assert)
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
        var first = ReflectData
            .GetProperties()
            .FirstOrDefault();
        var reflect = new ReflectionProperty(first);

        assert.Equal(first.Name, reflect.Name);
        assert.Equal(first.PropertyType, reflect.ClrType);
        assert.Equal(true, reflect.CanRead);
        assert.Equal(true, reflect.CanWrite);

        var mods = reflect.GetterAccess;
        assert.Equal(true, mods.IsPublic);
        assert.Equal(false, mods.IsPrivate);
        assert.Equal(false, mods.IsInternal);
        assert.Equal(false, mods.IsVirtual);
        assert.Equal(false, mods.IsStatic);
        assert.Equal(true, mods.IsInstance);

        mods = reflect.SetterAccess;
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
        var first = ReflectData
            .GetProperties()
            .FirstOrDefault();
        var reflect = new ReflectionProperty(first);

        var attr = reflect.FindAttribute<DecoratorAttribute>();
        assert.NotNull(attr);
    }

    [UnitTest]
    public void FindAttributes()
    {
        var first = ReflectData
            .GetProperties()
            .FirstOrDefault();
        var reflect = new ReflectionProperty(first);

        var attrs = reflect.FindAttributes<DecoratorAttribute>();
        assert.NotNull(attrs);
        assert.Equal(1, attrs.Count);
    }

    [UnitTest]
    public void GetValue()
    {
        var first = ReflectData
            .GetProperties()
            .FirstOrDefault();
        var reflect = new ReflectionProperty(first);

        var data = new ReflectData() { Field2 = true };
        assert.Ok((bool)reflect.GetValue(data));
    }

    [UnitTest]
    public void GetStaticValue()
    {
        var first = typeof(ReflectData).GetProperty("Field1");
        var reflect = new ReflectionProperty(first);

        try
        {
            ReflectData.Field1 = true;
            assert.Ok((bool)reflect.GetValue(null));
        }
        finally
        {
            ReflectData.Field1 = false;
        }
    }

    [UnitTest]
    public void SetValue()
    {
        var first = ReflectData
            .GetProperties()
            .FirstOrDefault();
        var reflect = new ReflectionProperty(first);

        var data = new ReflectData() { Field2 = true };
        assert.True(data.Field2);

        reflect.SetValue(data, false);
        assert.False(data.Field2);
    }

    [UnitTest(Id = "AB10", Ticket = "https://dev.azure.com/nerdymishka/dotnet/_workitems/edit/10/")]
    public void Private_GetValue()
    {
        var first = typeof(ReflectData).GetProperty(
            "Field4",
            BindingFlags.NonPublic | BindingFlags.Instance);

        assert.NotNull(first);

        var reflect = new ReflectionProperty(first);
        var data = new ReflectData();

        assert.Equal(10, (int)reflect.GetValue(data));
    }

    [AttributeUsage(AttributeTargets.Property)]
    internal class DecoratorAttribute : System.Attribute
    {
    }

    internal class ReflectData
    {
        [Decorator]
        public static bool Field1 { get; set; }

        [Decorator]
        public bool Field2 { get; set; }

        private int Field4 { get; set; } = 10;

        public static PropertyInfo[] GetProperties()
        {
            var t = typeof(ReflectData);
            return t.GetProperties(BindingFlags.Instance | BindingFlags.Public);
        }
    }
}
