using System.Linq.Expressions;
using Core.Application.Pagination;
using Core.Shared.Results;
using FluentAssertions;
using Moq;
using Student.Application.UseCases.Queries;
using Student.Domain.Enumerations;
using Student.Persistence.Projections;
using Student.Shared.Queries;
using Student.Domain;
using Student.Shared.Response;
using Subject.Shared.Queries;
using Subject.Shared.Response;
using Xunit;
using MediatR;

namespace WebBff.Tests.Modules.Student.Application.Queries;

public class PagedSubjectsProgressByStudentIdHandlerTests
{
    private readonly Mock<IStudentProjection<Projection.LessonProgress>> _projectionMock = new();
    private readonly Mock<ISender> _senderMock = new();
    private readonly PagedSubjectsProgressByStudentIdHandler _handler;

    public PagedSubjectsProgressByStudentIdHandlerTests()
    {
        _handler = new PagedSubjectsProgressByStudentIdHandler(_projectionMock.Object, _senderMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Return_Paged_Subjects_Progress()
    {
        // Arrange
        var studentId = Guid.NewGuid();
        var subjectId = Guid.NewGuid();
        var lessonId = Guid.NewGuid();

        var subject = new PagedSubjectResponse(subjectId, "Português", "Gramática", 1, DateTimeOffset.Now);

        var paging = new Core.Domain.Primitives.Paging();
        var query = new PagedSubjectProgressByStudentIdQuery(studentId, paging);

        _senderMock.Setup(x => x.Send(It.IsAny<ListSubjectsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(new List<PagedSubjectResponse> { subject }));

        _senderMock.Setup(x => x.Send(It.Is<GetLessonCountBySubjectIdQuery>(q => q.SubjectId == subjectId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(new CountResponse(2))); // total: 2

        _projectionMock.Setup(x => x.ListAsync(It.IsAny<Expression<Func<Projection.LessonProgress, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Projection.LessonProgress>
            {
                new(Guid.NewGuid(),subjectId, lessonId ,studentId, LessonStatus.Finished, DateTimeOffset.UtcNow)
            }); // finished: 1

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Items.Should().HaveCount(1);
        result.Value.Items.First().Lessons.FinishedLessons.Should().Be(1);
        result.Value.Items.First().Lessons.TotalLessons.Should().Be(2);
        result.Value.Items.First().Subject.Should().BeEquivalentTo(subject);
    }

    [Fact]
    public async Task Handle_Should_Return_Failure_When_SubjectsQuery_Fails()
    {
        var error = new Core.Shared.Errors.Error("Subjects.Failure", "Erro ao buscar disciplinas");

        _senderMock.Setup(x => x.Send(It.IsAny<ListSubjectsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure<List<PagedSubjectResponse>>(error));

        var query = new PagedSubjectProgressByStudentIdQuery(Guid.NewGuid(), new Core.Domain.Primitives.Paging());

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(error);
    }

    [Fact]
    public async Task Handle_Should_Return_Failure_When_LessonCount_Fails()
    {
        var studentId = Guid.NewGuid();
        var subjectId = Guid.NewGuid();
        var subject = new PagedSubjectResponse(subjectId, "História", "História Geral", 1, DateTimeOffset.Now);

        _senderMock.Setup(x => x.Send(It.IsAny<ListSubjectsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(new List<PagedSubjectResponse> { subject }));

        var error = new Core.Shared.Errors.Error("Lessons.Failure", "Erro ao contar lições");

        _senderMock.Setup(x => x.Send(It.Is<GetLessonCountBySubjectIdQuery>(q => q.SubjectId == subjectId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure<CountResponse>(error));

        var query = new PagedSubjectProgressByStudentIdQuery(studentId, new Core.Domain.Primitives.Paging(1, 10));

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(error);
    }
}
