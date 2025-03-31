using Microsoft.AspNetCore.Mvc;
using WebBff.Endpoints.Routes;

namespace WebBff.Endpoints.Subjects.Requests
{
    /// <summary>
    /// Represents the request for get subject by subject Id.
    /// </summary>
    /// <param name="SubjectId">The SubjectId.</param>
    public class GetSubjectByIdRequest
    {
        [FromRoute(Name = SubjectRoutes.SubjectId)]
        public Guid SubjectId { get; set; }
    }
}
