using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Subject.Domain.Enumerations;
using WebBff.Endpoints.Routes;

namespace WebBff.Endpoints.Subjects.Requests
{
    public class CreateQuestionRequest
    {
        [FromRoute(Name = SubjectRoutes.SubjectId)]
        [JsonRequired]
        public Guid SubjectId { get; set; }
        [FromRoute(Name = SubjectRoutes.LessonId)]
        [JsonRequired]
        public Guid LessonId { get; set; }
        public string Text { get; set; }
        public string Level { get; set; }
    }
}
