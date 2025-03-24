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
    public sealed class CreateSubjectEndpoint(ISender sender) : EndpointBaseAsync
        .WithRequest<CreateSubjectRequest>
        .WithActionResult<IdentifierResponse>
    {
        [ApiVersion("1.0")]
        [HttpPost(SubjectRoutes.CreateSubject)]
        [ProducesResponseType(typeof(IdentifierResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(
            Summary = "Create a new Subject.",
            Description = "Creates a new Subject based on the provided request data.",
            Tags = [Tags.Subjects])]
        public override async Task<ActionResult<IdentifierResponse>> HandleAsync(
        [FromBody] CreateSubjectRequest request,
        CancellationToken cancellationToken = default) =>
        await Result.Create(request)
            .Map(createSubjectRequest => new CreateSubjectCommand(
                createSubjectRequest.Name,
                createSubjectRequest.Description,
                createSubjectRequest.Index))
            .Bind(command => sender.Send(command, cancellationToken))
            .Match(Ok, this.HandleFailure);
    }
}
