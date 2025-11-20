using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Tp8.Models;

using SistemaVentas.Web.ViewModels; //Necesario para poder llegar a los ViewModels
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.DataProtection.Internal; //Necesario para SelectList

namespace Tp8.Controllers;

public class PresupuestosController : Controller
{
    private readonly ILogger<PresupuestosController> _logger;

    private ProductoRepository _productosRepository;
    private PresupuestosRepository _presupuestosRepository;
    public PresupuestosController(ILogger<PresupuestosController> logger)
    {
        _logger = logger;
        _productosRepository = new ProductoRepository();
        _presupuestosRepository = new PresupuestosRepository();
    }

    [HttpGet]
    public IActionResult Index()
    {
        List<Presupuestos> presupuestos = _presupuestosRepository.ListarPresupuestos();
        return View(presupuestos);
    }

    [HttpGet]
    public IActionResult Create()
    {
        var presupuestoVM = new PresupuestoViewModel();
        return View(presupuestoVM);
    }

    [HttpPost]
    public IActionResult Create(Presupuestos presupuestos)
    {
        _presupuestosRepository.CrearPresupuesto(presupuestos);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Edit(int idPresupuesto)
    {
        Console.WriteLine(idPresupuesto);
        var presupuesto = _presupuestosRepository.ObtenerPresupuesto(idPresupuesto);
        return View(presupuesto);
    }

    [HttpPost]
    public IActionResult Edit(Presupuestos presupuestos)
    {
        _presupuestosRepository.ModificarPresupuesto(presupuestos);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Details(int idPresupuesto)
    {
        var detalle = _presupuestosRepository.ObtenerDetalles(idPresupuesto);
        return View(detalle);
    }

    [HttpGet]
    public IActionResult Delete(int idPresupuesto)
    {
        var presupuestos = _presupuestosRepository.ObtenerPresupuesto(idPresupuesto);
        if (presupuestos == null) return RedirectToAction("Index");
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
}

