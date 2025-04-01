using Subject.Shared.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Student.Shared.Response
{
    public sealed record SubjectsProgressByStudentIdResponse(
            PagedSubjectResponse Subject,
            SubjectLessonProgress Lessons);
    public sealed record SubjectLessonProgress(
            int FinishedLessons,
            int TotalLessons);


}
