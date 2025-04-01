using Core.Application.Messaging;
using Core.Shared.Results;
using Subject.Domain;
using Subject.Persistence.Projections;
using Subject.Shared.Queries;
using Subject.Shared.Response;

namespace Subject.Application.UseCases.Queries
{
    public class ListSubjectsHandler(ISubjectProjection<Projection.Subject> projectionGateway) : IQueryHandler<ListSubjectsQuery, List<PagedSubjectResponse>>
    {
        public async Task<Result<List<PagedSubjectResponse>>> Handle(ListSubjectsQuery query, CancellationToken cancellationToken)
        {
            var subjects = await projectionGateway.ListAsync(cancellationToken);

            return subjects is not null
                ? Result.Success(subjects.Select(subject => new PagedSubjectResponse(
                subject.Id, subject.Name, subject.Description, subject.Index, subject.CreatedAt)).ToList())
                : Result.Success<List<PagedSubjectResponse>>(default);
        }
    }
}
