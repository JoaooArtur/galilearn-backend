using Microsoft.AspNetCore.Mvc;
using WebBff.Endpoints.Routes;

namespace WebBff.Endpoints.Students.Requests
{
    /// <summary>
    /// Represents the request for list friends requests by student Id.
    /// </summary>
    /// <param name="StudentId">The StudentId.</param>
    public class ListFriendsRequestsByStudentIdRequest
    {
        [FromRoute(Name = StudentsRoutes.StudentId)]
        public Guid StudentId { get; set; }
    }
}
