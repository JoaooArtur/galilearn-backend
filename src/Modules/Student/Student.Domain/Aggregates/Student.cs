using Core.Domain.Primitives;
using Student.Domain.Enumerations;
using Student.Domain.ValueObjects;

namespace Student.Domain.Aggregates;

public class Student : AggregateRoot
{
    public string Name { get; private set; }
    public string Phone { get; private set; }
    public string Email { get; private set; }
    public StudentStatus Status { get; private set; }
    public DateTimeOffset DateOfBirth { get; private set; }
    public List<Friend> Friends { get; private set; } = [];

    public static Student Create(string name, string email, string password, string phone, DateTimeOffset dateOfBirth)
    {
        Student Student = new();

        Student.RaiseEvent<DomainEvent.StudentCreated>(version => new(
            Guid.NewGuid(),
            email,
            name,
            phone,
            StudentStatus.PendingProfile,
            Student.DateOfBirth,
            Student.CreatedAt,
            version));

        return Student;
    }

    public void ChangeStatus(StudentStatus StudentStatus)
    {
        switch (StudentStatus)
        {
            case StudentStatus.DefaultStatus:
                RaiseEvent<DomainEvent.StudentDefaultStatus>(version => new(Id, StudentStatus.Default.Name, version));
                break;

            case StudentStatus.ActiveStatus:
                RaiseEvent<DomainEvent.StudentActiveStatus>(version => new(Id, StudentStatus.Active.Name, version));
                break;

            case StudentStatus.BlockedStatus:
                RaiseEvent<DomainEvent.StudentBlockedStatus>(version => new(Id, StudentStatus.Blocked.Name, version));
                break;
        }
    }

    public void AddFriend(Guid friendId)
        => RaiseEvent<DomainEvent.FriendAdded>(version => new(Id, friendId, version));

    public void Delete()
        => RaiseEvent<DomainEvent.StudentDeleted>(version => new(Id, version));

    protected override void ApplyEvent(IDomainEvent @event)
        => When(@event as dynamic);

    private void When(DomainEvent.StudentCreated @event)
    {
        Id = @event.StudentId;
        Name = @event.Name;
        Email = @event.Email;
        DateOfBirth = @event.DateOfBirth;
        Phone = @event.Phone;
        Status = (StudentStatus)@event.Status;
    }

    private void When(DomainEvent.StudentDeleted _)
        => IsDeleted = true;

    private void When(DomainEvent.FriendAdded @event)
        => Friends.Add(new(@event.Friend));

    private void When(DomainEvent.StudentDefaultStatus @event)
        => Status = @event.Status;

    private void When(DomainEvent.StudentActiveStatus @event)
        => Status = @event.Status;

    private void When(DomainEvent.StudentBlockedStatus @event)
        => Status = @event.Status;
}
