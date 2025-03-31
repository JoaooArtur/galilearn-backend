using Core.Application.Messaging;
using Core.Application.Pagination;
using Core.Domain.Primitives;
using Core.Shared.Errors;
using Core.Shared.Results;
using Subject.Domain;
using Subject.Persistence.Projections;
using Subject.Shared.Queries;
using Subject.Shared.Response;

namespace Subject.Application.UseCases.Queries
{
    public class GetSubjectByIdHandler(ISubjectProjection<Projection.Subject> projectionGateway) : IQueryHandler<GetSubjectByIdQuery, SubjectResponse>
    {
        public async Task<Result<SubjectResponse>> Handle(GetSubjectByIdQuery query, CancellationToken cancellationToken)
        {
            var subject = await projectionGateway.GetAsync(query.SubjectId, cancellationToken);

            if (subject is null)
                return Result.Failure<SubjectResponse>(new NotFoundError(new/*DomainError.BillNotFound*/("SubjectNotFound", "Subject não encontrado")));

            return Result.Success<SubjectResponse>(
                new(subject.Id,
                    subject.Name,
                    subject.Description,
                    subject.Index,
                    subject.CreatedAt));
        }
    }
}
