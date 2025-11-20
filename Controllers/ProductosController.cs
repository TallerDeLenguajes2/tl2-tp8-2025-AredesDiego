using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using SistemaVentas.Web.ViewModels;
using Tp8.Models;

using SistemaVentas.Web.ViewModels;

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
    public IActionResult Create()
    {
        var productoVM = new ProductoViewModel();
        return View(productoVM);
    }
    
    [HttpPost]
    public IActionResult Create(ProductoViewModel productoVM)
    {
        if(!ModelState.IsValid) return View(productoVM);

        var NuevoProducto = new Productos
        {
            Descripcion = productoVM.Descripcion,
            Precio = productoVM.Precio
        };

        _productoRepository.CrearProducto(NuevoProducto);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        Productos producto = _productoRepository.ObtenerDetalles(id);
        if(producto == null) return RedirectToAction("Index");

        var productoVM = new ProductoViewModel
        {
            idProducto = producto.idProducto,
            Descripcion = producto.Descripcion,
            Precio = producto.Precio
        };

        return View(productoVM);
    }

    [HttpPost]
    public IActionResult Edit(int id, ProductoViewModel productoVM)
    {
        if(id != productoVM.idProducto) return NotFound();
        if (!ModelState.IsValid) return View(productoVM);

        var productoAEditar = new Productos
        {
            idProducto = productoVM.idProducto,
            Descripcion = productoVM.Descripcion,
            Precio = productoVM.Precio
        };

        _productoRepository.ModificarProducto(productoAEditar);
        return RedirectToAction(nameof(Index));
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

