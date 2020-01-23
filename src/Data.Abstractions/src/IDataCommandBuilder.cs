
namespace NerdyMishka.Data
{
    public interface IDataCommandBuilder
    {
        IDataCommandConfiguration Configuration { get; }

        IDataCommand Command { get; set; }
    }
}