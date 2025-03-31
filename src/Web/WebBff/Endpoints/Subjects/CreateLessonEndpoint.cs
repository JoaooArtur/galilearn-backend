using Ardalis.ApiEndpoints;
using Asp.Versioning;
using Core.Endpoints.Extensions;
using Core.Shared.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Subject.Shared.Commands;
using Subject.Shared.Response;
using Swashbuckle.AspNetCore.Annotations;
using WebBff.Endpoints.Routes;
using WebBff.Endpoints.Subjects.Requests;

namespace WebBff.Endpoints.Subjects
{
    public sealed class CreateLessonEndpoint(ISender sender) : EndpointBaseAsync
    .WithRequest<CreateLessonRequest>
    .WithActionResult<IdentifierResponse>
    {
        [ApiVersion("1.0")]
        [HttpPost(SubjectRoutes.CreateLesson)]
        [ProducesResponseType(typeof(IdentifierResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(
            Summary = "Create a new Lesson.",
            Description = "Creates a new Lesson based on the provided request data.",
            Tags = [Tags.Subjects])]
        public override async Task<ActionResult<IdentifierResponse>> HandleAsync(
        CreateLessonRequest request,
        CancellationToken cancellationToken = default) =>
        await Result.Create(request)
            .Map(createLessonRequest => new CreateLessonCommand(
                createLessonRequest.SubjectId,
                createLessonRequest.Title,
                createLessonRequest.Content,
                createLessonRequest.Index))
            .Bind(command => sender.Send(command, cancellationToken))
            .Match(Ok, this.HandleFailure);
    }
}
