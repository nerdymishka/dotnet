using System.Data;

namespace NerdyMishka.Data
{
    public interface IDataCommandFactory
    {
        IDataCommand CreateCommand(CommandBehavior behavior = default);
    }
}