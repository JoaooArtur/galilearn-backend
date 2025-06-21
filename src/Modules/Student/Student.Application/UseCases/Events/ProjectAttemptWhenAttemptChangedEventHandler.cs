using Amazon.S3;
using Core.Application.EventBus;
using Core.Domain.Primitives;
using MediatR;
using MongoDB.Driver;
using Serilog;
using Student.Application.Services;
using Student.Domain;
using Student.Domain.Enumerations;
using Student.Persistence.Projections;

namespace Student.Application.UseCases.Events
{
    using StudentAggregate = Student.Domain.Aggregates.Student;
    public interface IProjectAttemptWhenAttemptChangedEventHandler :
        IEventHandler<DomainEvent.AttemptCreated>,
        IEventHandler<DomainEvent.AttemptInProgressStatus>,
        IEventHandler<DomainEvent.AttemptFinishedStatus>,
        IEventHandler<DomainEvent.AttemptAnswered>;

    public class ProjectAttemptWhenAttemptChangedEventHandler(
        IStudentProjection<Projection.Attempt> attemptProjection,
        IStudentProjection<Projection.LessonProgress> lessonProjection,
        IStudentApplicationService applicationService,
        ILogger logger) : IProjectAttemptWhenAttemptChangedEventHandler
    {

        public async Task Handle(DomainEvent.AttemptCreated @event, CancellationToken cancellationToken = default)
        {
            try
            {
                await attemptProjection.ReplaceInsertAsync(new(
                    @event.AttemptId,
                    @event.StudentId,
                    @event.LessonId,
                    0,
                    0,
                    AttemptStatus.Pending,
                    @event.Timestamp),
                 cancellationToken);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Falha ao criar a tentativa: {AttemptId} para a lição: {LessonId}", @event.AttemptId, @event.LessonId);

                var message = $"Falha ao criar a tentativa: {@event.AttemptId} para a lição: {@event.LessonId}";

                throw new ApplicationException(message, ex);
            }
        }

        public async Task Handle(DomainEvent.AttemptAnswered @event, CancellationToken cancellationToken = default)
        {
            try
            {
                var collection = attemptProjection.GetCollection();

                var attemptDb = await attemptProjection.GetAsync(@event.AttemptId, cancellationToken);

                var lesson = await lessonProjection.FindAsync(x => x.StudentId == @event.StudentId && x.LessonId == attemptDb.LessonId, cancellationToken);

                UpdateDefinition<Projection.Attempt> update;

                if (@event.CorrectAnswer)
                    update = Builders<Projection.Attempt>.Update
                        .Set(attempt => attempt.QuestionsAnswered, attemptDb.QuestionsAnswered + 1)
                        .Set(attempt => attempt.CorrectAnswers, attemptDb.CorrectAnswers + 1);
                else
                    update = Builders<Projection.Attempt>.Update
                        .Set(attempt => attempt.QuestionsAnswered, attemptDb.QuestionsAnswered + 1);

                await collection.UpdateOneAsync(
                    filter: attempt => attempt.Id == @event.AttemptId,
                    update: update,
                    cancellationToken: cancellationToken);

                if (attemptDb.QuestionsAnswered + 1 >= 5) 
                {
                    var studentResult = await applicationService.LoadAggregateAsync<StudentAggregate>(@event.StudentId, cancellationToken);

                    if (studentResult.IsFailure)
                        throw new Exception(studentResult.Error);
                    var student = studentResult.Value;

                    student.ChangeAttemptStatus(lesson.SubjectId, @event.AttemptId, lesson.Id, AttemptStatus.Finished);

                    await applicationService.AppendEventsAsync(student, cancellationToken);
                }

            }
            catch (Exception ex)
            {
                logger.Error(ex, "Falha ao atualizar a tentativa: {AttemptId} para a questão: {QuestionId}.", @event.AttemptId, @event.QuestionId);

                var message = $"Falha ao atualizar a tentativa: {@event.AttemptId} para a questão: {@event.QuestionId}.";

                throw new ApplicationException(message, ex);
            }
        }
        public async Task Handle(DomainEvent.AttemptInProgressStatus @event, CancellationToken cancellationToken = default)
        {
            try
            {
                await attemptProjection.UpdateOneFieldAsync(
                    id: @event.AttemptId,
                    field: attempt => attempt.Status,
                    value: @event.Status,
                    cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Falha ao atualizar o status da tentativa: {AttemptId}", @event.AttemptId);

                var message = $"Falha ao atualizar o status da tentativa: {@event.AttemptId}";

                throw new ApplicationException(message, ex);
            }
        }
        public async Task Handle(DomainEvent.AttemptFinishedStatus @event, CancellationToken cancellationToken = default)
        {
            try
            {
                var attempt = await attemptProjection.GetAsync(@event.AttemptId, cancellationToken);

                await attemptProjection.UpdateOneFieldAsync(
                    id: @event.AttemptId,
                    field: attempt => attempt.Status,
                    value: @event.Status,
                    cancellationToken: cancellationToken);

                if (attempt.CorrectAnswers >= 5)
                {
                    var studentResult = await applicationService.LoadAggregateAsync<StudentAggregate>(attempt.StudentId, cancellationToken);

                    if (studentResult.IsFailure)
                        throw new ApplicationException(studentResult.Error);
                    var student = studentResult.Value;

                    student.ChangeLessonStatus(@event.SubjectId, @event.LessonId, LessonStatus.Finished);

                    await applicationService.AppendEventsAsync(student, cancellationToken);

                    if (student.LastLessonAnswered?.Date != DateTimeOffset.Now.Date)
                    {
                        student.AddDayStreak();

                        await applicationService.AppendEventsAsync(student, cancellationToken);
                    }
                }

            }
            catch (Exception ex)
            {
                logger.Error(ex, "Falha ao atualizar o status da tentativa: {AttemptId}", @event.AttemptId);

                var message = $"Falha ao atualizar o status da tentativa: {@event.AttemptId}";

                throw new ApplicationException(message, ex);
            }
        }
    }
}
