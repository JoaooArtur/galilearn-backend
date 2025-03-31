using Core.Application.Messaging;
using Core.Application.Pagination;
using Core.Domain.Primitives;
using Core.Shared.Results;
using Subject.Domain;
using Subject.Persistence.Projections;
using Subject.Shared.Queries;
using Subject.Shared.Response;

namespace Subject.Application.UseCases.Queries
{
    public class PagedPaymentsByOrderIdHandler(ISubjectProjection<Projection.Subject> projectionGateway) : IQueryHandler<PagedSubjectQuery, IPagedResult<PagedSubjectResponse>>
    {
        public async Task<Result<IPagedResult<PagedSubjectResponse>>> Handle(PagedSubjectQuery query, CancellationToken cancellationToken)
        {
            var subjects = await projectionGateway.ListAsync(cancellationToken);

            return subjects is not null
                ? Result.Success(PagedResult<PagedSubjectResponse>.Create(query.Paging, subjects.Select(subject => new PagedSubjectResponse(
                subject.Id, subject.Name, subject.Description, subject.Index, subject.CreatedAt)).AsQueryable()))
                : Result.Success<IPagedResult<PagedSubjectResponse>>(default);
        }
    }
}
