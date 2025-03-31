using Core.Application.Messaging;
using Microsoft.AspNetCore.Mvc;
using WebBff.Endpoints.Routes;

namespace WebBff.Endpoints.Subjects.Requests
{
    public class CreateLessonRequest
    {
        [FromRoute(Name = SubjectRoutes.SubjectId)]
        public Guid SubjectId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int Index { get; set; }
    }
}
