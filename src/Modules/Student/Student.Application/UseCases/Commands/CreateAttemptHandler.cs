using Core.Application.Messaging;
using Core.Shared.Results;
using Serilog;
using Student.Application.Services;
using Student.Shared.Commands;
using Student.Shared.Response;

namespace Student.Application.UseCases.Commands
{
    using StudentAggreagate = Domain.Aggregates.Student;
    public class CreateAttemptHandler(
        IStudentApplicationService applicationService) : ICommandHandler<CreateAttemptCommand, IdentifierResponse>
    {

        public async Task<Result<IdentifierResponse>> Handle(CreateAttemptCommand cmd, CancellationToken cancellationToken)
        {
            var studentResult = await applicationService.LoadAggregateAsync<StudentAggreagate>(cmd.StudentId, cancellationToken);

            if (studentResult.IsFailure)
                return Result.Failure<IdentifierResponse>(studentResult.Error);

            var student = studentResult.Value;

            if (!student.SubjectProgresses.Any(x => x.SubjectId == cmd.SubjectId))
            {
                student.AddSubjectProgress(cmd.SubjectId);
                await applicationService.AppendEventsAsync(student, cancellationToken);
            }

            if (student.SubjectProgresses.FirstOrDefault(x => x.SubjectId == cmd.SubjectId)?.LessonProgresses.Any(x => x.LessonId == cmd.LessonId) == false)
            {
                student.AddLessonProgress(cmd.SubjectId, cmd.LessonId);
                await applicationService.AppendEventsAsync(student, cancellationToken);
            }

            var attemptId = student.AddAttempt(cmd.SubjectId, cmd.LessonId);
            await applicationService.AppendEventsAsync(student, cancellationToken);

            return Result.Success<IdentifierResponse>(new(attemptId));
        }
    }
}
