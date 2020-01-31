using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace NerdyMishka.Reflection
{
    public class ReflectionTypeInfo : ReflectionMember,
        IReflectionTypeInfo,
        IReflectionFactorySource,
        IReflectionEnum
    {
        private System.Type type;

        private IReflectionTypeInfo baseType;

        private IReflectionCache cache;

        private List<IReflectionConstructor> constructors;

        private List<IReflectionField> fields;

        private List<IReflectionProperty> properties;

        private List<IReflectionMethod> methods;

        private List<IReflectionTypeInfo> interfaces;

        private bool? isListLike;

        private bool? isDictionaryLike;
        private IReflectionFactory factory = null;

        private IReflectionTypeInfo elementType;

        private IReflectionTypeInfo keyType;

        private IReadOnlyCollection<string> enumNames;

        private IReadOnlyCollection<object> enumValues;

        private IReflectionTypeInfo underlyingType;

        private bool underlyingTypeSet = false;

        private bool? isDataType;

        public ReflectionTypeInfo(Type type, IReflectionCache cache = null)
        {
            this.type = type;
            this.cache = cache;
            this.factory = cache?.ReflectionFactory ??
                NerdyMishka.Reflection.ReflectionFactory.Default;
        }

        public override string Name => this.type.Name;

        public override Type ClrType => this.type;

        public virtual IReflectionTypeInfo UnderlyingType
        {
            get
            {
                if (this.underlyingTypeSet)
                    return this.underlyingType;

                var temp = Nullable.GetUnderlyingType(this.type);
                if (temp != null)
                    this.underlyingType = this.GetType(temp);

                this.underlyingTypeSet = true;
                return this.underlyingType;
            }
        }

        public bool HasElementType => this.GetElementType() != null;

        public bool IsInterface => this.type.IsInterface;

        public bool IsEnum => this.type.IsEnum;

        public bool IsAbstract => this.type.IsAbstract;

        public bool IsClass => this.type.IsClass;

        public bool IsArray => this.type.IsArray;

        public bool IsValueType => this.type.IsValueType;

        public bool IsPrimitive => this.type.IsPrimitive;

        public bool IsPublic => this.type.IsPublic;

        public bool IsNotPublic => this.type.IsNotPublic;

        public bool IsPointer => this.type.IsPointer;

        public bool IsSealed => this.type.IsSealed;

        public bool IsGenericType => this.type.IsGenericType;

        public bool IsGenericTypeDefinition => this.type.IsGenericTypeDefinition;

        public bool IsByRef => this.type.IsByRef;

        public bool IsInternal => !this.type.IsVisible;

        public virtual bool IsNullableOfT
        {
            get
            {
                return this.UnderlyingType != null;
            }
        }

        public virtual bool IsDataType
        {
            get
            {
                if (this.isDataType.HasValue)
                    return this.isDataType.Value;

                var types = new List<Type>()
                {
                    typeof(string),
                    typeof(DateTime),
                    typeof(TimeSpan),
                    typeof(DateTimeOffset),
                    typeof(Guid),
                };

                this.isDataType = this.IsPrimitive ||
                    types.Contains(this.type);

                return this.isDataType.Value;
            }
        }

        public virtual bool IsListLike
        {
            get
            {
                if (!this.isListLike.HasValue)
                    this.Inspect();

                return this.isListLike.Value;
            }
        }

        public bool IsDictionaryLike
        {
            get
            {
                if (!this.isDictionaryLike.HasValue)
                    this.Inspect();

                return this.isDictionaryLike.Value;
            }
        }

        public virtual IReadOnlyCollection<string> EnumNames
        {
            get
            {
                if (!this.IsEnum)
                    return Array.Empty<string>();

                this.enumNames = this.enumNames ?? (IReadOnlyCollection<string>)this.type.GetEnumNames();
                return this.enumNames;
            }
        }

        public virtual IReadOnlyCollection<object> EnumValues
        {
            get
            {
                if (!this.IsEnum)
                    return Array.Empty<object>();

                this.enumValues = this.enumValues ?? (IReadOnlyCollection<object>)this.type.GetEnumValues();
                return this.enumValues;
            }
        }

        public IReflectionFactory ReflectionFactory => this.factory;

        public IReflectionTypeInfo BaseType
        {
            get
            {
                if (this.baseType != null)
                    return this.baseType;

                if (this.cache != null)
                {
                    this.baseType = this.cache.GetOrAdd(this.type);
                    return this.baseType;
                }

                this.baseType = new ReflectionTypeInfo(this.type.BaseType, this.cache);
                return this.baseType;
            }
        }

        public string FullName => this.type.FullName;

        public virtual IReadOnlyCollection<IReflectionConstructor> Constructors
        {
            get
            {
                if (this.constructors != null)
                    return this.constructors;

                this.constructors = new List<IReflectionConstructor>();
                foreach (var ctor in this.type.GetConstructors())
                {
                    this.constructors.Add(
                        this.ReflectionFactory.CreateConstructor(ctor, null, this));
                }

                return this.constructors;
            }
        }

        public virtual IReadOnlyCollection<IReflectionTypeInfo> Interfaces
        {
            get
            {
                if (this.interfaces != null)
                    return this.interfaces;

                this.LoadInterfaces();
                return this.interfaces;
            }
        }

        public virtual IReadOnlyCollection<IReflectionField> Fields
        {
            get
            {
                if (this.fields != null)
                    return this.fields;

                this.LoadFields();
                return this.fields;
            }
        }

        public virtual IReadOnlyCollection<IReflectionProperty> Properties
        {
            get
            {
                if (this.properties != null)
                    return this.properties;

                this.LoadProperties();
                return this.properties;
            }
        }

        public virtual IReadOnlyCollection<IReflectionMethod> Methods
        {
            get
            {
                if (this.methods != null)
                    return this.methods;

                this.LoadMethods();
                return this.methods;
            }
        }

        public IReflectionTypeInfo GetElementType()
        {
            if (this.elementType != null)
                return this.elementType;

            if (this.IsArray)
            {
                this.elementType = this.ReflectionFactory.CreateType(this.type.GetElementType());
                return this.elementType;
            }

            if (!this.isDictionaryLike.HasValue)
            {
                this.Inspect();
            }

            return this.elementType;
        }

        public string GetEnumName(object value)
        {
            if (!this.IsEnum)
                return null;

            return this.type.GetEnumName(value);
        }

        public IReflectionTypeInfo GetKeyType()
        {
            if (this.keyType != null)
                return this.keyType;

            if (!this.isDictionaryLike.HasValue)
            {
                this.Inspect();
            }

            return this.keyType;
        }

        public IReflectionTypeInfo LoadInterfaces()
        {
            this.interfaces = new List<IReflectionTypeInfo>();
            foreach (var contract in this.type.GetInterfaces())
            {
                this.interfaces.Add(new ReflectionTypeInfo(contract, this.cache));
            }

            return this;
        }

        public IReflectionTypeInfo LoadFields(
            bool includeStatic = false,
            bool includeInherit = false)
        {
            var builder = new ReflectionBindingBuilder();
            builder.AddPublic().AddNonPublic().AddInstance();

            if (includeStatic)
                builder.AddStatic();

            if (includeInherit)
                builder.AddInherit();

            var flags = builder.ToFlags();

            return this.LoadFields(flags);
        }

        public IReflectionTypeInfo LoadFields(BindingFlags flags)
        {
            this.fields = new List<IReflectionField>();
            foreach (var fieldInfo in this.type.GetFields(flags))
            {
                this.fields.Add(new ReflectionField(fieldInfo, this));
            }

            return this;
        }

        public IReflectionTypeInfo LoadProperties(
            bool includeStatic = false,
            bool includeInherit = false)
        {
            var builder = new ReflectionBindingBuilder();
            builder.AddPublic().AddNonPublic().AddInstance();

            if (includeStatic)
                builder.AddStatic();

            if (includeInherit)
                builder.AddInherit();

            var flags = builder.ToFlags();

            return this.LoadProperties(flags);
        }

        public IReflectionTypeInfo LoadProperties(BindingFlags flags)
        {
            this.properties = new List<IReflectionProperty>();
            foreach (var propertyInfo in this.type.GetProperties(flags))
            {
                this.properties.Add(new ReflectionProperty(propertyInfo, this));
            }

            return this;
        }

        public virtual IReflectionTypeInfo LoadMethods(
            bool includeStatic = false,
            bool includeInherit = false)
        {
            var builder = new ReflectionBindingBuilder();
            builder.AddPublic().AddNonPublic().AddInstance();

            if (includeStatic)
                builder.AddStatic();

            if (includeInherit)
                builder.AddInherit();

            var flags = builder.ToFlags();

            return this.LoadProperties(flags);
        }

        public IReflectionTypeInfo LoadMethods(BindingFlags flags)
        {
            this.methods = new List<IReflectionMethod>();
            foreach (var method in this.type.GetMethods(flags))
            {
                this.methods.Add(new ReflectionMethod(
                    method,
                    this.factory,
                    null,
                    this));
            }

            return this;
        }

        protected void Inspect()
        {
            if (this.IsValueType || this.IsEnum || this.IsAbstract || this.IsPointer || this.IsNullableOfT
                || this.IsDataType)
                return;

            if (this.IsInterface)
            {
                if (this.type.FullName.StartsWith("System.Collections.Generic.IDictionary",
                    false,
                    CultureInfo.InvariantCulture))
                {
                    var args = this.ClrType.GetGenericArguments();
                    this.keyType = this.GetType(args[0]);
                    this.elementType = this.GetType(args[1]);
                    this.isDictionaryLike = true;
                    this.isListLike = true;
                    return;
                }

                if (this.type.FullName.StartsWith("System.Collections.IDictionary",
                    false,
                    CultureInfo.InvariantCulture))
                {
                    var objectType = this.GetType(typeof(object));
                    this.keyType = objectType;
                    this.elementType = objectType;
                    this.isDictionaryLike = true;
                    this.isListLike = true;
                    return;
                }

                if (this.type.FullName.StartsWith("System.Collections.Generic.IList",
                    false,
                    CultureInfo.InvariantCulture))
                {
                    var type = this.type.GetGenericArguments()[0];
                    this.elementType = this.GetType(type);
                    this.isListLike = true;
                    return;
                }

                if (this.type.FullName.StartsWith("System.Collections.Generic.IReadOnlyCollection",
                    false,
                    CultureInfo.InvariantCulture))
                {
                    var type = this.type.GetGenericArguments()[0];
                    this.elementType = this.GetType(type);
                    this.isListLike = true;
                    return;
                }

                if (this.type.FullName.StartsWith("System.Collections.Generic.ICollection",
                    false,
                    CultureInfo.InvariantCulture))
                {
                    var type = this.type.GetGenericArguments()[0];
                    this.elementType = this.GetType(type);
                    this.isListLike = true;
                    return;
                }

                if (this.type.FullName.StartsWith("System.Collections.IList",
                    false,
                    CultureInfo.InvariantCulture))
                {
                    this.elementType = this.GetType(typeof(object));
                    this.isListLike = true;
                    return;
                }
            }

            foreach (var c in this.Interfaces)
            {
                if (c.FullName.StartsWith("System.Collections.Generic.IDictionary",
                    false,
                    CultureInfo.InvariantCulture))
                {
                    var args = c.ClrType.GetGenericArguments();
                    this.keyType = this.GetType(args[0]);
                    this.elementType = this.GetType(args[1]);
                    this.isDictionaryLike = true;
                    this.isListLike = true;
                    return;
                }

                if (c.FullName.StartsWith("System.Collections.IDictionary",
                    false,
                    CultureInfo.InvariantCulture))
                {
                    var objectType = this.GetType(typeof(object));
                    this.keyType = objectType;
                    this.elementType = objectType;
                    this.isDictionaryLike = true;
                    this.isListLike = true;
                    return;
                }
            }

            foreach (var c in this.Interfaces)
            {
                if (c.FullName.StartsWith("System.Collections.Generic.IList",
                    false,
                    CultureInfo.InvariantCulture))
                {
                    var type = c.ClrType.GetGenericArguments()[0];
                    this.elementType = this.GetType(type);
                    this.isListLike = true;
                    return;
                }

                if (c.FullName.StartsWith("System.Collections.Generic.IReadOnlyCollection",
                    false,
                    CultureInfo.InvariantCulture))
                {
                    var type = c.ClrType.GetGenericArguments()[0];
                    this.elementType = this.GetType(type);
                    this.isListLike = true;
                    return;
                }

                if (c.FullName.StartsWith("System.Collections.Generic.ICollection",
                    false,
                    CultureInfo.InvariantCulture))
                {
                    var type = c.ClrType.GetGenericArguments()[0];
                    this.elementType = this.GetType(type);
                    this.isListLike = true;
                    return;
                }

                if (c.FullName.StartsWith("System.Collections.IList",
                    false,
                    CultureInfo.InvariantCulture))
                {
                    this.elementType = this.GetType(typeof(object));
                    this.isListLike = true;
                    return;
                }
            }
        }

        private IReflectionTypeInfo GetType(Type type)
        {
            var typeInfo = this.cache?.GetOrAdd(type);
            if (typeInfo != null)
                return typeInfo;

            return this.ReflectionFactory.CreateType(type);
        }
    }
}