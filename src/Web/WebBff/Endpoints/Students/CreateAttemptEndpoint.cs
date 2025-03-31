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
    public sealed class CreateAttemptEndpoint(ISender sender) : EndpointBaseAsync
        .WithRequest<CreateAttemptRequest>
        .WithActionResult<IdentifierResponse>
    {
        [ApiVersion("1.0")]
        [HttpPost(StudentsRoutes.CreateAttempt)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(
            Summary = "Create a new attempt.",
            Description = "Creates a new attempt based on the provided request data.",
            Tags = [Tags.Students])]
        public override async Task<ActionResult<IdentifierResponse>> HandleAsync(
        CreateAttemptRequest request,
        CancellationToken cancellationToken = default) =>
        await Result.Create(request)
            .Map(createAttemptRequest => new CreateAttemptCommand(
                createAttemptRequest.StudentId, createAttemptRequest.SubjectId, createAttemptRequest.LessonId))
            .Bind(command => sender.Send(command, cancellationToken))
            .Match(Ok, this.HandleFailure);
    }
}
