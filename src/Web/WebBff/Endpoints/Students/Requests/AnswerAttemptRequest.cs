using Microsoft.AspNetCore.Mvc;
using WebBff.Endpoints.Routes;

namespace WebBff.Endpoints.Students.Requests
{
    public class AnswerAttemptRequest
    {
        [FromRoute(Name = StudentsRoutes.AttemptId)]
        public Guid AttempId { get; set; }
        [FromRoute(Name = StudentsRoutes.SubjectId)]
        public Guid SubjectId { get; set; }
        [FromRoute(Name = StudentsRoutes.LessonId)]
        public Guid LessonId { get; set; }
        [FromHeader(Name = StudentsRoutes.Token)]
        public string Token { get; set; }
        [FromRoute(Name = StudentsRoutes.StudentId)]
        public Guid StudentId { get; set; }
        public Guid QuestionId { get; set; }
        public Guid AnswerId { get; set; }
    }
}
