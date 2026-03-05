using System.IO;
using System.Windows;
using AppGreenRoots.Models;
using Microsoft.Data.Sqlite;

namespace AppGreenRoots.Data;

public static class Database
{
    private static readonly string DbPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
        "AppGreenRoots_Banco"
    );

    private static readonly string CaminhoArquivoBanco = Path.Combine(DbPath, "Passaporte_Digital.db");
    
    // Unificando a Connection String para evitar confusão
    private static readonly string ConnectionString = $"Data Source={CaminhoArquivoBanco}";

    static Database()
    {
        if (!Directory.Exists(DbPath))
            Directory.CreateDirectory(DbPath);

        // Corrigido: Verifica o caminho do arquivo, não a string de conexão completa
        if (!File.Exists(CaminhoArquivoBanco))
        {
            // Opcional: Criar o banco e as tabelas automaticamente aqui
            Console.WriteLine("Banco de dados não encontrado!!");
        }
    }

    public static SqliteConnection GetConnection() => new SqliteConnection(ConnectionString);

    public static Usuario? AutenticarUsuario(string email, string senha)
    {
        // Nota: Em produção, compare o HASH da senha, não o texto puro
        using var conn = GetConnection();
        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT id_usuario, nome, email, senha FROM Usuario WHERE email = @email AND senha = @senha LIMIT 1";
        cmd.Parameters.AddWithValue("@email", email);
        cmd.Parameters.AddWithValue("@senha", senha);

        using var reader = cmd.ExecuteReader();
        if (reader.Read())
            return new Usuario {
                Id_Usuario = reader.GetInt32(0),
                Nome = reader.GetString(1),
                Email = reader.GetString(2),
                Senha = reader.GetString(3)
            };
        
        return null;
    }

    public static bool CadastrarUsuario(string nome, string email, string senha)
    {
        if (senha.Length > 8)
            throw new ArgumentException("A senha deve ter no máximo 8 caracteres.");

        using var conn = GetConnection();
        conn.Open();

        // Verificar se usuário já existe
        var checkCmd = conn.CreateCommand();
        checkCmd.CommandText = "SELECT EXISTS(SELECT 1 FROM Usuario WHERE email = @email)";
        checkCmd.Parameters.AddWithValue("@email", email);
        
        if ((long)checkCmd.ExecuteScalar()! == 1) return false;

        // Inserir novo usuário
        var cmd = conn.CreateCommand();
        cmd.CommandText = "INSERT INTO Usuario (nome, email, senha) VALUES (@nome, @email, @senha)";
        cmd.Parameters.AddWithValue("@nome", nome);
        cmd.Parameters.AddWithValue("@email", email);
        cmd.Parameters.AddWithValue("@senha", senha);
        
        return cmd.ExecuteNonQuery() > 0;
    }
}