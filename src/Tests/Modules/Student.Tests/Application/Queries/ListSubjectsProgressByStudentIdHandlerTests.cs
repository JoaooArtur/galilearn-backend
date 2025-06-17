using System.Linq.Expressions;
using Core.Shared.Results;
using FluentAssertions;
using Moq;
using Student.Application.UseCases.Queries;
using Student.Domain.Enumerations;
using Student.Persistence.Projections;
using Student.Shared.Queries;
using Student.Shared.Response;
using Subject.Shared.Queries;
using Student.Domain;
using Subject.Shared.Response;
using Xunit;
using MediatR;

namespace Student.Tests.Application.Queries;

public class ListSubjectsProgressByStudentIdHandlerTests
{
    private readonly Mock<IStudentProjection<Projection.LessonProgress>> _projectionMock = new();
    private readonly Mock<ISender> _senderMock = new();
    private readonly ListSubjectsProgressByStudentIdHandler _handler;

    public ListSubjectsProgressByStudentIdHandlerTests()
    {
        _handler = new ListSubjectsProgressByStudentIdHandler(_projectionMock.Object, _senderMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Return_Subjects_With_LessonProgress()
    {
        // Arrange
        var studentId = Guid.NewGuid();
        var subjectId = Guid.NewGuid();
        var lessonId = Guid.NewGuid();

        var subject = new PagedSubjectResponse(subjectId, "Matemática", "Descrição", 1, DateTimeOffset.Now);

        _senderMock.Setup(x => x.Send(It.IsAny<ListSubjectsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(new List<PagedSubjectResponse> { subject }));

        _senderMock.Setup(x => x.Send(It.Is<GetLessonCountBySubjectIdQuery>(q => q.SubjectId == subjectId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(new CountResponse(2))); // total: 2

        _projectionMock.Setup(x => x.ListAsync(
                It.IsAny<Expression<Func<Projection.LessonProgress, bool>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Projection.LessonProgress>
            {
                new(Guid.NewGuid(), subjectId, lessonId, studentId, LessonStatus.Finished, DateTimeOffset.UtcNow)
            }); // finished: 1

        var query = new ListSubjectsProgressByStudentIdQuery(studentId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(1);

        var progress = result.Value.First();
        progress.Subject.Should().BeEquivalentTo(subject);
        progress.Lessons.FinishedLessons.Should().Be(1);
        progress.Lessons.TotalLessons.Should().Be(2);
    }

    [Fact]
    public async Task Handle_Should_Return_Failure_When_SubjectsQuery_Fails()
    {
        // Arrange
        var error = new Core.Shared.Errors.Error("Subjects.Failure", "Erro ao buscar disciplinas");

        _senderMock.Setup(x => x.Send(It.IsAny<ListSubjectsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure<List<PagedSubjectResponse>>(error));

        var query = new ListSubjectsProgressByStudentIdQuery(Guid.NewGuid());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(error);
    }

    [Fact]
    public async Task Handle_Should_Return_Failure_When_LessonCount_Fails()
    {
        var studentId = Guid.NewGuid();
        var subjectId = Guid.NewGuid();
        var subject = new PagedSubjectResponse(subjectId, "História", "Descrição", 1, DateTimeOffset.Now);

        _senderMock.Setup(x => x.Send(It.IsAny<ListSubjectsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(new List<PagedSubjectResponse> { subject }));

        var error = new Core.Shared.Errors.Error("LessonCount.Failure", "Erro ao contar lições");

        _senderMock.Setup(x => x.Send(It.Is<GetLessonCountBySubjectIdQuery>(q => q.SubjectId == subjectId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure<CountResponse>(error));

        var query = new ListSubjectsProgressByStudentIdQuery(studentId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(error);
    }
}
