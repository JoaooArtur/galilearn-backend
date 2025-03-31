using Ardalis.ApiEndpoints;
using Asp.Versioning;
using Common.Policies;
using Core.Domain.Primitives;
using Core.Endpoints.Extensions;
using Core.Shared.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Subject.Shared.Queries;
using Subject.Shared.Response;
using Swashbuckle.AspNetCore.Annotations;
using WebBff.Endpoints.Routes;
using WebBff.Endpoints.Subjects.Requests;

namespace WebBff.Endpoints.Subjects
{
    public class PagedSubjectsEndpoint(ISender sender) : EndpointBaseAsync
        .WithRequest<PagedSubjectsRequest>
        .WithActionResult<IPagedResult<PagedSubjectResponse>>
    {
        [ApiVersion("1.0")]
        [HttpGet(SubjectRoutes.PagedSubjects)]
        [ProducesResponseType(typeof(IPagedResult<PagedSubjectResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(
            Summary = "List subjects.",
            Description = "List subjects based on the provided request data.",
            Tags = [Tags.Subjects])]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Policies.Student)]
        public override async Task<ActionResult<IPagedResult<PagedSubjectResponse>>> HandleAsync(PagedSubjectsRequest request, CancellationToken cancellationToken = default)
            => await Result.Create(request)
            .Map(listSubjectsRequest => new PagedSubjectQuery(
                new(listSubjectsRequest.Limit, listSubjectsRequest.Offset)))
            .Bind(query => sender.Send(query, cancellationToken))
            .Match(Ok, this.HandleFailure);
    }
}
