using Microsoft.AspNetCore.Mvc;
using WebBff.Endpoints.Routes;

namespace WebBff.Endpoints.Students.Requests
{
    public class AcceptFriendRequestRequest
    {
        [FromRoute(Name = StudentsRoutes.StudentId)]
        public Guid StudentId { get; set; }
        [FromRoute(Name = StudentsRoutes.RequestId)]
        public Guid RequestId { get; set; }
    }
}
