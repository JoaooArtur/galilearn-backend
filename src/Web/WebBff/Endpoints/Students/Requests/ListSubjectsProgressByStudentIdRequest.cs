using Microsoft.AspNetCore.Mvc;
using WebBff.Endpoints.Routes;

namespace WebBff.Endpoints.Students.Requests
{
    /// <summary>
    /// Represents the request for list subjects progressess by student Id.
    /// </summary>
    /// <param name="StudentId">The StudentId.</param>
    public class ListSubjectsProgressByStudentIdRequest
    {
        [FromRoute(Name = StudentsRoutes.StudentId)]
        public Guid StudentId { get; set; }
    }
}
