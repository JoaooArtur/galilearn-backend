using Microsoft.AspNetCore.Mvc;
using Subject.Domain.Enumerations;

namespace WebBff.Endpoints.Subjects.Requests
{
    public class CreateQuestionRequest
    {
        [FromRoute(Name = "SubjectId")]
        public Guid SubjectId { get; set; }
        [FromRoute(Name = "LessonId")]
        public Guid LessonId { get; set; }
        public string Text { get; set; }
        public string Level { get; set; }
    }
}
