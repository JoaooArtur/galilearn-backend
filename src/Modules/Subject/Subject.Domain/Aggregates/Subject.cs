using Core.Domain.Primitives;
using Subject.Domain.Entities;
using Subject.Domain.Enumerations;
using Subject.Domain.ValueObjects;

namespace Subject.Domain.Aggregates;


public class Subject : AggregateRoot
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public int Index { get; private set; }
    public List<Lesson> Lessons { get; private set; } = [];

    public static Subject Create(string name, string description, int index)
    {
        Subject subject = new();

        subject.RaiseEvent<DomainEvent.SubjectCreated>(version => new(
            Guid.NewGuid(),
            name,
            description,
            index,
            subject.CreatedAt,
            version));

        return subject;
    }

    public void Delete()
        => RaiseEvent<DomainEvent.SubjectDeleted>(version => new(Id, version));

    public Guid AddLesson(string title, string content, int index)
    {
        var lessonId = Guid.NewGuid();
        RaiseEvent<DomainEvent.LessonAdded>(version => new(Id, lessonId, title, content, index, DateTimeOffset.Now, version));
        return lessonId;
    }

    public Guid AddQuestion(Guid lessonId, string text, QuestionLevel level)
    {
        var questionId = Guid.NewGuid();
        RaiseEvent<DomainEvent.QuestionAdded>(version => new(questionId, lessonId, text, level, DateTimeOffset.Now, version));
        return questionId;
    }
    public Guid AddQuestionAnswerOption(Guid lessonId, Guid questionId, string text, bool isRightAnswer)
    {
        var answerId = Guid.NewGuid();
        RaiseEvent<DomainEvent.AnswerOptionAdded>(version => new(lessonId, questionId, answerId, text, isRightAnswer, DateTimeOffset.Now, version));
        return lessonId;
    }

    protected override void ApplyEvent(IDomainEvent @event)
        => When(@event as dynamic);

    private void When(DomainEvent.SubjectCreated @event)
    {
        Id = @event.SubjectId;
        Name = @event.Name;
        Description = @event.Description;
        Index = @event.Index;
    }

    private void When(DomainEvent.SubjectDeleted _)
        => IsDeleted = true;

    private void When(DomainEvent.LessonAdded @event)
        => Lessons.Add(Lesson.Create(@event.LessonId, @event.Title, @event.Content, @event.Index));
    private void When(DomainEvent.QuestionAdded @event)
        => Lessons?.FirstOrDefault(x => x.Id == @event.LessonId)?
        .Questions?.Add(Question.Create(@event.QuestionId, @event.Text, @event.Level));
    private void When(DomainEvent.AnswerOptionAdded @event)
        => Lessons?.FirstOrDefault(x => x.Id == @event.LessonId)?
        .Questions.FirstOrDefault(x => x.Id == @event.QuestionId)?
        .AddAnswer(@event.AnswerOptionId, @event.Text, @event.IsRightAnswer);

}
