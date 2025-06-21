using Core.Application.Messaging;
using Core.Shared.Results;
using Serilog;
using Subject.Application.Services;
using Subject.Shared.Commands;
using Subject.Shared.Response;

namespace Subject.Application.UseCases.Commands
{
    public class CreateSubjectHandler(
        ISubjectApplicationService applicationService) : ICommandHandler<CreateSubjectCommand, IdentifierResponse>
    {
        public async Task<Result<IdentifierResponse>> Handle(CreateSubjectCommand cmd, CancellationToken cancellationToken)
        {
            var subject = Domain.Aggregates.Subject.Create(cmd.Name, cmd.Description, cmd.Index);

            await applicationService.AppendEventsAsync(subject, cancellationToken);

            return Result.Success<IdentifierResponse>(new(subject.Id));
        }
    }
}
