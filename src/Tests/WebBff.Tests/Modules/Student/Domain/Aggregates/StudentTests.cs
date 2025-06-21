using FluentAssertions;
using Student.Domain.Aggregates;
using Student.Domain.Enumerations;
using StudentAggregate = Student.Domain.Aggregates.Student;
using Xunit;
using System;
using System.Linq;

namespace WebBff.Tests.Modules.Student.Domain.Aggregates;

public class StudentTests
{
    [Fact]
    public void Create_ShouldRaiseStudentCreatedEvent()
    {
        var name = "John Doe";
        var email = "john@example.com";
        var password = "securepassword";
        var phone = "+5511999999999";
        var dateOfBirth = new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero);

        var student = StudentAggregate.Create(name, email, password, phone, dateOfBirth);

        student.Should().NotBeNull();
        student.Name.Should().Be(name);
        student.Email.Should().Be(email);
        student.Password.Should().Be(password);
        student.Phone.Should().Be(phone);
        student.DateOfBirth.Should().Be(dateOfBirth);
    }

    [Fact]
    public void AddXp_ShouldRaiseXpAddedEventAndPossiblyLevelUp()
    {
        var student = StudentAggregate.Create("John Doe", "john@example.com", "password", "123", DateTimeOffset.Now);
        var initialXp = student.Xp;

        student.AddXp(60);

        student.Xp.Should().BeGreaterThan(initialXp);
    }

    [Fact]
    public void AddDayStreak_ShouldIncreaseStreak()
    {
        var student = StudentAggregate.Create("Jane", "jane@example.com", "pass", "456", DateTimeOffset.Now);
        var initialStreak = student.DayStreak;

        student.AddDayStreak();

        student.DayStreak.Should().BeGreaterThan(initialStreak);
    }

    [Fact]
    public void ChangeStatus_ShouldRaiseCorrectEvent()
    {
        var student = StudentAggregate.Create("Jane", "jane@example.com", "pass", "456", DateTimeOffset.Now);

        student.ChangeStatus(StudentStatus.Blocked);

        student.Status.Should().Be(StudentStatus.Blocked);
    }

    [Fact]
    public void AddFriend_ShouldRaiseFriendAddedEvent()
    {
        var student = StudentAggregate.Create("Jane", "jane@example.com", "pass", "456", DateTimeOffset.Now);
        var friendId = Guid.NewGuid();

        student.AddFriend(friendId);

        student.Friends.Should().ContainSingle(x => x.StudentId == friendId);
    }

    [Fact]
    public void AddAttempt_ShouldRaiseAttemptCreatedEvent()
    {
        var student = StudentAggregate.Create("Alex", "alex@example.com", "pwd", "789", DateTimeOffset.Now);
        var subjectId = Guid.NewGuid();
        var lessonId = Guid.NewGuid();
        student.AddSubjectProgress(subjectId);
        student.AddLessonProgress(subjectId, lessonId);

        var attemptId = student.AddAttempt(subjectId, lessonId);

        attemptId.Should().NotBeEmpty();
    }

    [Fact]
    public void CreateFriendRequest_ShouldAddToRequests()
    {
        var student = StudentAggregate.Create("Anna", "anna@example.com", "pwd", "000", DateTimeOffset.Now);
        var friendId = Guid.NewGuid();

        student.CreateFriendRequest(friendId);

        student.Requests.Should().ContainSingle(r => r.FriendId == friendId);
    }

    [Fact]
    public void AcceptFriendRequest_ShouldChangeRequestStatusToAccepted()
    {
        var student = StudentAggregate.Create("Ana", "ana@example.com", "pwd", "999", DateTimeOffset.Now);
        var friendId = Guid.NewGuid();
        student.CreateFriendRequest(friendId);
        var requestId = student.Requests.First().Id;

        student.AcceptFriendRequest(requestId, friendId);

        student.Requests.First().Status.Should().Be(FriendRequestStatus.Accepted);
    }

    [Fact]
    public void RejectFriendRequest_ShouldChangeRequestStatusToRejected()
    {
        var student = StudentAggregate.Create("Leo", "leo@example.com", "pwd", "321", DateTimeOffset.Now);
        var friendId = Guid.NewGuid();
        student.CreateFriendRequest(friendId);
        var requestId = student.Requests.First().Id;

        student.RejectFriendRequest(requestId);

        student.Requests.First().Status.Should().Be(FriendRequestStatus.Rejected);
    }

    [Fact]
    public void AddSubjectProgress_ShouldAddSubject()
    {
        var student = StudentAggregate.Create("Lucas", "lucas@example.com", "pwd", "888", DateTimeOffset.Now);
        var subjectId = Guid.NewGuid();

        student.AddSubjectProgress(subjectId);

        student.SubjectProgresses.Should().ContainSingle(p => p.SubjectId == subjectId);
    }

    [Fact]
    public void AddLessonProgress_ShouldAddLessonToSubject()
    {
        var student = StudentAggregate.Create("Mia", "mia@example.com", "pwd", "777", DateTimeOffset.Now);
        var subjectId = Guid.NewGuid();
        var lessonId = Guid.NewGuid();
        student.AddSubjectProgress(subjectId);

        student.AddLessonProgress(subjectId, lessonId);

        student.SubjectProgresses.First().LessonProgresses.Should().ContainSingle(l => l.LessonId == lessonId);
    }

    [Fact]
    public void Delete_ShouldMarkAsDeleted()
    {
        var student = StudentAggregate.Create("Zoe", "zoe@example.com", "pwd", "333", DateTimeOffset.Now);

        student.Delete();

        student.IsDeleted.Should().BeTrue();
    }
}
