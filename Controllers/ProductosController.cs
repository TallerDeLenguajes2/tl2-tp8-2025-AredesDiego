using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Tp8.Models;

namespace Tp8.Controllers;

public class ProductosController : Controller
{
    private readonly ILogger<ProductosController> _logger;
    private ProductoRepository _productoRepository;
    public ProductosController(ILogger<ProductosController> logger)
    {
        _logger = logger;
        _productoRepository = new ProductoRepository();
    }

    [HttpGet]
    public IActionResult Index()
    {
        List<Productos> productos = _productoRepository.ListarProductos();
        return View(productos);
    }
    
    [HttpGet]
    public IActionResult Create(int id)
    {
        Productos producto = new Productos();
        return View(producto);
    }
    
    [HttpPost]
    public IActionResult Create(Productos producto)
    {
        _productoRepository.CrearProducto(producto);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        Productos producto = _productoRepository.ObtenerDetalles(id);
        if(producto == null)
        {
            return RedirectToAction("Index");
        }

        return View(producto);
    }

    [HttpPost]
    public IActionResult Edit(Productos producto)
    {
        _productoRepository.ModificarProducto(producto);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Delete(int id)
    {
        var producto = _productoRepository.ObtenerDetalles(id);
        if (producto == null) return RedirectToAction("Index");
        return View(producto);
    }
    
    [HttpPost]
    public IActionResult DeleteConfirmacion(int idProducto)
    {
        _productoRepository.EliminarProducto(idProducto);
        return RedirectToAction("Index");
    }

}

