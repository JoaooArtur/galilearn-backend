namespace WebBff.Endpoints.Routes
{
    internal static class StudentsRoutes
    {
        internal const string BaseUri = "v{version:apiVersion}/students";

        internal const string StudentId = "studentId";
        internal const string FriendId = "friendId";
        internal const string SubjectId = "subjectId";
        internal const string AttemptId = "attempId";
        internal const string LessonId = "lessonId";
        internal const string RequestId = "requestId";
        internal const string Token = "idToken";

        internal const string SignUp = $"{BaseUri}/sign-up";
        internal const string SignIn = $"{BaseUri}/sign-in";
        internal const string DeleteStudent = $"{BaseUri}/{{{StudentId}:guid}}";

        internal const string SendFriendRequest = $"{BaseUri}/{{{StudentId}:guid}}/friends/{{{FriendId}:guid}}";
        internal const string AcceptFriendRequest = $"{BaseUri}/{{{StudentId}:guid}}/friends/requests/{{{RequestId}:guid}}/accept";
        internal const string RejectFriendRequest = $"{BaseUri}/{{{StudentId}:guid}}/friends/requests/{{{RequestId}:guid}}/reject";

        internal const string CreateAttempt = $"{BaseUri}/{{{StudentId}:guid}}/{{{SubjectId}:guid}}/{{{LessonId}:guid}}/attempt";
        internal const string AnswerAttempt = $"{BaseUri}/{{{StudentId}:guid}}/{{{SubjectId}:guid}}/{{{LessonId}:guid}}/{{{AttemptId}:guid}}/answer";

        internal const string PagedSubjectProgressByStudentId = $"{BaseUri}/{{{StudentId}:guid}}/subjects/paged";
        internal const string ListSubjectProgressByStudentId = $"{BaseUri}/{{{StudentId}:guid}}/subjects";
        internal const string ListLessonProgressBySubjectId = $"{BaseUri}/{{{StudentId}:guid}}/{{{SubjectId}:guid}}/lessons";

        internal const string GetStudentById = $"{BaseUri}/{{{StudentId}:guid}}";
        internal const string ListStudents = $"{BaseUri}";
        internal const string ListFriendsByStudentById = $"{BaseUri}/{{{StudentId}:guid}}/friends";
        internal const string ListFriendsRequestsByStudentById = $"{BaseUri}/{{{StudentId}:guid}}/friends/requests";
    }
}
