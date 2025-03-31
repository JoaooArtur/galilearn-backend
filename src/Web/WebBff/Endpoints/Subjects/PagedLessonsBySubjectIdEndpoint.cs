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
    public class PagedLessonsBySubjectIdEndpoint(ISender sender) : EndpointBaseAsync
        .WithRequest<PagedLessonsBySubjectIdRequest>
        .WithActionResult<IPagedResult<PagedLessonBySubjectIdResponse>>
    {
        [ApiVersion("1.0")]
        [HttpGet(SubjectRoutes.PagedLessons)]
        [ProducesResponseType(typeof(IPagedResult<PagedLessonBySubjectIdResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(
            Summary = "List lessons by subject Id.",
            Description = "List lessons by subject id based on the provided request data.",
            Tags = [Tags.Subjects])]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Policies.Student)]
        public override async Task<ActionResult<IPagedResult<PagedLessonBySubjectIdResponse>>> HandleAsync(PagedLessonsBySubjectIdRequest request, CancellationToken cancellationToken = default)
            => await Result.Create(request)
            .Map(listLessonsBySubjectIdRequest => new PagedLessonsBySubjectIdQuery(
                listLessonsBySubjectIdRequest.SubjectId,
                new(listLessonsBySubjectIdRequest.Limit, listLessonsBySubjectIdRequest.Offset)))
            .Bind(query => sender.Send(query, cancellationToken))
            .Match(Ok, this.HandleFailure);
    }
}
