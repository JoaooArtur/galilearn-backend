using Microsoft.AspNetCore.Mvc;

namespace WebBff.Endpoints.Subjects.Requests
{
    /// <summary>
    /// Represents the request for list subjects.
    /// </summary>
    public class PagedSubjectsRequest
    {
        [FromQuery]
        public int Offset { get; set; } = 1;

        [FromQuery]
        public int Limit { get; set; } = 10;
    }
}
