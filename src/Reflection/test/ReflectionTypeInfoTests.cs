using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Mettle;
using NerdyMishka.Reflection;
#pragma warning disable CS0649
#pragma warning disable CS0414
#pragma warning disable CA1823

public class ReflectionTypeInfoTests
{
    private IAssert assert;

    public ReflectionTypeInfoTests(IAssert assert)
    {
        this.assert = assert;
    }

    [UnitTest]
    public void Ctor_ThrowsArgumentNull()
    {
        assert.Throws<ArgumentNullException>(() =>
        {
            _ = new ReflectionTypeInfo(null, null);
        });
    }

    [UnitTest]
    public void Ctor()
    {
        var cache = new ReflectionCache();
        var reflect = new ReflectionTypeInfo(ReflectData.GetString(), cache);

        assert.NotNull(reflect);
        assert.Equal(ReflectData.GetString(), reflect.ClrType);
        assert.True(reflect.IsClass);
        assert.False(reflect.IsEnum);
        assert.False(reflect.IsNotPublic);
        assert.True(reflect.IsPublic);
        assert.False(reflect.IsPrimitive);
        assert.False(reflect.IsPointer);
        assert.False(reflect.IsValueType);
        assert.False(reflect.IsAbstract);
        assert.False(reflect.IsArray);
        assert.False(reflect.IsGenericType);
        assert.False(reflect.IsGenericTypeDefinition);
        assert.False(reflect.IsInternal);
        assert.False(reflect.IsNullableOfT);
        assert.Null(reflect.UnderlyingType);
        assert.True(reflect.IsDataType);
        assert.False(reflect.IsDictionaryLike);
        assert.False(reflect.IsListLike);
    }

    [UnitTest]
    public void GetElementType()
    {
        var cache = new ReflectionCache();
        var reflect = new ReflectionTypeInfo(ReflectData.GetList(), cache);

        var type = reflect.GetElementType();
        assert.NotNull(type);
        assert.Equal(typeof(string), type.ClrType);
        assert.True(reflect.IsListLike);
    }

    [UnitTest]
    public void GetKeyType()
    {
        var cache = new ReflectionCache();
        var reflect = new ReflectionTypeInfo(ReflectData.GetDictionary(), cache);

        var type = reflect.GetKeyType();
        assert.NotNull(type);
        assert.Equal(typeof(string), type.ClrType);
        var oType = reflect.GetElementType();
        assert.NotNull(oType);
        assert.Equal(typeof(object), oType.ClrType);
        assert.True(reflect.IsDictionaryLike);
    }

    [UnitTest]
    public void LoadMethods()
    {
        var cache = new ReflectionCache();
        var reflect = new ReflectionTypeInfo(ReflectData.GetString(), cache);

        var methods = reflect.Methods;
        assert.NotNull(methods);
        assert.True(methods.Any());

        foreach (var m in methods)
        {
            assert.False(m.ModifierAccess.IsStatic);
        }
    }


    [UnitTest]
    public void LoadProperties()
    {
        var cache = new ReflectionCache();
        var reflect = new ReflectionTypeInfo(ReflectData.GetList(), cache);

        var properties = reflect.Properties;
        assert.NotNull(properties);
        assert.True(properties.Any());

        foreach (var p in properties)
        {
            assert.False(p.GetterAccess.IsStatic);
        }
    }

    [UnitTest]
    public void LoadFields()
    {
        var cache = new ReflectionCache();
        var reflect = new ReflectionTypeInfo(ReflectData.GetSelf(), cache);

        var fields = reflect.Fields;
        assert.NotNull(fields);
        assert.True(fields.Any());

        foreach (var f in fields)
        {
            assert.False(f.ModifierAccess.IsStatic);
        }
    }

    [UnitTest]
    public void LoadConstructors()
    {
        var cache = new ReflectionCache();
        var reflect = new ReflectionTypeInfo(ReflectData.GetSelf(), cache);

        var constructors = reflect.Constructors;
        assert.NotNull(constructors);
        assert.True(constructors.Any());
    }

    [AttributeUsage(AttributeTargets.Property)]
    internal class DecoratorAttribute : System.Attribute
    {
    }

    internal class ReflectData
    {
        private string Field1;

        public static Type GetString() => typeof(string);

        public static Type GetList() => typeof(List<string>);

        public static Type GetDictionary() => typeof(Dictionary<string, object>);

        public static Type GetIList() => typeof(IList<string>);

        public static Type GetNullableOfT() => typeof(bool?);

        public static Type GetArray() => typeof(string[]);

        public static Type GetSelf() => typeof(ReflectData);
    }
}
