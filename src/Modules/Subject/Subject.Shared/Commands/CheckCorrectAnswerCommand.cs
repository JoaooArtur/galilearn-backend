using Core.Application.Messaging;
using Subject.Shared.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subject.Shared.Commands
{
    public sealed record CheckCorrectAnswerCommand(Guid QuestionId, Guid AnswerId) : ICommand<CheckCorrectAnswerResponse>;
}
