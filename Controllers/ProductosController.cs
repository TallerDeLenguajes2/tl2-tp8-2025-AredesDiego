using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using SistemaVentas.Web.ViewModels;
using Tp8.Models;

using Mvc.Interfaces;

namespace Tp8.Controllers;

public class ProductosController : Controller
{
    private IProductoRepository _productoRepository;
    private IAuthenticationService _authService;    
    public ProductosController(IProductoRepository productoRepository, IAuthenticationService authService)
    {
        _productoRepository = productoRepository;
        _authService = authService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var securityCheck = CheckAdminPermissions();
        if(securityCheck != null) return securityCheck;

        List<Productos> productos = _productoRepository.ListarProductos();
        return View(productos);
    }
    [HttpGet]
    private IActionResult CheckAdminPermissions()
    {
        if(!_authService.IsAuthenticated())
        {
            return RedirectToAction("Index", "Login");
        }
        if(!_authService.HasAccessLevel("Administrador"))
        {
            return RedirectToAction(nameof(AccesoDenegado));
        }
        return null;
    }
    public IActionResult AccesoDenegado()
    {
        return View();
    }    
    
    [HttpGet]
    public IActionResult Create()
    {
        var securityCheck = CheckAdminPermissions();
        if (securityCheck != null) return securityCheck;

        return View();
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
        var securityCheck = CheckAdminPermissions();
        if (securityCheck != null) return securityCheck;

        Productos producto = _productoRepository.ObtenerDetalles(id);
        if(producto == null) return RedirectToAction("Index");

        var productoVM = new ProductoViewModel
        {
            idProducto = producto.idProducto,
            Descripcion = producto.Descripcion,
            Precio = producto.Precio
        };
        
        if (producto == null)
        {
            return NotFound();
        }

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
        var securityCheck = CheckAdminPermissions();
        if (securityCheck != null) return securityCheck;

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

