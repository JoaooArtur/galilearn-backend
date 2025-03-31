using Microsoft.AspNetCore.Mvc;
using Subject.Domain.Enumerations;
using WebBff.Endpoints.Routes;

namespace WebBff.Endpoints.Subjects.Requests
{
    public class CreateQuestionRequest
    {
        [FromRoute(Name = SubjectRoutes.SubjectId)]
        public Guid SubjectId { get; set; }
        [FromRoute(Name = SubjectRoutes.LessonId)]
        public Guid LessonId { get; set; }
        public string Text { get; set; }
        public string Level { get; set; }
    }
}
