using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebBff.Endpoints.Routes;

namespace WebBff.Endpoints.Students.Requests
{
    public class AnswerAttemptRequest
    {
        [FromRoute(Name = StudentsRoutes.AttemptId)]
        [JsonRequired]
        public Guid AttempId { get; set; }
        [FromRoute(Name = StudentsRoutes.SubjectId)]
        [JsonRequired]
        public Guid SubjectId { get; set; }
        [FromRoute(Name = StudentsRoutes.LessonId)]
        [JsonRequired]
        public Guid LessonId { get; set; }
        [FromHeader(Name = StudentsRoutes.Token)]
        public string Token { get; set; }
        [FromRoute(Name = StudentsRoutes.StudentId)]
        [JsonRequired]
        public Guid StudentId { get; set; }
        [JsonRequired]
        public Guid QuestionId { get; set; }
        [JsonRequired]
        public Guid AnswerId { get; set; }
    }
}
