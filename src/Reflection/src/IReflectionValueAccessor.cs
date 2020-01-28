namespace NerdyMishka.Reflection
{
    public interface IReflectionValueAccessor
    {
        object GetValue(object instance);

        void SetValue(object instance, object value);
    }
}