using Microsoft.AspNetCore.Mvc;

namespace WebBff.Endpoints.Subjects.Requests
{
    public class AddAnswerOptionRequest
    {
        [FromRoute(Name = "SubjectId")]
        public Guid SubjectId { get; set; }

        [FromRoute(Name = "LessonId")]
        public Guid LessonId { get; set; }

        [FromRoute(Name = "QuestionId")]
        public Guid QuestionId { get; set; }
        public string Text { get; set; }
        public bool IsRightAnswer { get; set; }
    }
}
