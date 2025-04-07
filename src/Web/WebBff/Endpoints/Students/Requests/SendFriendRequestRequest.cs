using Microsoft.AspNetCore.Mvc;
using WebBff.Endpoints.Routes;

namespace WebBff.Endpoints.Students.Requests
{
    public class SendFriendRequestRequest
    {
        [FromHeader(Name = StudentsRoutes.Token)]
        public string Token { get; set; }
        [FromRoute(Name = StudentsRoutes.StudentId)]
        public Guid StudentId { get; set; }
        [FromRoute(Name = StudentsRoutes.FriendId)]
        public Guid FriendId { get; set; }
    }
}
