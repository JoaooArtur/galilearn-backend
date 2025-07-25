﻿using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebBff.Endpoints.Routes;

namespace WebBff.Endpoints.Students.Requests
{
    public class PagedSubjectsProgressByStudentIdRequest
    {
        [FromHeader(Name = StudentsRoutes.Token)]
        public string Token { get; set; }
        [FromRoute(Name = StudentsRoutes.StudentId)]
        [JsonRequired]
        public Guid StudentId { get; set; }

        [FromQuery]
        public int Offset { get; set; } = 1;

        [FromQuery]
        public int Limit { get; set; } = 10;
    }
}
