using Ardalis.ApiEndpoints;
using Asp.Versioning;
using Core.Domain.Primitives;
using Core.Endpoints.Extensions;
using Core.Shared.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Student.Shared.Queries;
using Student.Shared.Response;
using Subject.Shared.Queries;
using Swashbuckle.AspNetCore.Annotations;
using WebBff.Endpoints.Routes;
using WebBff.Endpoints.Students.Requests;

namespace WebBff.Endpoints.Students
{
    public class ListSubjectProgressByStudentIdEndpoint(ISender sender) : EndpointBaseAsync
        .WithRequest<ListSubjectsProgressByStudentIdRequest>
        .WithActionResult<List<SubjectsProgressByStudentIdResponse>>
    {
        [ApiVersion("1.0")]
        [HttpGet(StudentsRoutes.ListSubjectProgressByStudentId)]
        [ProducesResponseType(typeof(List<SubjectsProgressByStudentIdResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(
            Summary = "List subjects progress by student Id.",
            Description = "List subjects progress by student id based on the provided request data.",
            Tags = [Tags.Students])]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Policies.Student)]
        public override async Task<ActionResult<List<SubjectsProgressByStudentIdResponse>>> HandleAsync(ListSubjectsProgressByStudentIdRequest request, CancellationToken cancellationToken = default)
            => await Result.Create(request)
            .Map(listLessonsBySubjectIdRequest => new ListSubjectsProgressByStudentIdQuery(
                listLessonsBySubjectIdRequest.StudentId))
            .Bind(query => sender.Send(query, cancellationToken))
            .Match(Ok, this.HandleFailure);
    }
}
