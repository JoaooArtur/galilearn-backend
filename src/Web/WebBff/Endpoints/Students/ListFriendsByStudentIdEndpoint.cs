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
    public class ListFriendsByStudentIdEndpoint(ISender sender) : EndpointBaseAsync
        .WithRequest<ListFriendsByStudentIdRequest>
        .WithActionResult<List<ListFriendsByStudentIdResponse>>
    {
        [ApiVersion("1.0")]
        [HttpGet(StudentsRoutes.ListFriendsByStudentById)]
        [ProducesResponseType(typeof(List<ListFriendsByStudentIdResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(
            Summary = "List friends by student Id.",
            Description = "List friends by student id based on the provided request data.",
            Tags = [Tags.Students])]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Policies.Student)]
        public override async Task<ActionResult<List<ListFriendsByStudentIdResponse>>> HandleAsync(ListFriendsByStudentIdRequest request, CancellationToken cancellationToken = default)
            => await Result.Create(request)
            .Map(listFriendsByStudentIdRequest => new ListFriendsByStudentIdQuery(
                listFriendsByStudentIdRequest.StudentId))
            .Bind(query => sender.Send(query, cancellationToken))
            .Match(Ok, this.HandleFailure);
    }
}
