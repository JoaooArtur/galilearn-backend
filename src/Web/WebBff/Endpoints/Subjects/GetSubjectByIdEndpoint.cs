using Ardalis.ApiEndpoints;
using Asp.Versioning;
using Common.Policies;
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
    public sealed class GetSubjectByIdEndpoint(ISender sender) : EndpointBaseAsync
        .WithRequest<GetSubjectByIdRequest>
        .WithActionResult<SubjectResponse>
    {
        [ApiVersion("1.0")]
        [HttpGet(SubjectRoutes.GetSubjectById)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Summary = "Get Subject By SubjectId",
            Description = "Get subject By SubjectId based on the provided request data.",
            Tags = [Tags.Subjects])]
        
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Policies.Customer)]
        public override async Task<ActionResult<SubjectResponse>> HandleAsync(
        GetSubjectByIdRequest request,
        CancellationToken cancellationToken = default) =>
        await Result.Create(request)
            .Map(request => new GetSubjectByIdQuery(
                request.SubjectId))
            .Bind(query => sender.Send(query, cancellationToken))
            .Match(Ok, this.HandleFailure);
    }
}
