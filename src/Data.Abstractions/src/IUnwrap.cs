namespace NerdyMishka.Data
{
    /// <summary>
    /// A contract for objects that wrap object.
    /// </summary>
    public interface IUnwrap
    {
        /// <summary>
        /// Unwraps the current object by returning the inner object.
        /// </summary>
        /// <returns>The inner object.</returns>
        object Unwrap();
    }
}