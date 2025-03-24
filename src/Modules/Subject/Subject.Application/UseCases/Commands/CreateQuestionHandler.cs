using Core.Application.Messaging;
using Core.Shared.Results;
using Serilog;
using Subject.Application.Services;
using Subject.Shared.Commands;
using Subject.Shared.Response;

namespace Subject.Application.UseCases.Commands
{
    public class CreateQuestionHandler(
        ISubjectApplicationService applicationService,
        ILogger logger) : ICommandHandler<CreateQuestionCommand, IdentifierResponse>
    {
        public async Task<Result<IdentifierResponse>> Handle(CreateQuestionCommand cmd, CancellationToken cancellationToken)
        {
            var subjectResult = await applicationService.LoadAggregateAsync<Domain.Aggregates.Subject>(cmd.SubjectId, cancellationToken);

            if (subjectResult.IsFailure)
                return Result.Failure<IdentifierResponse>(subjectResult.Error);

            var subject = subjectResult.Value;

            var questionId = subject.AddQuestion(cmd.LessonId, cmd.Text, cmd.Level);

            await applicationService.AppendEventsAsync(subject, cancellationToken);

            return Result.Success<IdentifierResponse>(new(questionId));
        }
    }
}
