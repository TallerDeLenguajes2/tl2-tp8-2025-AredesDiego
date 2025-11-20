using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Tp8.Models;

namespace Tp8.Controllers;

public class PresupuestosController : Controller
{
    private readonly ILogger<PresupuestosController> _logger;

    private PresupuestosRepository _presupuestosRepository;
    public PresupuestosController(ILogger<PresupuestosController> logger)
    {
        _logger = logger;
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
        var presupuesto = new Presupuestos();
        return View(presupuesto);
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

}

