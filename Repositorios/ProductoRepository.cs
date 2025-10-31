using System.Reflection.Metadata.Ecma335;
using Microsoft.Data.Sqlite;
using Microsoft.VisualBasic;

interface IProductoRepository
{
    void CrearProducto(Productos producto);
    bool ModificarProducto(int id, Productos productos);
    List<Productos> ListarProductos();
    Productos ObtenerDetalles(int id);
    bool EliminarProducto(int id);
}
public class ProductoRepository : IProductoRepository
{
    private string conection_string = "Data Source=Tienda.db";

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
    public bool ModificarProducto(int id, Productos productos)
    {
        using var conexion = new SqliteConnection(conection_string);
        conexion.Open();

        string sql = "UPDATE Productos SET Descripcion = @Descripcion, Precio = @Precio WHERE idProducto = @id";
        
        using var comando = new SqliteCommand(sql, conexion);
        comando.Parameters.Add(new SqliteParameter("@id", id));
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
        using var conexion = new SqliteConnection(conection_string);
        conexion.Open();

        string sql = "DELETE FROM Productos WHERE idProducto = @id";
        using var comando = new SqliteCommand(sql, conexion);

        comando.Parameters.Add(new SqliteParameter("@id", id));
        int filasAfectadas = comando.ExecuteNonQuery(); 

        return filasAfectadas > 0;
    }
}