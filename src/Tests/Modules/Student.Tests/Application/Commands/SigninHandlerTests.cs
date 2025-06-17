using Xunit;
using Moq;
using FluentAssertions;
using Serilog;
using System;
using System.Threading;
using System.Threading.Tasks;
using Student.Application.UseCases.Commands;
using Student.Application.Services;
using Student.Persistence.Projections;
using Student.Shared.Commands;
using Student.Shared.Response;
using Core.Shared.Results;
using System.Linq.Expressions;
using Projection = Student.Domain.Projection;
using StudentAggregate = Student.Domain.Aggregates.Student;
using Student.Application.Extensions;

namespace Student.Tests.Application.Commands;
public class SignInHandlerTests
{
    private readonly Mock<IStudentProjection<Projection.Student>> _studentProjectionMock;
    private readonly Mock<IStudentApplicationService> _applicationServiceMock;
    private readonly Mock<ITokenService> _tokenServiceMock;
    private readonly Mock<ILogger> _loggerMock;
    private readonly SignInHandler _handler;

    public SignInHandlerTests()
    {
        _studentProjectionMock = new Mock<IStudentProjection<Projection.Student>>();
        _applicationServiceMock = new Mock<IStudentApplicationService>();
        _tokenServiceMock = new Mock<ITokenService>();
        _loggerMock = new Mock<ILogger>();

        _handler = new SignInHandler(
            _studentProjectionMock.Object,
            _applicationServiceMock.Object,
            _tokenServiceMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Success_When_Credentials_Are_Correct()
    {
        // Arrange
        var command = new SignInCommand("joao@email.com", "password123");
        var studentId = Guid.NewGuid();
        var hashedPassword = command.Password.HashMD5();

        var studentProjection = new Projection.Student(
            Id: studentId,
            Name: "João",
            Phone: "+5511999999999",
            Email: command.Email,
            Status: "Active",
            Level: 1,
            Xp: 0,
            NextLevelXPNeeded: 100,
            DaysStreak: 0,
            Friends: [],
            DateOfBirth: DateTimeOffset.Parse("2000-01-01"),
            LastLessonAnswered: null,
            CreatedAt: DateTimeOffset.UtcNow
        );

        var aggregateStudent = StudentAggregate.Create(
            name: "João",
            email: command.Email,
            password: hashedPassword,
            phone: studentProjection.Phone,
            dateOfBirth: studentProjection.DateOfBirth.Date
        );

        _studentProjectionMock
            .Setup(x => x.FindAsync(It.IsAny<Expression<Func<Projection.Student, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(studentProjection);

        _applicationServiceMock
            .Setup(x => x.LoadAggregateAsync<StudentAggregate>(studentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(aggregateStudent));

        _tokenServiceMock
            .Setup(x => x.GenerateToken(aggregateStudent.Name, aggregateStudent.Id, aggregateStudent.Email, "student"))
            .Returns("fake-jwt-token");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Token.Should().Be("fake-jwt-token");

        _tokenServiceMock.Verify(x =>
            x.GenerateToken(aggregateStudent.Name, aggregateStudent.Id, aggregateStudent.Email, "student"), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Fail_When_Email_Not_Found()
    {
        // Arrange
        var command = new SignInCommand("joao@email.com", "password123");

        _studentProjectionMock
            .Setup(x => x.FindAsync(It.IsAny<Expression<Func<Projection.Student, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Projection.Student?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("SignIn");
        result.Error.Message.Should().Be("Verifique as informações de login.");

        _applicationServiceMock.Verify(x =>
            x.LoadAggregateAsync<StudentAggregate>(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_Should_Fail_When_LoadAggregate_Fails()
    {
        // Arrange
        var command = new SignInCommand("joao@email.com", "password123");
        var studentId = Guid.NewGuid();

        var studentProjection = new Projection.Student(
            Id: studentId,
            Name: "João",
            Phone: "+5511999999999",
            Email: command.Email,
            Status: "Active",
            Level: 1,
            Xp: 0,
            NextLevelXPNeeded: 100,
            DaysStreak: 0,
            Friends: [],
            DateOfBirth: DateTimeOffset.Parse("2000-01-01"),
            LastLessonAnswered: null,
            CreatedAt: DateTimeOffset.UtcNow
        );

        _studentProjectionMock
            .Setup(x => x.FindAsync(It.IsAny<Expression<Func<Projection.Student, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(studentProjection);

        _applicationServiceMock
            .Setup(x => x.LoadAggregateAsync<StudentAggregate>(studentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure<StudentAggregate>(new Core.Shared.Errors.Error("LoadError", "Failed to load aggregate")));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("SignIn");

        _tokenServiceMock.Verify(x =>
            x.GenerateToken(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<string>(), "student"), Times.Never);
    }

    [Fact]
    public async Task Handle_Should_Fail_When_Password_Is_Invalid()
    {
        // Arrange
        var command = new SignInCommand("joao@email.com", "wrongpassword");
        var studentId = Guid.NewGuid();
        var correctPassword = "correctpassword".HashMD5();

        var studentProjection = new Projection.Student(
            Id: studentId,
            Name: "João",
            Phone: "+5511999999999",
            Email: command.Email,
            Status: "Active",
            Level: 1,
            Xp: 0,
            NextLevelXPNeeded: 100,
            DaysStreak: 0,
            Friends: [],
            DateOfBirth: DateTimeOffset.Parse("2000-01-01"),
            LastLessonAnswered: null,
            CreatedAt: DateTimeOffset.UtcNow
        );

        var aggregateStudent = StudentAggregate.Create(
            name: "João",
            email: command.Email,
            password: correctPassword,
            phone: studentProjection.Phone,
            dateOfBirth: studentProjection.DateOfBirth.Date
        );

        _studentProjectionMock
            .Setup(x => x.FindAsync(It.IsAny<Expression<Func<Projection.Student, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(studentProjection);

        _applicationServiceMock
            .Setup(x => x.LoadAggregateAsync<StudentAggregate>(studentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(aggregateStudent));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("SignIn");

        _tokenServiceMock.Verify(x =>
            x.GenerateToken(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<string>(), "student"), Times.Never);
    }
}
