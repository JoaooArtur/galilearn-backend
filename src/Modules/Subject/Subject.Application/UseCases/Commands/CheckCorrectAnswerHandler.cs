﻿using Core.Application.Messaging;
using Core.Shared.Results;
using Serilog;
using Subject.Domain;
using Subject.Persistence.Projections;
using Subject.Shared.Commands;
using Subject.Shared.Response;

namespace Subject.Application.UseCases.Commands
{
    public class CheckCorrectAnswerHandler(
        ISubjectProjection<Projection.Question> questionProjectionGateway,
        ILogger logger) : ICommandHandler<CheckCorrectAnswerCommand, CheckCorrectAnswerResponse>
    {
        public async Task<Result<CheckCorrectAnswerResponse>> Handle(CheckCorrectAnswerCommand cmd, CancellationToken cancellationToken)
        {
            var question = await questionProjectionGateway.GetAsync(cmd.QuestionId, cancellationToken);

            var correctAnswer = question.Answers.FirstOrDefault(x => x.RightAnswer);

            return Result.Success<CheckCorrectAnswerResponse>(new(correctAnswer.Id, correctAnswer.Id == cmd.AnswerId));
        }
    }
}
