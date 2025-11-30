interface IPresupuestosRepository
{
	public void CrearPresupuesto(Presupuestos presupuestos);
	List<Presupuestos> ListarPresupuestos();
	Presupuestos ObtenerDetalles(int id);
	void AgregarProductoAPresupuesto(int idPresupuesto, int idProducto, int cantidad);
	public bool EliminarPresupuesto(int id); 
}