using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Tp8.Models;

using SistemaVentas.Web.ViewModels; //Necesario para poder llegar a los ViewModels
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.DataProtection.Internal; //Necesario para SelectList

namespace Tp8.Controllers;
using Mvc.Interfaces;

public class PresupuestosController : Controller
{
    private readonly ILogger<PresupuestosController> _logger;

    private IPresupuestosRepository _presupuestosRepository;
    private IProductoRepository _productosRepository;
    private IAuthenticationService _authService;    
    public PresupuestosController(IPresupuestosRepository presupuestosRepository, IProductoRepository productosRepository, IAuthenticationService authService)
    {
        _productosRepository = productosRepository;
        _presupuestosRepository = presupuestosRepository;
        _authService = authService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        if(!_authService.IsAuthenticated())
        {
            return RedirectToAction("Index", "Login");
        }
        if(_authService.HasAccessLevel("Administrador") || _authService.HasAccessLevel("Cliente"))
        {
            List<Presupuestos> presupuestos = _presupuestosRepository.ListarPresupuestos();
            return View(presupuestos);
        }
        else
        {
            return RedirectToAction("Index", "Login");
        }
    }

    [HttpGet]
    public IActionResult Create()
    {
        if(!_authService.IsAuthenticated())
        {
            return RedirectToAction("Index", "Login");
        }
        if(!_authService.HasAccessLevel("Administrador"))
        {
            return RedirectToAction(nameof(AccesoDenegado));
        }
            return View(new PresupuestoViewModel());
    }

    [HttpPost]
    public IActionResult Create(PresupuestoViewModel presupuestosVM)
    {
        if(presupuestosVM.FechaCreacion > DateTime.Today)
        {
            ModelState.AddModelError("FechaCreacion", "La fecha de creación no puede ser una fecha futura.");
        }
        if(!ModelState.IsValid)
        {
            return View(presupuestosVM);
        }

        var nuevoPresupuesto = new Presupuestos
        {
            NombreDestinatario = presupuestosVM.NombreDestinatario,
            FechaCreacion = presupuestosVM.FechaCreacion
        };
        
        _presupuestosRepository.CrearPresupuesto(nuevoPresupuesto);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Edit(int idPresupuesto)
    {
         if (!_authService.IsAuthenticated())
        {
            return RedirectToAction("Index", "Login");
        }
        if (!_authService.HasAccessLevel("Administrador"))
        {
            return RedirectToAction(nameof(AccesoDenegado));
        }

        
        var presupuesto = _presupuestosRepository.ObtenerPresupuesto(idPresupuesto);
        if (presupuesto == null) return NotFound();

        var presupuestosVM = new PresupuestoViewModel(presupuesto);

        return View(presupuestosVM);
    }

    [HttpPost]
    public IActionResult Edit(int id, PresupuestoViewModel presupuestoVM)
    {
        if (id != presupuestoVM.idPresupuesto) return NotFound();

        // ❗ 1. VALIDACIÓN DE REGLA DE NEGOCIO Específica
        if (presupuestoVM.FechaCreacion > DateTime.Today)
        {
            ModelState.AddModelError("FechaCreacion", "La fecha de creación no puede ser una fecha futura.");
        }
        if (!ModelState.IsValid)
        {
            // ❌ Si falla: Retorna a la vista con el VM
            return View(presupuestoVM); 
        }
        var presupuestoAEditar = new Presupuestos
        {
            idPresupuesto = presupuestoVM.idPresupuesto,
            NombreDestinatario = presupuestoVM.NombreDestinatario,
            FechaCreacion = presupuestoVM.FechaCreacion
        };
        
        _presupuestosRepository.ModificarPresupuesto(presupuestoAEditar);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Details(int idPresupuesto)
    {
        var detalle = _presupuestosRepository.ObtenerDetalles(idPresupuesto);
        if(detalle ==null)
        {
            return NotFound();
        }
        return View(detalle);
    }

    [HttpGet]
    public IActionResult Delete(int idPresupuesto)
    {
         if (!_authService.IsAuthenticated())
        {
            return RedirectToAction("Index", "Login");
        }

        // Comprobación manual de nivel de acceso
        if (!_authService.HasAccessLevel("Administrador"))
        {
            return RedirectToAction(nameof(AccesoDenegado));
        }

        var presupuestos = _presupuestosRepository.ObtenerPresupuesto(idPresupuesto);
        if (presupuestos == null) return NotFound();
        return View(presupuestos);
    }
    
    [HttpPost]
    public IActionResult DeleteConfirmacion(int idPresupuesto)
    {
        _presupuestosRepository.EliminarPresupuesto(idPresupuesto);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult AgregarProducto(int idPresupuesto)
    {
        if (!_authService.IsAuthenticated())
        {
            return RedirectToAction("Index", "Login");
        }
        if (!_authService.HasAccessLevel("Administrador"))
        {
            return RedirectToAction(nameof(AccesoDenegado));
        }

        List<Productos> productos = _productosRepository.ListarProductos();

        AgregarProductoViewModel model = new AgregarProductoViewModel
        {
            idPresupuesto = idPresupuesto,
            ListaProductos = new SelectList(productos, "idProducto", "Descripcion")
        };
        return View(model);
    }

    [HttpPost]
    public IActionResult AgregarProducto(AgregarProductoViewModel model)
    {
        if(!ModelState.IsValid) //Chequeo de seguridad para la cantidad
        {
            foreach (var modelStateKey in ModelState.Keys)
            {
                var modelStateVal = ModelState[modelStateKey];
                foreach (var error in modelStateVal.Errors)
                {
                    // Imprime el nombre del campo y el error de validación exacto.
                    Console.WriteLine($"Error en el campo '{modelStateKey}': {error.ErrorMessage}");
                }
            }
            
            //LOGICA CRITICA DE RECARGA: Si falla la validacion, debemos recargar el SelectList porque se pierde el POST.
            var productos = _productosRepository.ListarProductos();
            model.ListaProductos = new SelectList(productos, "idProducto", "Descripcion");

            return View(model);
        }
        
        //Si es VALIDO: Llamamos al repositorio para guardar la relacion
        _presupuestosRepository.AgregarProductoAPresupuesto(model.idPresupuesto, model.idProducto, model.Cantidad);
    
        //Rederigimos al detalle del presupuesto
        return RedirectToAction(nameof(Details), new {idPresupuesto = model.idPresupuesto});
    }

    [HttpGet]
    public IActionResult AccesoDenegado()
    {
        return View();
    }
}

