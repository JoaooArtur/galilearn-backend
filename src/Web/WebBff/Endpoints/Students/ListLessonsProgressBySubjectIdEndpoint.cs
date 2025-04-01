using Ardalis.ApiEndpoints;
using Asp.Versioning;
using Core.Endpoints.Extensions;
using Core.Shared.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Student.Shared.Queries;
using Student.Shared.Response;
using Swashbuckle.AspNetCore.Annotations;
using WebBff.Endpoints.Routes;
using WebBff.Endpoints.Students.Requests;

namespace WebBff.Endpoints.Students
{
    public class ListLessonsProgressBySubjectIdEndpoint(ISender sender) : EndpointBaseAsync
        .WithRequest<ListLessonsProgressBySubjectIdRequest>
        .WithActionResult<List<LessonsProgressBySubjectIdResponse>>
    {
        [ApiVersion("1.0")]
        [HttpGet(StudentsRoutes.ListLessonProgressBySubjectId)]
        [ProducesResponseType(typeof(List<LessonsProgressBySubjectIdResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(
            Summary = "List subjects progress by student Id.",
            Description = "List subjects progress by student id based on the provided request data.",
            Tags = [Tags.Students])]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Policies.Student)]
        public override async Task<ActionResult<List<LessonsProgressBySubjectIdResponse>>> HandleAsync(ListLessonsProgressBySubjectIdRequest request, CancellationToken cancellationToken = default)
            => await Result.Create(request)
            .Map(listLessonsBySubjectIdRequest => new ListLessonProgressBySubjectIdQuery(
                listLessonsBySubjectIdRequest.SubjectId, listLessonsBySubjectIdRequest.StudentId))
            .Bind(query => sender.Send(query, cancellationToken))
            .Match(Ok, this.HandleFailure);
    }
}
