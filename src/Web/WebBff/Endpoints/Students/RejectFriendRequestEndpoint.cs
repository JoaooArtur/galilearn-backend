using Ardalis.ApiEndpoints;
using Asp.Versioning;
using Common.Policies;
using Core.Endpoints.Extensions;
using Core.Shared.Results;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Student.Shared.Commands;
using Swashbuckle.AspNetCore.Annotations;
using WebBff.Endpoints.Routes;
using WebBff.Endpoints.Students.Requests;

namespace WebBff.Endpoints.Students
{
    public sealed class RejectFriendRequestEndpoint(ISender sender) : EndpointBaseAsync
        .WithRequest<RejectFriendRequestRequest>
        .WithActionResult
    {
        [ApiVersion("1.0")]
        [HttpPost(StudentsRoutes.RejectFriendRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(
            Summary = "Rejects a friend request.",
            Description = "Rejects friend request based on the provided request data.",
            Tags = [Tags.Students])]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Policies.Student)]
        public override async Task<ActionResult> HandleAsync(
        RejectFriendRequestRequest request,
        CancellationToken cancellationToken = default) =>
        await Result.Create(request)
            .Map(rejectFriendRequest => new RejectFriendRequestCommand(
                ClaimsPrincipalExtensions.ExtractIdFromToken(rejectFriendRequest.Token),
                rejectFriendRequest.RequestId))
            .Bind(command => sender.Send(command, cancellationToken))
            .Match(Ok, this.HandleFailure);
    }
}
