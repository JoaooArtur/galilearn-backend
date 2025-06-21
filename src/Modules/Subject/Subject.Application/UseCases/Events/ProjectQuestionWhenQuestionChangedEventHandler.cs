using Core.Application.EventBus;
using MongoDB.Driver;
using Serilog;
using Subject.Domain;
using Subject.Persistence.Projections;

namespace Subject.Application.UseCases.Events
{
    public interface IProjectQuestionWhenQuestionChangedEventHandler :
        IEventHandler<DomainEvent.QuestionAdded>,
        IEventHandler<DomainEvent.AnswerOptionAdded>;

    public class ProjectQuestionWhenQuestionChangedEventHandler(
        ISubjectProjection<Projection.Question> questionProjection,
        ILogger logger) : IProjectQuestionWhenQuestionChangedEventHandler
    {

        public async Task Handle(DomainEvent.QuestionAdded @event, CancellationToken cancellationToken = default)
        {
            try
            {
                await questionProjection.ReplaceInsertAsync(new(
                    @event.QuestionId,
                    @event.LessonId,
                    @event.Text,
                    @event.Level,
                    [],
                    @event.Timestamp),
                 cancellationToken);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Falha ao criar a questão: {QuestionId}.", @event.QuestionId);

                var message = $"Falha ao criar a questão: {@event.QuestionId}.";

                throw new InvalidOperationException(message, ex);
            }
        }
        public async Task Handle(DomainEvent.AnswerOptionAdded @event, CancellationToken cancellationToken = default)
        {
            try
            {
                var collection = questionProjection.GetCollection();

                var newAnswer = new Dto.AnswerOption(
                    Id: @event.AnswerOptionId,
                    Text: @event.Text,
                    RightAnswer: @event.IsRightAnswer
                );

                var filter = Builders<Projection.Question>.Filter.Eq(x => x.Id, @event.QuestionId);

                var pushUpdate = Builders<Projection.Question>.Update.Push(x => x.Answers, newAnswer);

                await collection.UpdateOneAsync(filter, pushUpdate, new UpdateOptions { IsUpsert = true }, cancellationToken);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Falha ao adicionar a resposta:{AnswerOptionId} para a questão: {QuestionId}.", @event.AnswerOptionId, @event.QuestionId);

                var message = $"Falha ao adicionar a resposta:{@event.AnswerOptionId} para a questão: {@event.QuestionId}.";

                throw new InvalidOperationException(message, ex);
            }
        }
    }
}
