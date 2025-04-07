using Microsoft.AspNetCore.Mvc;
using WebBff.Endpoints.Routes;

namespace WebBff.Endpoints.Students.Requests
{
    /// <summary>
    /// Represents the request for list users by Name.
    /// </summary>
    /// <param name="Name">The name.</param>
    public class ListStudentsByNameRequest
    {
        [FromQuery]
        public string Name { get; set; }
    }
}
