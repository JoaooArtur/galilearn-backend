using Core.Application.Messaging;
using Core.Shared.Results;
using Serilog;
using Subject.Application.Services;
using Subject.Shared.Commands;
using Subject.Shared.Response;

namespace Subject.Application.UseCases.Commands
{
    public class CreateLessionHandler(
        ISubjectApplicationService applicationService,
        ILogger logger) : ICommandHandler<CreateLessonCommand, IdentifierResponse>
    {
        public async Task<Result<IdentifierResponse>> Handle(CreateLessonCommand cmd, CancellationToken cancellationToken)
        {
            var subjectResult = await applicationService.LoadAggregateAsync<Domain.Aggregates.Subject>(cmd.SubjectId, cancellationToken);

            if (subjectResult.IsFailure)
                return Result.Failure<IdentifierResponse>(subjectResult.Error);

            var subject = subjectResult.Value;

            var lessonId = subject.AddLesson(cmd.Title, cmd.Content, cmd.Index);

            await applicationService.AppendEventsAsync(subject, cancellationToken);

            return Result.Success<IdentifierResponse>(new(lessonId));
        }
    }
}
