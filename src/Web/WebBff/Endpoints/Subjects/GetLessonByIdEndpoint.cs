using Ardalis.ApiEndpoints;
using Asp.Versioning;
using Core.Endpoints.Extensions;
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
    public sealed class GetLessonByIdEndpoint(ISender sender) : EndpointBaseAsync
        .WithRequest<GetLessonByIdRequest>
        .WithActionResult<LessonResponse>
    {
        [ApiVersion("1.0")]
        [HttpGet(SubjectRoutes.GetLessonById)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Summary = "Get Lesson By LessonId",
            Description = "Get lesson By LessonId based on the provided request data.",
            Tags = [Tags.Subjects])]

        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Policies.Customer)]
        public override async Task<ActionResult<LessonResponse>> HandleAsync(
        GetLessonByIdRequest request,
        CancellationToken cancellationToken = default) =>
        await Result.Create(request)
            .Map(request => new GetLessonByIdQuery(
                request.LessonId))
            .Bind(query => sender.Send(query, cancellationToken))
            .Match(Ok, this.HandleFailure);
    }
}
