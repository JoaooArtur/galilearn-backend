using Core.Shared.Results;
using FluentAssertions;
using Moq;
using Student.Application.UseCases.Queries;
using Student.Persistence.Projections;
using Student.Domain;
using Student.Shared.Queries;
using Student.Shared.Response;
using Xunit;

namespace Student.Tests.Application.Queries;

public class GetStudentDetailsByIdHandlerTests
{
    private readonly Mock<IStudentProjection<Projection.Student>> _projectionMock = new();
    private readonly GetStudentDetailsByIdHandler _handler;

    public GetStudentDetailsByIdHandlerTests()
    {
        _handler = new GetStudentDetailsByIdHandler(_projectionMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Return_StudentDetails_When_StudentExists()
    {
        // Arrange
        var studentId = Guid.NewGuid();
        var now = DateTimeOffset.UtcNow;

        var student = new Projection.Student(
            Id: studentId,
            Name: "Test User",
            Phone: "+5511999999999",
            Email: "user@test.com",
            Status: "Active",
            Level: 2,
            Xp: 150,
            NextLevelXPNeeded: 300,
            DaysStreak: 5,
            Friends: [Guid.NewGuid(), Guid.NewGuid()],
            DateOfBirth: now.AddYears(-20),
            LastLessonAnswered: now.AddDays(-1),
            CreatedAt: now.AddMonths(-6)
        );

        _projectionMock
            .Setup(x => x.GetAsync(studentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(student);

        var query = new GetStudentDetailByIdQuery(studentId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeOfType<StudentDetailResponse>();

        result.Value.Should().BeEquivalentTo(new StudentDetailResponse(
            student.Id,
            student.Name,
            student.Phone,
            student.Email,
            student.Status,
            student.Level,
            student.Xp,
            student.NextLevelXPNeeded,
            student.DaysStreak,
            student.Friends.Count,
            student.DateOfBirth,
            student.LastLessonAnswered,
            student.CreatedAt
        ));
    }
}
