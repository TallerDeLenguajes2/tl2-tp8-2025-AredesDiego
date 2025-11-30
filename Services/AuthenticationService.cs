using Mvc.Interfaces;
namespace Mvc.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUserRepository _userRepository;
    private readonly IHttpContextAccessor _httpContextAccesor;

    public AuthenticationService(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
    {
        _userRepository = userRepository;
        _httpContextAccesor = httpContextAccessor;
    }

    public bool Login(string username, string password)
    {
        var context = _httpContextAccesor.HttpContext;
        var user = _userRepository.GetUser(username, password);

        if(user != null)
        {
            if(context == null)
            {
                throw new InvalidOperationException("HttpContext no esta disponible");
            }
            
            context.Session.SetString("IsAuthenticated", "true");
            context.Session.SetString("User", user.User);
            context.Session.SetString("UserNombre", user.Nombre);
            context.Session.SetString("Rol", user.Rol);

            return true;
        }
        return false;
    }
    public void Logout()
    {
        var context = _httpContextAccesor.HttpContext;

        if(context == null)
        {
            throw new InvalidOperationException("HttpContext no esta disponible");
        }
        /* context.Session.Remove("IsAuthenticated");
        context.Session.Remove("User");
        context.Session.Remove("UserNombre");
        context.Session.Remove("Rol");*/

        context.Session.Clear();
    }
    public bool IsAutheticated()
    {
        var context = _httpContextAccesor.HttpContext;
        if(context == null)
        {
            throw new InvalidOperationException("HttpContext no esta disponible");
        }
        return context.Session.GetString("IsAuthenticated") == "true";
        
    }
    public bool HasAccessLevel(string requiredAccessLevel)
    {
        var context = _httpContextAccesor.HttpContext;
        if(context == null)
        {
            throw new InvalidOperationException("HttpContext no esta disponible");
        }
        return context.Session.GetString("Rol") == requiredAccessLevel;
    }
}