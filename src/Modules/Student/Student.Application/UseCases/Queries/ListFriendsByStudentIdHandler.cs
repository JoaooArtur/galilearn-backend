using Core.Application.Messaging;
using Core.Shared.Results;
using MediatR;
using Student.Domain;
using Student.Persistence.Projections;
using Student.Shared.Queries;
using Student.Shared.Response;

namespace Student.Application.UseCases.Queries
{
    public class ListFriendsByStudentIdHandler(
        IStudentProjection<Projection.Student> projectionGateway) : IQueryHandler<ListFriendsByStudentIdQuery, List<ListFriendsByStudentIdResponse>>
    {
        public async Task<Result<List<ListFriendsByStudentIdResponse>>> Handle(ListFriendsByStudentIdQuery query, CancellationToken cancellationToken)
        {
            var student = await projectionGateway.GetAsync(query.StudentId, cancellationToken);

            var response = new List<ListFriendsByStudentIdResponse>();

            foreach (var friendId in student.Friends)
            {
                var friend = await projectionGateway.GetAsync(friendId, cancellationToken);

                response.Add(new ListFriendsByStudentIdResponse(friendId, friend.Name, friend.Level, friend.DaysStreak));
            }

            return Result.Success(response.OrderByDescending(x => x.Level).ThenBy(n => n.DaysStreak).ToList());
        }
    }
}
