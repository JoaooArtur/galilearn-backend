using Core.Application.Messaging;
using Core.Shared.Results;
using MediatR;
using Student.Domain;
using Student.Persistence.Projections;
using Student.Shared.Queries;
using Student.Shared.Response;

namespace Student.Application.UseCases.Queries
{
    public class ListStudentsByNameHandler(
        IStudentProjection<Projection.Student> projectionGateway,
        ISender sender) : IQueryHandler<ListStudentsByNameQuery, List<StudentsByNameResponse>>
    {
        public async Task<Result<List<StudentsByNameResponse>>> Handle(ListStudentsByNameQuery query, CancellationToken cancellationToken)
        {
            var students = await projectionGateway.ListAsync(x => x.Name.ToLower().Contains(query.Name.ToLower()), cancellationToken);

            return Result.Success(students.Select(x => new StudentsByNameResponse(x.Id, x.Name, x.Email)).ToList());
        }
    }
}
