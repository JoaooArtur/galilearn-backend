using Core.Domain.Primitives;
using Subject.Domain.Entities;

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

    public void AddLesson(string title, string content, int index)
        => RaiseEvent<DomainEvent.LessonAdded>(version => new(Id, Guid.NewGuid(), title, content, index, DateTimeOffset.Now, version));

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

}
