using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
        [JsonRequired]
        public Guid SubjectId { get; set; }
    }
}
