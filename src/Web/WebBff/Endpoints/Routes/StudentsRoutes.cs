namespace WebBff.Endpoints.Routes
{
    internal class StudentsRoutes
    {
        internal const string BaseUri = "v{version:apiVersion}/students";

        internal const string StudentId = "studentId";
        internal const string SubjectId = "subjectId";
        internal const string AttemptId = "attempId";
        internal const string LessonId = "lessonId";
        internal const string Token = "idToken";

        internal const string SignUp = $"{BaseUri}/sign-up";
        internal const string CreateAttempt = $"{BaseUri}/{{{StudentId}:guid}}/{{{SubjectId}:guid}}/{{{LessonId}:guid}}/attempt";
        internal const string AnswerAttempt = $"{BaseUri}/{{{StudentId}:guid}}/{{{SubjectId}:guid}}/{{{LessonId}:guid}}/{{{AttemptId}:guid}}/answer";
        internal const string DeleteStudent = $"{BaseUri}/{{{StudentId}:guid}}";
        internal const string AddFriend = $"{BaseUri}/friends/{{{StudentId}:guid}}";

        internal const string GetStudentById = $"{BaseUri}";
    }
}
