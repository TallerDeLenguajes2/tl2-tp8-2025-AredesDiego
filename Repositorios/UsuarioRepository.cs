using Microsoft.Data.Sqlite;

namespace Mvc.Interfaces;

public class UsuarioRepository : IUserRepository
{
    private readonly string CadenaConexion = "Data Source=DB/Tienda.db";
    public Usuario GetUser(string usuario, string contrasena)
    {
        Usuario user = null;

        const string sql = @"
                    SELECT Id, Nombre, User, Pass, Rol
                    FROM Usuarios
                    WHERE User = @Usuario AND Pass = @Contrasena";
        using var conexion = new SqliteConnection(CadenaConexion);
        conexion.Open();

        using var comando = new SqliteCommand(sql, conexion);
        comando.Parameters.AddWithValue("@Usuario", usuario);
        comando.Parameters.AddWithValue("@Contrasena", contrasena);

        using var reader = comando.ExecuteReader();

        if(reader.Read())
        {
            user = new Usuario
            {
                Id = reader.GetInt32(0),
                Nombre = reader.GetString(1),
                User = reader.GetString(2),
                Pass = reader.GetString(3),
                Rol = reader.GetString(4)
            };
        }
        return user;
    }
}