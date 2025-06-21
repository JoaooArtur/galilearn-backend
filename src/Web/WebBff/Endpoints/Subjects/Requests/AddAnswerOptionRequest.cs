using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebBff.Endpoints.Routes;

namespace WebBff.Endpoints.Subjects.Requests
{
    public class AddAnswerOptionRequest
    {
        [FromRoute(Name = SubjectRoutes.SubjectId)]
        [JsonRequired]
        public Guid SubjectId { get; set; }

        [FromRoute(Name = SubjectRoutes.LessonId)]
        [JsonRequired]
        public Guid LessonId { get; set; }

        [FromRoute(Name = SubjectRoutes.QuestionId)]
        [JsonRequired]
        public Guid QuestionId { get; set; }
        public string Text { get; set; }
        public bool IsRightAnswer { get; set; }
    }
}
