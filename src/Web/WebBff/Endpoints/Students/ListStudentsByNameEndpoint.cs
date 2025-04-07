using Ardalis.ApiEndpoints;
using Asp.Versioning;
using Common.Policies;
using Core.Endpoints.Extensions;
using Core.Shared.Results;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Student.Shared.Queries;
using Student.Shared.Response;
using Swashbuckle.AspNetCore.Annotations;
using WebBff.Endpoints.Routes;
using WebBff.Endpoints.Students.Requests;

namespace WebBff.Endpoints.Students
{
    public class ListStudentsByNameEndpoint(ISender sender) : EndpointBaseAsync
        .WithRequest<ListStudentsByNameRequest>
        .WithActionResult<List<StudentsByNameResponse>>
    {
        [ApiVersion("1.0")]
        [HttpGet(StudentsRoutes.ListStudents)]
        [ProducesResponseType(typeof(List<StudentsByNameResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(
            Summary = "List students by name.",
            Description = "List students by name based on the provided request data.",
            Tags = [Tags.Students])]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Policies.Student)]
        public override async Task<ActionResult<List<StudentsByNameResponse>>> HandleAsync(ListStudentsByNameRequest request, CancellationToken cancellationToken = default)
            => await Result.Create(request)
            .Map(listStudentsRequest => new ListStudentsByNameQuery(
                listStudentsRequest.Name))
            .Bind(query => sender.Send(query, cancellationToken))
            .Match(Ok, this.HandleFailure);
    }
}
