using Microsoft.AspNetCore.Mvc;
using Mvc.Interfaces;

public class LoginController : Controller
{
    private readonly IAuthenticationService _authenticationService;

    public LoginController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        // Crear un nuevo ViewModel, pasamos el estado de autenticación
        var model = new LoginViewModel()
        {
            IsAuthenticated = HttpContext.Session.GetString("IsAuthenticated") == "true"
        };
        return View(model); // Pasamos el ViewModel con la propiedad de autenticación
    }

    [HttpPost]
    public IActionResult Login(LoginViewModel model)
    {
        if(string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
        {
            model.ErrorMessage = "Debe ingresar usuario y contrasena";
            return View("Index", model);
        }

        if(_authenticationService.Login(model.Username, model.Password))
        {
            return RedirectToAction("Index", "Home");
        }

        model.ErrorMessage = "Credenciales Invalidas";
        return View("Index", model);
    }

    [HttpGet]
    public IActionResult Logout()
    {
        _authenticationService.Logout();
        return RedirectToAction("Index");
    }
}

