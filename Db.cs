using Npgsql;
using DotNetEnv;

namespace cpwp_pr_1;

public static class Db
{
    private static string _connString = "";

    public static void Init()
    {
        Env.Load();
        var host = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost";
        var port = Environment.GetEnvironmentVariable("DB_PORT") ?? "5432";
        var name = Environment.GetEnvironmentVariable("DB_NAME") ?? "Protucts_db";
        var user = Environment.GetEnvironmentVariable("DB_USER") ?? "postgres";
        var pass = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "123456";

        _connString = $"Host={host};Port={port};Database={name};Username={user};Password={pass}";
    }

    public static NpgsqlConnection GetConnection()
    {
        var conn = new NpgsqlConnection(_connString);
        conn.Open();
        return conn;
    }
}