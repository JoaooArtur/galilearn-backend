
namespace Subject.Shared.Response
{
    public sealed record PagedSubjectResponse(
            Guid Id,
            string Name,
            string Description,
            int Index,
            DateTimeOffset CreatedAt);
}
