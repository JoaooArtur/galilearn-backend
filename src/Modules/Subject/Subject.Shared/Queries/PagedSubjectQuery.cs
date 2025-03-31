using Core.Application.Messaging;
using Core.Domain.Primitives;
using Subject.Shared.Response;

namespace Subject.Shared.Queries
{
    public sealed record PagedSubjectQuery(Paging Paging) : IQuery<IPagedResult<PagedSubjectResponse>>;
}
