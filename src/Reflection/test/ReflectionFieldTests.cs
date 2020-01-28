using System;
using System.Linq;
using System.Reflection;
using Mettle;
using NerdyMishka.Reflection;
#pragma warning disable CS0649
#pragma warning disable CS0414
#pragma warning disable CA1823

public class ReflectionFieldTests
{
    private IAssert assert;

    public ReflectionFieldTests(IAssert assert)
    {
        this.assert = assert;
    }

    [UnitTest]
    public void Ctor_ThrowsArgumentNull()
    {
        assert.Throws<ArgumentNullException>(() =>
        {
            _ = new ReflectionField(null);
        });
    }

    [UnitTest]
    public void Ctor()
    {
        var first = ReflectData
            .GetFields()
            .FirstOrDefault();
        var reflectionField = new ReflectionField(first);

        assert.Equal(first.Name, reflectionField.Name);
        assert.Equal(first.FieldType, reflectionField.ClrType);
        assert.Equal(true, reflectionField.CanWrite);
        assert.Equal(true, reflectionField.CanRead);

        var mods = reflectionField.ModifierAccess;
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
            .GetFields()
            .FirstOrDefault();
        var reflect = new ReflectionField(first);

        var attr = reflect.FindAttribute<DecoratorAttribute>();
        assert.NotNull(attr);
    }

    [UnitTest]
    public void FindAttributes()
    {
        var first = ReflectData
            .GetFields()
            .FirstOrDefault();
        var reflect = new ReflectionField(first);

        var attrs = reflect.FindAttributes<DecoratorAttribute>();
        assert.NotNull(attrs);
        assert.Equal(1, attrs.Count);
    }

    [UnitTest]
    public void GetValue()
    {
        var first = ReflectData
            .GetFields()
            .FirstOrDefault();
        var reflect = new ReflectionField(first);

        var data = new ReflectData() { Field2 = true };
        assert.Ok((bool)reflect.GetValue(data));
    }

    [UnitTest]
    public void SetValue()
    {
        var first = ReflectData
            .GetFields()
            .FirstOrDefault();
        var reflect = new ReflectionField(first);

        var data = new ReflectData() { Field2 = true };
        assert.True(data.Field2);

        reflect.SetValue(data, false);
        assert.False(data.Field2);
    }

    [UnitTest(Id = "AB10", Ticket = "https://dev.azure.com/nerdymishka/dotnet/_workitems/edit/10/")]
    public void Private_GetValue()
    {
        var first = typeof(ReflectData).GetField(
            "Field4",
            BindingFlags.NonPublic | BindingFlags.Instance);

        assert.NotNull(first);

        var reflect = new ReflectionField(first);
        var data = new ReflectData();

        assert.Equal(10, (int)reflect.GetValue(data));
    }

    [AttributeUsage(AttributeTargets.Field)]
    internal class DecoratorAttribute : System.Attribute
    {
    }

    internal class ReflectData
    {
        [Decorator]
        public static bool Field1;

        [Decorator]
        public bool Field2;

        private int Field4 = 10;

        public static FieldInfo[] GetFields()
        {
            var t = typeof(ReflectData);
            return t.GetFields();
        }
    }
}
