using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebBff.Endpoints.Routes;

namespace WebBff.Endpoints.Subjects.Requests
{
    /// <summary>
    /// Represents the request for list lessons by subject Id.
    /// </summary>
    /// <param name="SubjectId">The SubjectId.</param>
    public class PagedLessonsBySubjectIdRequest
    {
        [FromRoute(Name = SubjectRoutes.SubjectId)]
        [JsonRequired]
        public Guid SubjectId { get; set; }

        [FromQuery]
        public int Offset { get; set; } = 1;

        [FromQuery]
        public int Limit { get; set; } = 10;
    }
}
