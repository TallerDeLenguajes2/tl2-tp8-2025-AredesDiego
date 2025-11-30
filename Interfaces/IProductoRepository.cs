public interface IProductoRepository
{
    void CrearProducto(Productos producto);
    bool ModificarProducto(Productos productos);
    List<Productos> ListarProductos();
    Productos ObtenerDetalles(int id);
    bool EliminarProducto(int id);
}