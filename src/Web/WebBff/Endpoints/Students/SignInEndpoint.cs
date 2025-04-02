using Ardalis.ApiEndpoints;
using Asp.Versioning;
using Core.Endpoints.Extensions;
using Core.Shared.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Student.Shared.Commands;
using Student.Shared.Response;
using Swashbuckle.AspNetCore.Annotations;
using WebBff.Endpoints.Routes;
using WebBff.Endpoints.Students.Requests;

namespace WebBff.Endpoints.Students
{
    public sealed class SignInEndpoint(ISender sender) : EndpointBaseAsync
        .WithRequest<SignInRequest>
        .WithActionResult<SignInResponse>
    {
        [ApiVersion("1.0")]
        [HttpPost(StudentsRoutes.SignIn)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(
            Summary = "Sign in.",
            Description = "Sign in based on the provided request data.",
            Tags = [Tags.Students])]
        public override async Task<ActionResult<SignInResponse>> HandleAsync(
        [FromBody]SignInRequest request,
        CancellationToken cancellationToken = default) =>
        await Result.Create(request)
            .Map(signInRequest => new SignInCommand(
                signInRequest.Email,
                signInRequest.Password))
            .Bind(command => sender.Send(command, cancellationToken))
            .Match(Ok, this.HandleFailure);
    }
}
