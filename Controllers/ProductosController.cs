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
    

}

