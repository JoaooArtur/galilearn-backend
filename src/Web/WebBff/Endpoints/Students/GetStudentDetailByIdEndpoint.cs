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
    public class GetStudentDetailByIdEndpoint(ISender sender) : EndpointBaseAsync
        .WithRequest<GetStudentDetailByStudentIdRequest>
        .WithActionResult<StudentDetailResponse>
    {
        [ApiVersion("1.0")]
        [HttpGet(StudentsRoutes.GetStudentById)]
        [ProducesResponseType(typeof(StudentDetailResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(
            Summary = "Get student detail by student Id.",
            Description = "Get student details by student id based on the provided request data.",
            Tags = [Tags.Students])]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Policies.Student)]
        public override async Task<ActionResult<StudentDetailResponse>> HandleAsync(GetStudentDetailByStudentIdRequest request, CancellationToken cancellationToken = default)
            => await Result.Create(request)
            .Map(getStudentDetailByStudentIdRequest => new GetStudentDetailByIdQuery(
                getStudentDetailByStudentIdRequest.StudentId))
            .Bind(query => sender.Send(query, cancellationToken))
            .Match(Ok, this.HandleFailure);
    }
}
