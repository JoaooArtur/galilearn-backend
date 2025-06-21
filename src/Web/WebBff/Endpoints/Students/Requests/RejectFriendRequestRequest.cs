using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebBff.Endpoints.Routes;

namespace WebBff.Endpoints.Students.Requests
{
    public class RejectFriendRequestRequest
    {
        [FromHeader(Name = StudentsRoutes.Token)]
        public string Token { get; set; }
        [FromRoute(Name = StudentsRoutes.StudentId)]
        [JsonRequired]
        public Guid StudentId { get; set; }
        [FromRoute(Name = StudentsRoutes.RequestId)]
        [JsonRequired]
        public Guid RequestId { get; set; }
    }
}
