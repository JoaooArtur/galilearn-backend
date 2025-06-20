﻿using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebBff.Endpoints.Routes;

namespace WebBff.Endpoints.Students.Requests
{
    /// <summary>
    /// Represents the request for list lesson progressess by student Id.
    /// </summary>
    /// <param name="StudentId">The StudentId.</param>
    /// <param name="SubjectId">The SubjectId.</param>
    public class ListLessonsProgressBySubjectIdRequest
    {
        [FromHeader(Name = StudentsRoutes.Token)]
        public string Token { get; set; }
        [FromRoute(Name = StudentsRoutes.StudentId)]
        [JsonRequired]
        public Guid StudentId { get; set; }
        [FromRoute(Name = StudentsRoutes.SubjectId)]
        [JsonRequired]
        public Guid SubjectId { get; set; }
    }
}
