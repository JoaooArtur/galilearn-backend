using Microsoft.AspNetCore.Mvc;
using WebBff.Endpoints.Routes;

namespace WebBff.Endpoints.Subjects.Requests
{
    /// <summary>
    /// Represents the request for get lesson by lesson Id.
    /// </summary>
    /// <param name="LessonId">The LessonId.</param>
    public class GetLessonByIdRequest
    {
        [FromRoute(Name = SubjectRoutes.LessonId)]
        public Guid LessonId { get; set; }
    }
}
