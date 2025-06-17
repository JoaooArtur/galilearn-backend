
namespace Student.Application.Services
{
    public interface ITokenService
    {
        string GenerateToken(string username, Guid id, string email, string role);
    }
}
