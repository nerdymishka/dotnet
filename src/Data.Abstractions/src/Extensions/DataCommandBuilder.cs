namespace NerdyMishka.Data.Extensions
{
    public class DataCommandBuilder : IDataCommandBuilder
    {
        public DataCommandBuilder()
        {
            this.Configuration = new DataCommandConfiguration();
        }

        public IDataCommand Command { get; set; }

        public IDataCommandConfiguration Configuration { get; set; }
    }
}