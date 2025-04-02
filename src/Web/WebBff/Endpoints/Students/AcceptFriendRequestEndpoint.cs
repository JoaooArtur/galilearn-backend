using Ardalis.ApiEndpoints;
using Asp.Versioning;
using Core.Endpoints.Extensions;
using Core.Shared.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Student.Shared.Commands;
using Swashbuckle.AspNetCore.Annotations;
using WebBff.Endpoints.Routes;
using WebBff.Endpoints.Students.Requests;

namespace WebBff.Endpoints.Students
{
    public sealed class AcceptFriendRequestEndpoint(ISender sender) : EndpointBaseAsync
        .WithRequest<AcceptFriendRequestRequest>
        .WithActionResult
    {
        [ApiVersion("1.0")]
        [HttpPost(StudentsRoutes.AcceptFriendRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(
            Summary = "Accept a friend request.",
            Description = "Accept friend request based on the provided request data.",
            Tags = [Tags.Students])]
        public override async Task<ActionResult> HandleAsync(
        AcceptFriendRequestRequest request,
        CancellationToken cancellationToken = default) =>
        await Result.Create(request)
            .Map(acceptFriendRequest => new AcceptFriendRequestCommand(
                acceptFriendRequest.StudentId,
                acceptFriendRequest.RequestId))
            .Bind(command => sender.Send(command, cancellationToken))
            .Match(Ok, this.HandleFailure);
    }
}
