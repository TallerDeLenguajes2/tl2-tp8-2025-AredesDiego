public class Presupuestos
{
    public int idPresupuesto { get; set; }
    public string NombreDestinatario { get; set; }
    public DateTime FechaCreacion { get; set; }
    public List<PresupuestosDetalle> Detalle { get; set; }

    public decimal MontoPresupuestoConIva(List<PresupuestosDetalle> detalle)
    {
        return MontoPresupuesto(detalle) * (decimal)1.21;
    }
    private decimal MontoPresupuesto(List<PresupuestosDetalle> detalle)
    {
        decimal total = 0;

        foreach (var presupuesto_detalle in detalle)
        {
            total += presupuesto_detalle.Producto.Precio;
        }
        return total;
    }
    public int CantidadProductos()
    {
        int cantidad = 0;
        foreach (var item in Detalle)
        {
            cantidad += item.Cantidad;
        }
        return Detalle.Count; 
    } 
}