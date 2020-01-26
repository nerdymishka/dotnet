using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using Mettle;
using Microsoft.Data.Sqlite;
using NerdyMishka.Data;
using NerdyMishka.Data.Extensions;

public class SimpleIntegrationTests
{
    private IAssert assert;

    public SimpleIntegrationTests(IAssert assert)
    {
        this.assert = assert;
    }

    [IntegrationTest]
    public void SimpleReader_ListParameters()
    {
        using var sqlite = new Microsoft.Data.Sqlite.SqliteConnection("Data Source=:memory:");
        using var conn = new DataConnection(sqlite, new DataSqlDialect());

        conn.Open();
        assert.Equal(ConnectionState.Open, conn.State);

        var query = conn.SqlDialect.CreateBuilder();
        var reader = conn.ExecuteReader(
            query.Append("SELECT ? AS first"), new List<object>() { "one" });

        bool read = false;
        while (reader.Read())
        {
            var v = reader.GetString("first");
            assert.Equal("one", v);

            read = true;
        }

        assert.Ok(read);
    }

    [IntegrationTest]
    public void SimpleReader_Dictionary()
    {
        using var sqlite = new Microsoft.Data.Sqlite.SqliteConnection("Data Source=:memory:");
        using var conn = new DataConnection(sqlite, new DataSqlDialect());

        conn.Open();
        assert.Equal(ConnectionState.Open, conn.State);

        var parameters = new Hashtable()
        {
            { "first", "one" },
        };

        var query = conn.SqlDialect.CreateBuilder();
        var reader = conn.ExecuteReader(
            query.Append("SELECT @first AS first"), parameters);

        bool read = false;
        while (reader.Read())
        {
            var v = reader.GetString("first");

            assert.Equal("one", v);

            read = true;
        }

        assert.Ok(read);
    }

    [IntegrationTest]
    public void SimpleReader_TypedDictionary()
    {
        using var sqlite = new Microsoft.Data.Sqlite.SqliteConnection("Data Source=:memory:");
        using var conn = new DataConnection(sqlite, new DataSqlDialect());

        conn.Open();
        assert.Equal(ConnectionState.Open, conn.State);

        var parameters = new Dictionary<string, object>()
            {
                { "first", "one" },
            };

        var query = conn.SqlDialect.CreateBuilder();
        var reader = conn.ExecuteReader(
            query.Append("SELECT @first AS first"), parameters);

        bool read = false;
        while (reader.Read())
        {
            var v = reader.GetString("first");
            assert.Equal("one", v);

            read = true;
        }

        assert.Ok(read);
    }

    [IntegrationTest]
    public void SimpleReader_TypedList()
    {
        using var sqlite = new Microsoft.Data.Sqlite.SqliteConnection("Data Source=:memory:");
        using var conn = new DataConnection(sqlite, new DataSqlDialect());

        conn.Open();
        assert.Equal(ConnectionState.Open, conn.State);

        var parameters = new Collection<IDbDataParameter>()
        {
            new SqliteParameter("@first", "one"),
        };

        var query = conn.SqlDialect.CreateBuilder();
        var reader = conn.ExecuteReader(
            query.Append("SELECT @first AS first"), parameters);

        bool read = false;
        while (reader.Read())
        {
            var v = reader.GetString("first");
            assert.Equal("one", v);

            read = true;
        }

        assert.Ok(read);
    }
}