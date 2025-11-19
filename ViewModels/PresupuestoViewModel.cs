using System.ComponentModel.DataAnnotations;

namespace SistemaVentas.Web.ViewModels
{
    public class PresupuestoViewModel
    {
        public int idPresupuesto {get; set;}

        [Display(Name = "Nombre o email del Destinatario")]
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [EmailAddress(ErrorMessage = "El formato del email no es valido")]
        public string NombreDestinatario {get; set;}

        [Display(Name = "Fecha de Creacion")]
        [Required(ErrorMessage = "La fecha es obligatoria")]
        [DataType(DataType.Date)]
        public DateTime FechaCreacion {get; set;}
    }
}