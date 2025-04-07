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
    public class ListFriendsRequestsByStudentIdEndpoint(ISender sender) : EndpointBaseAsync
        .WithRequest<ListFriendsRequestsByStudentIdRequest>
        .WithActionResult<List<ListFriendsRequestsByStudentIdResponse>>
    {
        [ApiVersion("1.0")]
        [HttpGet(StudentsRoutes.ListFriendsRequestsByStudentById)]
        [ProducesResponseType(typeof(List<ListFriendsRequestsByStudentIdResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(
            Summary = "List friends requests by student Id.",
            Description = "List friends requests by student id based on the provided request data.",
            Tags = [Tags.Students])]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Policies.Student)]
        public override async Task<ActionResult<List<ListFriendsRequestsByStudentIdResponse>>> HandleAsync(ListFriendsRequestsByStudentIdRequest request, CancellationToken cancellationToken = default)
            => await Result.Create(request)
            .Map(listFriendsByStudentIdRequest => new ListFriendsRequestsByStudentIdQuery(
                ClaimsPrincipalExtensions.ExtractIdFromToken(listFriendsByStudentIdRequest.Token)))
            .Bind(query => sender.Send(query, cancellationToken))
            .Match(Ok, this.HandleFailure);
    }
}
