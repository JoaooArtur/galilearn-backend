using Ardalis.ApiEndpoints;
using Asp.Versioning;
using Core.Domain.Primitives;
using Core.Endpoints.Extensions;
using Core.Shared.Extensions;
using Core.Shared.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Subject.Shared.Queries;
using Subject.Shared.Response;
using Swashbuckle.AspNetCore.Annotations;
using WebBff.Endpoints.Routes;
using WebBff.Endpoints.Subjects.Requests;

namespace WebBff.Endpoints.Subjects
{
    public class ListRandomQuestionsByLessonIdEndpoint(ISender sender) : EndpointBaseAsync
        .WithRequest<ListRandomQuestionsByLessonIdRequest>
        .WithActionResult<List<RandomQuestionsByLessonIdResponse>>
    {
        [ApiVersion("1.0")]
        [HttpGet(SubjectRoutes.ListRandomQuestionsByLessonId)]
        [ProducesResponseType(typeof(List<RandomQuestionsByLessonIdResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(
            Summary = "List random questions by lesson Id.",
            Description = "List random questions based on the provided request data.",
            Tags = [Tags.Subjects])]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Policies.Student)]
        public override async Task<ActionResult<List<RandomQuestionsByLessonIdResponse>>> HandleAsync(ListRandomQuestionsByLessonIdRequest request, CancellationToken cancellationToken = default)
            => await Result.Create(request)
            .Map(listrandomQuestionsRequest => new ListRandomQuestionsByLessonIdQuery(listrandomQuestionsRequest.LessonId))
            .Bind(query => sender.Send(query, cancellationToken))
            .Match(Ok, this.HandleFailure);
    }
}
