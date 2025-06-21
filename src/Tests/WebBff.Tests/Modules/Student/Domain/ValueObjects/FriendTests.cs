using FluentAssertions;
using Student.Domain.ValueObjects;
using Xunit;
using System;

namespace Student.UnitTests.Domain.ValueObjects;

public class FriendTests
{
    [Fact]
    public void Create_FromDto_ShouldReturnCorrectFriend()
    {
        // Arrange
        var studentId = Guid.NewGuid();
        var dto = new Student.Domain.Dto.Friend(studentId);

        // Act
        var friend = Friend.Create(dto);

        // Assert
        friend.StudentId.Should().Be(studentId);
    }

    [Fact]
    public void ImplicitConversion_ToDto_ShouldMatchStudentId()
    {
        // Arrange
        var studentId = Guid.NewGuid();
        var friend = new Friend(studentId);

        // Act
        Student.Domain.Dto.Friend dto = friend;

        // Assert
        dto.StudentId.Should().Be(studentId);
    }

    [Fact]
    public void ImplicitConversion_FromDto_ShouldMatchStudentId()
    {
        // Arrange
        var studentId = Guid.NewGuid();
        Student.Domain.Dto.Friend dto = new(studentId);

        // Act
        Friend friend = dto;

        // Assert
        friend.StudentId.Should().Be(studentId);
    }

    [Fact]
    public void OperatorEquals_ShouldReturnTrueWhenSameId()
    {
        // Arrange
        var studentId = Guid.NewGuid();
        Friend friend = new(studentId);
        Student.Domain.Dto.Friend dto = new(studentId);

        // Act & Assert
        (friend == dto).Should().BeTrue();
    }

    [Fact]
    public void OperatorNotEquals_ShouldReturnTrueWhenDifferentId()
    {
        // Arrange
        Friend friend = new(Guid.NewGuid());
        Student.Domain.Dto.Friend dto = new(Guid.NewGuid());

        // Act & Assert
        (friend != dto).Should().BeTrue();
    }

    [Fact]
    public void Undefined_ShouldHaveEmptyGuid()
    {
        // Act
        var undefined = Friend.Undefined;

        // Assert
        undefined.StudentId.Should().Be(Guid.Empty);
    }
}
