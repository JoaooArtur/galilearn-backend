﻿using Ardalis.ApiEndpoints;
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
    public sealed class SendFriendRequestEndpoint(ISender sender) : EndpointBaseAsync
        .WithRequest<SendFriendRequestRequest>
        .WithActionResult
    {
        [ApiVersion("1.0")]
        [HttpPost(StudentsRoutes.SendFriendRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(
            Summary = "Create a friend request.",
            Description = "Creates a new friend request based on the provided request data.",
            Tags = [Tags.Students])]
        public override async Task<ActionResult> HandleAsync(
        SendFriendRequestRequest request,
        CancellationToken cancellationToken = default) =>
        await Result.Create(request)
            .Map(sendFriendRequest => new SendFriendRequestCommand(
                sendFriendRequest.StudentId,
                sendFriendRequest.FriendId))
            .Bind(command => sender.Send(command, cancellationToken))
            .Match(Ok, this.HandleFailure);
    }
}
