using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering; //Necesario para SelecList

namespace SistemaVentas.Web.ViewModels
{
    public class AgregarProductoViewModel
    {
        public int idPresupuesto{get; set;}

        [Display(Name = "Producto o agregar")]
        public int idProducto {get; set;}

        [Display(Name = "Cantidad")]
        [Required(ErrorMessage = "La cantidad es obligatoria")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a cero")]
        public int Cantidad {get; set;}

        public SelectList ListaProductos{get; set;}
    }
}