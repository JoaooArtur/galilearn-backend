using Ardalis.ApiEndpoints;
using Asp.Versioning;
using Core.Domain.Primitives;
using Core.Endpoints.Extensions;
using Core.Shared.Results;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Student.Shared.Queries;
using Student.Shared.Response;
using Subject.Shared.Queries;
using Subject.Shared.Response;
using Swashbuckle.AspNetCore.Annotations;
using WebBff.Endpoints.Routes;
using WebBff.Endpoints.Students.Requests;
using WebBff.Endpoints.Subjects.Requests;

namespace WebBff.Endpoints.Students
{
    public class PagedSubjectProgressByStudentIdEdnpoint(ISender sender) : EndpointBaseAsync
        .WithRequest<PagedSubjectsProgressByStudentIdRequest>
        .WithActionResult<IPagedResult<SubjectsProgressByStudentIdResponse>>
    {
        [ApiVersion("1.0")]
        [HttpGet(StudentsRoutes.PagedSubjectProgressByStudentId)]
        [ProducesResponseType(typeof(IPagedResult<SubjectsProgressByStudentIdResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(
            Summary = "List subjects progress by student Id.",
            Description = "List subjects progress by student id based on the provided request data.",
            Tags = [Tags.Students])]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Policies.Student)]
        public override async Task<ActionResult<IPagedResult<SubjectsProgressByStudentIdResponse>>> HandleAsync(PagedSubjectsProgressByStudentIdRequest request, CancellationToken cancellationToken = default)
            => await Result.Create(request)
            .Map(listLessonsBySubjectIdRequest => new PagedSubjectProgressByStudentIdQuery(
                listLessonsBySubjectIdRequest.StudentId,
                new(listLessonsBySubjectIdRequest.Limit, listLessonsBySubjectIdRequest.Offset)))
            .Bind(query => sender.Send(query, cancellationToken))
            .Match(Ok, this.HandleFailure);
    }
}
