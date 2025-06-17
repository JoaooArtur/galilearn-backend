using Moq;
using FluentAssertions;
using Serilog;
using Student.Application.UseCases.Commands;
using Student.Application.Services;
using Student.Shared.Commands;
using Student.Persistence.Projections;
using System.Linq.Expressions;
using Projection = Student.Domain.Projection;

namespace Student.Tests.Application.Commands;
public class SignUpHandlerTests
{
    private readonly Mock<IStudentApplicationService> _applicationServiceMock;
    private readonly Mock<IStudentProjection<Projection.Student>> _studentProjectionMock;
    private readonly Mock<ILogger> _loggerMock;
    private readonly SignUpHandler _handler;

    public SignUpHandlerTests()
    {
        _applicationServiceMock = new Mock<IStudentApplicationService>();
        _studentProjectionMock = new Mock<IStudentProjection<Projection.Student>>();
        _loggerMock = new Mock<ILogger>();

        _handler = new SignUpHandler(
            _applicationServiceMock.Object,
            _studentProjectionMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Success_When_Email_Not_Exists()
    {
        // Arrange
        var command = new SignUpCommand(
            Name: "João",
            Email: "joao@email.com",
            Password: "password123",
            Phone: "+5511999999999",
            DateOfBirth: DateTime.Parse("2000-01-01")
        );

        _studentProjectionMock
            .Setup(x => x.FindAsync(It.IsAny<Expression<Func<Projection.Student, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Projection.Student?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Id.Should().NotBeEmpty();

        _applicationServiceMock.Verify(x =>
            x.AppendEventsAsync(It.IsAny<Student.Domain.Aggregates.Student>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Fail_When_Email_AlreadyExists()
    {
        // Arrange
        var command = new SignUpCommand(
            Name: "Maria",
            Email: "maria@email.com",
            Password: "password123",
            Phone: "+5511988888888",
            DateOfBirth: DateTime.Parse("1999-05-05")
        );

        var existingStudent = new Projection.Student(
            Id: Guid.NewGuid(),
            Name: "Maria",
            Phone: "+5511988888888",
            Email: command.Email,
            Status: "Active",
            Level: 1,
            Xp: 0,
            NextLevelXPNeeded: 100,
            DaysStreak: 0,
            Friends: [],
            DateOfBirth: DateTimeOffset.Parse("1999-05-05"),
            LastLessonAnswered: null,
            CreatedAt: DateTimeOffset.UtcNow
        );

        _studentProjectionMock
            .Setup(x => x.FindAsync(It.IsAny<Expression<Func<Projection.Student, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingStudent);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("EmailAlreadyExists");
        result.Error.Message.Should().Be("O Email informado já esta sendo utilizado.");

        _applicationServiceMock.Verify(x =>
            x.AppendEventsAsync(It.IsAny<Student.Domain.Aggregates.Student>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }
}