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
    public IActionResult Create(int id)
    {
        var presupuesto = _presupuestosRepository.ObtenerDetalles(id);
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
        var presupuesto = _presupuestosRepository.ObtenerDetalles(idPresupuesto);
        return View(presupuesto);
    }

    [HttpPost]
    public IActionResult Edit(Presupuestos presupuestos)
    {
        _presupuestosRepository.ModificarPresupuesto(presupuestos);
        return RedirectToAction("Index");
    }
}

