
namespace Student.Shared.Response
{
    public sealed record StudentsByNameResponse(
            Guid Id,
            string Name,
            string Email);
}
