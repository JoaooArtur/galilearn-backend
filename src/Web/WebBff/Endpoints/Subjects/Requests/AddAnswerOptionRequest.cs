﻿using Microsoft.AspNetCore.Mvc;
using WebBff.Endpoints.Routes;

namespace WebBff.Endpoints.Subjects.Requests
{
    public class AddAnswerOptionRequest
    {
        [FromRoute(Name = SubjectRoutes.SubjectId)]
        public Guid SubjectId { get; set; }

        [FromRoute(Name = SubjectRoutes.LessonId)]
        public Guid LessonId { get; set; }

        [FromRoute(Name = SubjectRoutes.QuestionId)]
        public Guid QuestionId { get; set; }
        public string Text { get; set; }
        public bool IsRightAnswer { get; set; }
    }
}
