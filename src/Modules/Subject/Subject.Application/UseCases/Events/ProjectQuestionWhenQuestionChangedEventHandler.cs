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
                logger.Error(ex, $"Falha ao criar a questão: {@event.QuestionId}.");

                throw;
            }
        }
        public async Task Handle(DomainEvent.AnswerOptionAdded @event, CancellationToken cancellationToken = default)
        {
            try
            {
                var collection = questionProjection.GetCollection();

                var question = await questionProjection.GetAsync(@event.QuestionId, cancellationToken);

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
                logger.Error(ex, $"Falha ao adicionar a resposta:{@event.AnswerOptionId} para a questão: {@event.QuestionId}.");

                throw;
            }
        }
    }
}
