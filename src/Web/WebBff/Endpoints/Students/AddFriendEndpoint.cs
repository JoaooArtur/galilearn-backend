using Ardalis.ApiEndpoints;
using Asp.Versioning;
using Core.Endpoints.Extensions;
using Core.Shared.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Student.Shared.Commands;
using Swashbuckle.AspNetCore.Annotations;
using WebBff.Endpoints.Routes;

namespace WebBff.Endpoints.Students
{
    //public sealed class AddFriendEndpoint(ISender sender) : EndpointBaseAsync
    //    .WithRequest<AddFriendRequest>
    //    .WithActionResult
    //{
    //    [ApiVersion("1.0")]
    //    [HttpPost(StudentsRoutes.AddFriend)]
    //    [ProducesResponseType(StatusCodes.Status200OK)]
    //    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    //    [SwaggerOperation(
    //        Summary = "Add a friend.",
    //        Description = "Add a friend based on the provided request data.",
    //        Tags = [Tags.Students])]
    //    public override async Task<ActionResult> HandleAsync(
    //    AddFriendRequest request,
    //    CancellationToken cancellationToken = default) =>
    //    await Result.Create(request)
    //        .Map(createPaymentProfileRequest => new AddFriendCommand())
    //        .Bind(command => sender.Send(command, cancellationToken))
    //        .Match(Ok, this.HandleFailure);
    //}
}
