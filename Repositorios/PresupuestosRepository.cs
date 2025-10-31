using Microsoft.Data.Sqlite;

interface IPresupuestosRepository
{
	public void CrearPresupuesto(Presupuestos presupuestos);
	List<Presupuestos> ListarPresupuestos();
	Presupuestos ObtenerDetalles(int id);
	void AgregarProductoAPresupuesto(int idPresupuesto, int idProducto, int cantidad);
	public bool EliminarPresupuesto(int id); 
}
public class PresupuestosRepository : IPresupuestosRepository
{
	private string conection_string = "Data Source=Tienda.db";
	public void CrearPresupuesto(Presupuestos presupuestos)
	{
		using var conexion = new SqliteConnection(conection_string);
		conexion.Open();

		string sql = "INSERT INTO Presupuestos (idPresupuesto, NombreDestinatario, FechaCreacion) VALUES (@idPresupuesto, @NombreDestinatario, @FechaCreacion)";

		using var comando = new SqliteCommand(sql, conexion);

		comando.Parameters.Add(new SqliteParameter("@idPresupuesto", presupuestos.idPresupuesto));
		comando.Parameters.Add(new SqliteParameter("@NombreDestinatario", presupuestos.NombreDestinatario));
		comando.Parameters.Add(new SqliteParameter("@FechaCreacion", presupuestos.FechaCreacion));

		comando.ExecuteNonQuery();
	}
	public List<Presupuestos> ListarPresupuestos()
	{
		string sql = "SELECT * FROM Presupuestos;";

		List<Presupuestos> listaPresupuestos = [];

		using var connection = new SqliteConnection(conection_string);
		connection.Open();

		using var comando = new SqliteCommand(sql, connection);
		using SqliteDataReader reader = comando.ExecuteReader();

		while (reader.Read())
		{
			var presupuestos = new Presupuestos()
			{
				idPresupuesto = Convert.ToInt32(reader["idPresupuesto"]),
				NombreDestinatario = reader["NombreDestinatario"].ToString(),
				FechaCreacion = Convert.ToDateTime(reader["FechaCreacion"])
			};

			listaPresupuestos.Add(presupuestos);
		}

		connection.Close();

		return listaPresupuestos;
	}
	public Presupuestos ObtenerDetalles(int id)
	{
		using var conexion = new SqliteConnection(conection_string);
		conexion.Open();

		string sql = @"
						SELECT 
							p.idPresupuesto,
							p.NombreDestinatario,
							p.FechaCreacion,
							pr.idProducto,
							pr.Descripcion,
							pr.Precio,
							d.Cantidad
						FROM Presupuestos p
						INNER JOIN PresupuestosDetalle d ON p.idPresupuesto = d.idPresupuesto
						INNER JOIN Productos pr ON d.idProducto = pr.idProducto
						WHERE p.idPresupuesto = @id;
					";

		using var comando = new SqliteCommand(sql, conexion);
		comando.Parameters.Add(new SqliteParameter("@id", id));

		using var lector = comando.ExecuteReader();

		Presupuestos presupuesto = null;

		while (lector.Read())
		{

			if (presupuesto == null) //Si encontró un registro
			{
				presupuesto = new Presupuestos()
				{
					idPresupuesto = Convert.ToInt32(lector["idPresupuesto"]),
					NombreDestinatario = lector["NombreDestinatario"].ToString(),
					FechaCreacion = Convert.ToDateTime(lector["FechaCreacion"]),
					Detalle = new List<PresupuestosDetalle>()
				};
			}

			var producto = new Productos()
			{
				idProducto = Convert.ToInt32(lector["idProducto"]),
				Descripcion = lector["Descripcion"].ToString(),
				Precio = Convert.ToInt32(lector["Precio"])
			};

			var detalle = new PresupuestosDetalle()
			{
				Producto = producto,
				Cantidad = Convert.ToInt32(lector["Cantidad"])
			};

			presupuesto.Detalle.Add(detalle);
		}

		return presupuesto;
	}
	public void AgregarProductoAPresupuesto(int idPresupuesto, int idProducto, int cantidad)
	{
		using var conexion = new SqliteConnection(conection_string);
		conexion.Open();

		string sql = @"
			INSERT INTO PresupuestosDetalle (idPresupuesto, idProducto, Cantidad)
			VALUES (@idPresupuesto, @idProducto, @Cantidad);
		";

		using var comando = new SqliteCommand(sql, conexion);
		comando.Parameters.Add(new SqliteParameter("@idPresupuesto", idPresupuesto));
		comando.Parameters.Add(new SqliteParameter("@idProducto", idProducto));
		comando.Parameters.Add(new SqliteParameter("@Cantidad", cantidad));

		comando.ExecuteNonQuery();
	}
	public bool EliminarPresupuesto(int id)
	{
		using var conexion = new SqliteConnection(conection_string);
		conexion.Open();

		string sql = "DELETE FROM Presupuestos WHERE idPresupuesto = @id";
		using var comando = new SqliteCommand(sql, conexion);

		comando.Parameters.Add(new SqliteParameter("@id", id));
		int filasAfectadas = comando.ExecuteNonQuery(); 

        return filasAfectadas > 0;
	}
}