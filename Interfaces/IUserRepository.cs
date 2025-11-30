namespace Mvc.Interfaces;

public interface IUserRepository
{
    Usuario GetUser(string username, string password);
}
