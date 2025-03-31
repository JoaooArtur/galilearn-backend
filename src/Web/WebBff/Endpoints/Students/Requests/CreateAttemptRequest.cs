using Microsoft.AspNetCore.Mvc;
using WebBff.Endpoints.Routes;

namespace WebBff.Endpoints.Students.Requests
{
    public class CreateAttemptRequest
    {
        [FromRoute(Name = StudentsRoutes.SubjectId)]
        public Guid SubjectId { get; set; }
        [FromRoute(Name = StudentsRoutes.StudentId)]
        public Guid StudentId { get; set; }
        [FromRoute(Name = StudentsRoutes.LessonId)]
        public Guid LessonId { get; set; }
    }
}
