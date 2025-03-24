using Core.Application.Messaging;
using Microsoft.AspNetCore.Mvc;

namespace WebBff.Endpoints.Subjects.Requests
{
    public class CreateLessonRequest
    {
        [FromRoute(Name = "SubjectId")]
        public Guid SubjectId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int Index { get; set; }
    }
}
