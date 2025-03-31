
namespace Subject.Shared.Response
{
    public sealed record SubjectResponse(
            Guid Id,
            string Name,
            string Description,
            int Index,
            DateTimeOffset CreatedAt);
}
