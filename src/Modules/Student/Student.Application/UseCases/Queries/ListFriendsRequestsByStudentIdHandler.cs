using Core.Application.Messaging;
using Core.Shared.Results;
using MediatR;
using Student.Domain;
using Student.Domain.Enumerations;
using Student.Persistence.Projections;
using Student.Shared.Queries;
using Student.Shared.Response;

namespace Student.Application.UseCases.Queries
{
    public class ListFriendsRequestsByStudentIdHandler(
        IStudentProjection<Projection.FriendRequests> projectionGateway,
        IStudentProjection<Projection.Student> studentProjectionGateway) : IQueryHandler<ListFriendsRequestsByStudentIdQuery, List<ListFriendsRequestsByStudentIdResponse>>
    {
        public async Task<Result<List<ListFriendsRequestsByStudentIdResponse>>> Handle(ListFriendsRequestsByStudentIdQuery query, CancellationToken cancellationToken)
        {
            var friendsRequests = await projectionGateway.ListAsync(x => x.StudentId == query.StudentId && x.Status == FriendRequestStatus.Pending, cancellationToken);

            var response = new List<ListFriendsRequestsByStudentIdResponse>();

            if(friendsRequests is not null)
                foreach (var friendRequest in friendsRequests) 
                {
                    var friendInfo = await studentProjectionGateway.FindAsync(x => x.Id == friendRequest.FriendId, cancellationToken);
                    response.Add(new ListFriendsRequestsByStudentIdResponse(
                        friendRequest.Id,
                        friendRequest.StudentId,
                        friendRequest.FriendId,
                        friendInfo.Name,
                        friendInfo.Level,
                        friendRequest.Status,
                        friendRequest.CreatedAt));
                }
            return response;
        }
    }
}
