using System;
using System.Data;
using System.Threading.Tasks;
using Fakes;
using Mettle;
using NerdyMishka.Data;

public class DataConnectionTests
{
    private IAssert assert;

    public DataConnectionTests(IAssert assert)
    {
        this.assert = assert;
    }

    [UnitTest]
    public void Ctor_ThrowsOnNull()
    {
        assert.Throws<System.ArgumentNullException>(() =>
        {
            _ = new DataConnection(null, null);
        });

        assert.Throws<System.ArgumentNullException>(() =>
        {
            _ = new DataConnection(new FakeDbConnection(), null);
        });
    }

    [UnitTest]
    public void Ctor()
    {
        using var fake = new FakeDbConnection();
        using var conn = new DataConnection(fake, new DataSqlDialect());

        assert.Equal(ConnectionState.Closed, conn.State);
        assert.Null(conn.ConnectionString);
        assert.NotNull(conn.SqlDialect);
        assert.Equal("Sqlite", conn.Provider);
    }

    [UnitTest]
    public void Set_ConnectionString()
    {
        using var fake = new FakeDbConnection();
        using var conn = new DataConnection(fake, new DataSqlDialect())
        {
            ConnectionString = "Data Source=localhost;Initial Catalog=Test_DB",
        };

        assert.NotNull(fake.ConnectionString);
        assert.Equal(fake.DataSource, "localhost");
        assert.Equal(fake.ConnectionString, conn.ConnectionString);

        fake.ConnectionString = "Data Source=newHost;Initial Catalog=Test2_DB";
        assert.NotNull(fake.ConnectionString);
        assert.NotNull(conn.ConnectionString);
        assert.Equal(fake.ConnectionString, conn.ConnectionString);
        assert.Equal(fake.DataSource, "newHost");
    }

    [UnitTest]
    public void Open()
    {
        using var fake = new FakeDbConnection();
        using var conn = new DataConnection(fake, new DataSqlDialect());

        conn.Open();
        assert.Equal(ConnectionState.Open, fake.State);
        assert.Equal(ConnectionState.Open, conn.State);
    }

    [UnitTest]
    public async Task OpenAsync()
    {
        using var fake = new FakeDbConnection();
        using var conn = new DataConnection(fake, new DataSqlDialect());

        await conn.OpenAsync().ConfigureAwait(false);
        assert.Equal(ConnectionState.Open, fake.State);
        assert.Equal(ConnectionState.Open, conn.State);
    }

    [UnitTest]
    public void Close()
    {
        using var fake = new FakeDbConnection();
        using var conn = new DataConnection(fake, new DataSqlDialect());

        fake.SetState(ConnectionState.Open);
        assert.Equal(ConnectionState.Open, fake.State);
        assert.Equal(ConnectionState.Open, conn.State);

        conn.Close();
        assert.Equal(ConnectionState.Closed, fake.State);
        assert.Equal(ConnectionState.Closed, conn.State);
    }

    [UnitTest]
    public void Dispose()
    {
        using var fake = new FakeDbConnection();
        var conn = new DataConnection(fake, new DataSqlDialect(), true);

        fake.SetState(ConnectionState.Open);
        assert.Equal(ConnectionState.Open, fake.State);
        assert.Equal(ConnectionState.Open, conn.State);

        conn.Dispose();
        assert.Equal(ConnectionState.Closed, conn.State);
        assert.Equal(ConnectionState.Closed, fake.State);
    }

    [UnitTest]
    public void CreateCommand()
    {
        using var fake = new FakeDbConnection();
        using var conn = new DataConnection(fake, new DataSqlDialect());

        var cmd = conn.CreateCommand();
        assert.NotNull(cmd);
        assert.IsType(typeof(DataCommand), cmd);
    }

    [UnitTest]
    public void BeginTransaction()
    {
        using var fake = new FakeDbConnection();
        using var conn = new DataConnection(fake, new DataSqlDialect());

        var tx = conn.BeginTransaction();
        assert.NotNull(tx);
        assert.IsType(typeof(DataTransaction), tx);
        assert.IsType(typeof(FakeDbTransaction), ((IUnwrap)tx).Unwrap());
    }

    [UnitTest]
    public void OnCompleted()
    {
        using var fake = new FakeDbConnection();
        using var conn = new DataConnection(fake, new DataSqlDialect(), true);

        fake.SetState(ConnectionState.Open);
        assert.Equal(ConnectionState.Open, conn.State);

        conn.OnCompleted();
        assert.Equal(ConnectionState.Closed, conn.State);
        assert.Equal(ConnectionState.Closed, fake.State);
    }

    [UnitTest]
    public void OnError()
    {
        using var fake = new FakeDbConnection();
        using var conn = new DataConnection(fake, new DataSqlDialect());

        fake.SetState(ConnectionState.Open);
        assert.Equal(ConnectionState.Open, conn.State);

        conn.OnError(null);
        assert.Equal(ConnectionState.Closed, conn.State);
        assert.Equal(ConnectionState.Closed, fake.State);
    }

    [UnitTest]
    public void OnNext_ThrowException()
    {
        using var fake = new FakeDbConnection();
        using var conn = new DataConnection(fake, new DataSqlDialect());

        assert.Throws<ArgumentNullException>(() =>
        {
            conn.OnNext(null);
        });
    }
}