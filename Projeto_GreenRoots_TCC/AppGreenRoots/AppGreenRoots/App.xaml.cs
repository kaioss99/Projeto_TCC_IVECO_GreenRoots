using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;
using Microsoft.Data.Sqlite;

namespace AppGreenRoots;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Passaporte_Digital.db");
        using var conn = new SqliteConnection($"Data Source={dbPath}");
        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            CREATE TABLE IF NOT EXISTS Usuario (
                id_usuario INTEGER PRIMARY KEY AUTOINCREMENT,
                nome       TEXT NOT NULL,
                email      TEXT NOT NULL UNIQUE,
                senha      TEXT NOT NULL
            );";
        cmd.ExecuteNonQuery();
    }
}