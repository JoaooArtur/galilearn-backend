using Ardalis.ApiEndpoints;
using Asp.Versioning;
using Core.Endpoints.Extensions;
using Core.Shared.Extensions;
using Core.Shared.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Student.Shared.Commands;
using Student.Shared.Response;
using Subject.Shared.Response;
using Swashbuckle.AspNetCore.Annotations;
using WebBff.Endpoints.Routes;
using WebBff.Endpoints.Students.Requests;

namespace WebBff.Endpoints.Students
{
    public sealed class AnswerAttemptEndpoint(ISender sender) : EndpointBaseAsync
        .WithRequest<AnswerAttemptRequest>
        .WithActionResult<CorrectAnswerResponse>
    {
        [ApiVersion("1.0")]
        [HttpPost(StudentsRoutes.AnswerAttempt)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(
            Summary = "Answer a attempt.",
            Description = "Answer a attempt based on the provided request data.",
            Tags = [Tags.Students])]
        public override async Task<ActionResult<CorrectAnswerResponse>> HandleAsync(
        AnswerAttemptRequest request,
        CancellationToken cancellationToken = default) =>
        await Result.Create(request)
            .Map(answerAttemptRequest => new AnswerAttemptCommand(
                answerAttemptRequest.StudentId, answerAttemptRequest.SubjectId, answerAttemptRequest.LessonId, answerAttemptRequest.AttempId, answerAttemptRequest.QuestionId, answerAttemptRequest.AnswerId))
            .Bind(command => sender.Send(command, cancellationToken))
            .Match(Ok, this.HandleFailure);
    }
}
