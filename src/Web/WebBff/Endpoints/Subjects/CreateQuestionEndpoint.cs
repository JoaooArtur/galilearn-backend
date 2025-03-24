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
    public sealed class CreateQuestionEndpoint(ISender sender) : EndpointBaseAsync
        .WithRequest<CreateQuestionRequest>
        .WithActionResult<IdentifierResponse>
    {
        [ApiVersion("1.0")]
        [HttpPost(SubjectRoutes.CreateQuestion)]
        [ProducesResponseType(typeof(IdentifierResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(
            Summary = "Create a new Question.",
            Description = "Creates a new Question based on the provided request data.",
            Tags = [Tags.Subjects])]
        public override async Task<ActionResult<IdentifierResponse>> HandleAsync(
        [FromBody] CreateQuestionRequest request,
        CancellationToken cancellationToken = default) =>
        await Result.Create(request)
            .Map(createQuestionRequest => new CreateQuestionCommand(
                createQuestionRequest.SubjectId,
                createQuestionRequest.LessonId,
                createQuestionRequest.Text,
                createQuestionRequest.Level))
            .Bind(command => sender.Send(command, cancellationToken))
            .Match(Ok, this.HandleFailure);
    }
}
