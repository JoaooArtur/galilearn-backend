namespace WebBff.Endpoints.Routes
{
    internal class SubjectRoutes
    {
        internal const string BaseUri = "v{version:apiVersion}/subjects";

        internal const string SubjectId = "subjectId";
        internal const string LessonId = "lessonId";
        internal const string QuestionId = "questionId";
        internal const string Token = "idToken";

        internal const string CreateSubject = $"{BaseUri}/subject";
        internal const string CreateLesson = $"{BaseUri}/{{{SubjectId}:guid}}/lesson";
        internal const string CreateQuestion = $"{BaseUri}/{{{SubjectId}:guid}}/{{{LessonId}:guid}}/question";
        internal const string AddAnswerOption = $"{BaseUri}/{{{SubjectId}:guid}}/{{{LessonId}:guid}}/{{{QuestionId}:guid}}/answer";


        internal const string GetSubjectById = $"{BaseUri}/{{{SubjectId}:guid}}";
        internal const string GetLessonById = $"{BaseUri}/{{{SubjectId}:guid}}/{{{LessonId}:guid}}";
        internal const string ListRandomQuestionsByLessonId = $"{BaseUri}/{{{SubjectId}:guid}}/{{{LessonId}:guid}}/questions/random";
        internal const string PagedSubjects = $"{BaseUri}/subject";
        internal const string PagedLessons = $"{BaseUri}/{{{SubjectId}:guid}}/lesson";
        internal const string PagedQuestions = $"{BaseUri}/{{{SubjectId}:guid}}/{{{LessonId}:guid}}/question";
    }
}
