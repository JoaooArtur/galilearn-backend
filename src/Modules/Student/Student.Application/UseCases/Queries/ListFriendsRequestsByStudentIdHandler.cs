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
        ISender sender) : IQueryHandler<ListFriendsRequestsByStudentIdQuery, List<ListFriendsRequestsByStudentIdResponse>>
    {
        public async Task<Result<List<ListFriendsRequestsByStudentIdResponse>>> Handle(ListFriendsRequestsByStudentIdQuery query, CancellationToken cancellationToken)
        {
            var friendsRequests = await projectionGateway.ListAsync(x => x.StudentId == query.StudentId && x.Status == FriendRequestStatus.Pending);

            return friendsRequests is null ?
                Result.Success<List<ListFriendsRequestsByStudentIdResponse>>(default) :
                Result.Success(friendsRequests.Select(friendRequest => new ListFriendsRequestsByStudentIdResponse(
                    friendRequest.Id,
                    friendRequest.StudentId,
                    friendRequest.FriendId,
                    friendRequest.Status,
                    friendRequest.CreatedAt)).ToList());
        }
    }
}
