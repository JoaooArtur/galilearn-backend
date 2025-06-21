using Core.Application.Messaging;
using Core.Shared.Results;
using Serilog;
using Subject.Application.Services;
using Subject.Shared.Commands;
using Subject.Shared.Response;

namespace Subject.Application.UseCases.Commands
{
    public class AddAnswerOptionHandler(
        ISubjectApplicationService applicationService) : ICommandHandler<AddAnswerOptionCommand, IdentifierResponse>
    {
        public async Task<Result<IdentifierResponse>> Handle(AddAnswerOptionCommand cmd, CancellationToken cancellationToken)
        {
            var subjectResult = await applicationService.LoadAggregateAsync<Domain.Aggregates.Subject>(cmd.SubjectId, cancellationToken);

            if (subjectResult.IsFailure)
                return Result.Failure<IdentifierResponse>(subjectResult.Error);

            var subject = subjectResult.Value;

            var answerId = subject.AddQuestionAnswerOption(cmd.LessonId,
                cmd.QuestionId,
                cmd.Text,
                cmd.IsRightAnswer);

            await applicationService.AppendEventsAsync(subject, cancellationToken);

            return Result.Success<IdentifierResponse>(new(answerId));
        }
    }
}
