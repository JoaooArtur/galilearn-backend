using Core.Application.Messaging;
using Core.Shared.Results;
using MediatR;
using Serilog;
using Student.Application.Services;
using Student.Shared.Commands;
using Student.Shared.Response;
using Subject.Shared.Commands;

namespace Student.Application.UseCases.Commands
{
    public class AnswerAttemptHandler(
        IStudentApplicationService applicationService,
        ISender sender) : ICommandHandler<AnswerAttemptCommand, CorrectAnswerResponse>
    {

        public async Task<Result<CorrectAnswerResponse>> Handle(AnswerAttemptCommand cmd, CancellationToken cancellationToken)
        {
            var studentResult = await applicationService.LoadAggregateAsync<Domain.Aggregates.Student>(cmd.StudentId, cancellationToken);

            if (studentResult.IsFailure)
                return Result.Failure<CorrectAnswerResponse>(studentResult.Error);

            var student = studentResult.Value;

            var correctAnswerResult = await sender.Send(new CheckCorrectAnswerCommand(cmd.QuestionId, cmd.AnswerId), cancellationToken);

            if(correctAnswerResult.IsFailure)
                return Result.Failure<CorrectAnswerResponse>(correctAnswerResult.Error);

            var correctAnswer = correctAnswerResult.Value;

            student.AnswerAttempt(cmd.AttemptId, cmd.SubjectId, cmd.LessonId, cmd.QuestionId, cmd.AnswerId, correctAnswer.IsCorrectAnswer);

            await applicationService.AppendEventsAsync(student, cancellationToken);

            return Result.Success<CorrectAnswerResponse>(new (cmd.AttemptId, cmd.QuestionId, cmd.AnswerId, correctAnswer.correctAnswerId, correctAnswer.IsCorrectAnswer));
        }
    }
}
