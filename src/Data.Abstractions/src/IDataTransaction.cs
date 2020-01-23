namespace NerdyMishka.Data
{
    public interface IDataTransaction :
        IDataTransactionActions,
        IDataCommandScope
    {
        IDataConnection Connection { get; }
    }
}