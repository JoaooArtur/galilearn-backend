using Core.Domain.Primitives;
using DocumentFormat.OpenXml.Bibliography;
using Student.Domain.Entities;
using Student.Domain.Enumerations;
using Student.Domain.ValueObjects;

namespace Student.Domain.Aggregates;

public class Student : AggregateRoot
{
    public string Name { get; private set; }
    public string Phone { get; private set; }
    public string Email { get; private set; }
    public int Level { get; private set; } = 1;
    public int DayStreak { get; private set; } = 0;
    public int Xp { get; private set; }
    public int NextLevelXPNeeded { get; private set; } = 50;
    public StudentStatus Status { get; private set; }
    public DateTimeOffset DateOfBirth { get; private set; }
    public DateTimeOffset? LastLessonAnswered { get; private set; }
    public List<Friend> Friends { get; private set; } = [];
    public List<FriendRequest> Requests { get; private set; } = [];
    public List<SubjectProgress> SubjectProgresses { get; private set; } = [];

    public static Student Create(string name, string email, string password, string phone, DateTimeOffset dateOfBirth)
    {
        Student Student = new();

        Student.RaiseEvent<DomainEvent.StudentCreated>(version => new(
            Guid.NewGuid(),
            email,
            name,
            phone,
            StudentStatus.PendingProfile,
            dateOfBirth,
            Student.CreatedAt,
            version));

        return Student;
    }

    public void ChangeStatus(StudentStatus studentStatus)
    {
        switch (studentStatus)
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
    public void AnswerAttempt(Guid attemptId, Guid subjectId, Guid lessonId, Guid questionId, Guid answerId, bool correctAnswer)
        => RaiseEvent<DomainEvent.AttemptAnswered>(version => new(attemptId, Id, subjectId, lessonId, questionId, answerId, correctAnswer, version));

    public void ChangeSubjectStatus(Guid subjectId, SubjectStatus subjectStatus)
    {
        switch (subjectStatus)
        {
            case SubjectStatus.FinishedStatus:
                RaiseEvent<DomainEvent.SubjectProgressFinishedStatus>(version => new(subjectId, Id, SubjectStatus.Finished.Name, version));
                break;
        }
    }
    public void ChangeLessonStatus(Guid subjectId, Guid lessonId, LessonStatus lessonStatus)
    {
        switch (lessonStatus)
        {
            case LessonStatus.FinishedStatus:
                RaiseEvent<DomainEvent.LessonProgressFinishedStatus>(version => new(lessonId, subjectId, Id, LessonStatus.Finished.Name, version));
                break;
        }
    }

    public void ChangeAttemptStatus(Guid subjectId, Guid attemptId, Guid lessonId, AttemptStatus attemptStatus)
    {
        switch (attemptStatus)
        {
            case AttemptStatus.PendingStatus:
                RaiseEvent<DomainEvent.AttemptInProgressStatus>(version => new(attemptId, lessonId, subjectId, AttemptStatus.Pending.Name, version));
                break;

            case AttemptStatus.FinishedStatus:
                RaiseEvent<DomainEvent.AttemptFinishedStatus>(version => new(attemptId, lessonId, subjectId, AttemptStatus.Finished.Name, version));
                break;
        }
    }

    public void AddFriend(Guid friendId)
        => RaiseEvent<DomainEvent.FriendAdded>(version => new(Id, friendId, version));

    public void AddXp(int xpAmount)
        => RaiseEvent<DomainEvent.XpAdded>(version => new(Id, xpAmount, Xp + xpAmount >= NextLevelXPNeeded ? true : false, version));

    public void AddDayStreak()
        => RaiseEvent<DomainEvent.StreakAdded>(version => new(Id, version));

    public void CreateFriendRequest(Guid friendId)
        => RaiseEvent<DomainEvent.FriendRequestCreatedStatus>(version => new(Guid.NewGuid(), Id, friendId, version));

    public void AcceptFriendRequest(Guid requestId, Guid friendId)
        => RaiseEvent<DomainEvent.FriendRequestAcceptedStatus>(version => new(requestId, Id, friendId, FriendRequestStatus.Accepted, version));

    public void RejectFriendRequest(Guid requestId)
        => RaiseEvent<DomainEvent.FriendRequestRejectedStatus>(version => new(requestId, FriendRequestStatus.Rejected, version));

    public void AddSubjectProgress(Guid subjectId)
        => RaiseEvent<DomainEvent.SubjectProgressCreated>(version => new(Guid.NewGuid(), subjectId, Id, version));

    public void AddLessonProgress(Guid subjectId, Guid lessonId)
        => RaiseEvent<DomainEvent.LessonProgressCreated>(version => new(Guid.NewGuid(), lessonId, subjectId, Id, version));

    public Guid AddAttempt(Guid subjectId, Guid lessonId)
    {
        var attemptId = Guid.NewGuid();
        RaiseEvent<DomainEvent.AttemptCreated>(version => new(attemptId, Id, subjectId, lessonId, version));
        return attemptId;
    }

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

    private void When(DomainEvent.XpAdded @event)
    {
        Xp = (Xp + @event.XpAmount) - NextLevelXPNeeded;

        if (@event.LeveledUp)
        {
            Level = Level++;
            NextLevelXPNeeded = 100 * Level;
        }
    }
    private void When(DomainEvent.StreakAdded @event)
    {
        DayStreak = DayStreak++;
        LastLessonAnswered = DateTimeOffset.Now;
    }

    private void When(DomainEvent.StudentActiveStatus @event)
        => Status = @event.Status;

    private void When(DomainEvent.StudentBlockedStatus @event)
        => Status = @event.Status;
    private void When(DomainEvent.SubjectProgressCreated @event)
        => SubjectProgresses.Add(SubjectProgress.Create(@event.Id, @event.SubjectId));
    private void When(DomainEvent.LessonProgressCreated @event)
        => SubjectProgresses.FirstOrDefault(x => x.SubjectId == @event.SubjectId).LessonProgresses.Add(LessonProgress.Create(@event.Id, @event.LessonId));
    private void When(DomainEvent.AttemptCreated @event)
        => SubjectProgresses.FirstOrDefault(x => x.SubjectId == @event.SubjectId)
        .LessonProgresses.FirstOrDefault(x => x.LessonId == @event.LessonId)
        .Attempts.Add(Attempt.Create(@event.AttemptId, @event.LessonId));
    private void When(DomainEvent.AttemptFinishedStatus @event)
        => SubjectProgresses.FirstOrDefault(x => x.SubjectId == @event.SubjectId)
        .LessonProgresses.FirstOrDefault(x => x.Id == @event.LessonId)
        .Attempts.FirstOrDefault(x => x.Id == @event.AttemptId).ChangeStatus(@event.Status);
    private void When(DomainEvent.AttemptInProgressStatus @event)
        => SubjectProgresses.FirstOrDefault(x => x.SubjectId == @event.SubjectId)
        .LessonProgresses.FirstOrDefault(x => x.LessonId == @event.LessonId)
        .Attempts.FirstOrDefault(x => x.Id == @event.AttemptId).ChangeStatus(@event.Status);
    private void When(DomainEvent.LessonProgressFinishedStatus @event)
        => SubjectProgresses.FirstOrDefault(x => x.SubjectId == @event.SubjectId)
        .LessonProgresses.FirstOrDefault(x => x.Id == @event.LessonId).ChangeStatus(@event.Status);
    private void When(DomainEvent.SubjectProgressFinishedStatus @event)
        => SubjectProgresses.FirstOrDefault(x => x.SubjectId == @event.SubjectId).ChangeStatus(@event.Status);
    private void When(DomainEvent.AttemptAnswered @event)
        => SubjectProgresses.FirstOrDefault(x => x.SubjectId == @event.SubjectId)
        .LessonProgresses.FirstOrDefault(x => x.LessonId == @event.LessonId)
        .Attempts.FirstOrDefault(x => x.Id == @event.AttemptId).AnswerQuestion(@event.CorrectAnswer);

    #region FriendRequest
    private void When(DomainEvent.FriendRequestCreatedStatus @event)
        => Requests.Add(FriendRequest.Create(@event.RequestId, @event.FriendId));

    private void When(DomainEvent.FriendRequestAcceptedStatus @event)
        => Requests.FirstOrDefault(x => x.Id == @event.RequestId).ChangeStatus(@event.Status);

    private void When(DomainEvent.FriendRequestRejectedStatus @event)
        => Requests.FirstOrDefault(x => x.Id == @event.RequestId).ChangeStatus(@event.Status);

    #endregion
}
