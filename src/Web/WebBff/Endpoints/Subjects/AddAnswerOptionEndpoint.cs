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
    public sealed class AddAnswerOptionEndpoint(ISender sender) : EndpointBaseAsync
    .WithRequest<AddAnswerOptionRequest>
    .WithActionResult<IdentifierResponse>
    {
        [ApiVersion("1.0")]
        [HttpPost(SubjectRoutes.AddAnswerOption)]
        [ProducesResponseType(typeof(IdentifierResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(
            Summary = "Create a new Answer Option.",
            Description = "Creates a new Answer Option for a question based on the provided request data.",
            Tags = [Tags.Subjects])]
        public override async Task<ActionResult<IdentifierResponse>> HandleAsync(
        AddAnswerOptionRequest request,
        CancellationToken cancellationToken = default) =>
        await Result.Create(request)
            .Map(addAnswerOptionRequest => new AddAnswerOptionCommand(
                addAnswerOptionRequest.SubjectId,
                addAnswerOptionRequest.LessonId,
                addAnswerOptionRequest.QuestionId,
                addAnswerOptionRequest.Text,
                addAnswerOptionRequest.IsRightAnswer))
            .Bind(command => sender.Send(command, cancellationToken))
            .Match(Ok, this.HandleFailure);
    }
}
