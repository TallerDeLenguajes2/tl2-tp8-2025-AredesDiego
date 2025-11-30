using System.Reflection.Metadata.Ecma335;
using Microsoft.Data.Sqlite;
using Microsoft.VisualBasic;

public class ProductoRepository : IProductoRepository
{
    private string conection_string = "Data Source=DB/Tienda.db";

    public void CrearProducto(Productos producto)
    {
        using var conexion = new SqliteConnection(conection_string);
        conexion.Open();

        string sql = "INSERT INTO Productos (Descripcion, Precio) VALUES (@Descripcion, @Precio)";

        using var comando = new SqliteCommand(sql, conexion);

        comando.Parameters.Add(new SqliteParameter("@Descripcion", producto.Descripcion));
        comando.Parameters.Add(new SqliteParameter("@Precio", producto.Precio));

        comando.ExecuteNonQuery();
    }
    public bool ModificarProducto(Productos productos)
    {
        using var conexion = new SqliteConnection(conection_string);
        conexion.Open();

        string sql = "UPDATE Productos SET Descripcion = @Descripcion, Precio = @Precio WHERE idProducto = @id";
        
        using var comando = new SqliteCommand(sql, conexion);
        comando.Parameters.Add(new SqliteParameter("@id", productos.idProducto));
        comando.Parameters.Add(new SqliteParameter("@Descripcion", productos.Descripcion));
        comando.Parameters.Add(new SqliteParameter("@Precio", productos.Precio));

        int filasAfectadas = comando.ExecuteNonQuery();

        return filasAfectadas > 0;
    }
    public List<Productos> ListarProductos()
    {
        string sql = "SELECT * FROM Productos;";

        List<Productos> listaProductos = [];

        using var connection = new SqliteConnection(conection_string);
        connection.Open();

        using var comando = new SqliteCommand(sql, connection);

        using (SqliteDataReader reader = comando.ExecuteReader())
        {
            while (reader.Read())
            {
                var producto = new Productos()
                {
                    idProducto = Convert.ToInt32(reader["idProducto"]),
                    Descripcion = reader["Descripcion"].ToString(),
                    Precio = Convert.ToInt32(reader["Precio"])
                };
                listaProductos.Add(producto);
            }
        }

        connection.Close();

        return listaProductos;
    }
    public Productos ObtenerDetalles(int id)
    {
        using var conexion = new SqliteConnection(conection_string);
        conexion.Open();

        string sql = "SELECT idProducto, Descripcion, Precio FROM Productos WHERE idProducto = @id";

        using var comando  = new SqliteCommand(sql, conexion);
        comando .Parameters.Add(new SqliteParameter("@id", id));

        using var lector = comando .ExecuteReader();

        if (lector.Read()) //Si encontró un registro
        {
            var producto = new Productos()
            {
                idProducto = Convert.ToInt32(lector["idProducto"]),
                Descripcion = lector["Descripcion"].ToString(),
                Precio = Convert.ToInt32(lector["Precio"])
            };

            return producto;
        }

        return null;
    }
    public bool EliminarProducto(int id)
    {
       /*  using var conexion = new SqliteConnection(conection_string);
        conexion.Open();

        string sql = "DELETE FROM Productos WHERE idProducto = @id";
        using var comando = new SqliteCommand(sql, conexion);

        comando.Parameters.Add(new SqliteParameter("@id", id));
        int filasAfectadas = comando.ExecuteNonQuery(); 

        return filasAfectadas > 0; */
        using var conexion = new SqliteConnection(conection_string);
        conexion.Open();

        // Primero borro las referencias en PresupuestosDetalle
        string sql1 = "DELETE FROM PresupuestosDetalle WHERE idProducto = @id";
        using (var cmd1 = new SqliteCommand(sql1, conexion))
        {
            cmd1.Parameters.Add(new SqliteParameter("@id", id));
            cmd1.ExecuteNonQuery();
        }

        // Luego borro el producto
        string sql2 = "DELETE FROM Productos WHERE idProducto = @id";
        using (var cmd2 = new SqliteCommand(sql2, conexion))
        {
            cmd2.Parameters.Add(new SqliteParameter("@id", id));
            int filas = cmd2.ExecuteNonQuery();
            return filas > 0;
        }
    }
}
