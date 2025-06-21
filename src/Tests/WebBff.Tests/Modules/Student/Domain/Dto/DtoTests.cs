using FluentAssertions;
using Student.Domain;
using StudentDto = Student.Domain.Dto;
using System;
using Xunit;

namespace Student.UnitTests.Domain.Dto;

public class DtoTests
{
    [Fact]
    public void Friend_ShouldStoreStudentId()
    {
        // Arrange
        var studentId = Guid.NewGuid();

        // Act
        var friend = new StudentDto.Friend(studentId);

        // Assert
        friend.StudentId.Should().Be(studentId);
    }

    [Fact]
    public void Friend_Equality_ShouldBeTrueForSameId()
    {
        // Arrange
        var studentId = Guid.NewGuid();
        var friend1 = new StudentDto.Friend(studentId);
        var friend2 = new StudentDto.Friend(studentId);

        // Assert
        friend1.Should().Be(friend2);
    }

    [Fact]
    public void Email_ShouldStoreAllFields()
    {
        // Arrange
        var id = Guid.NewGuid();
        var address = "user@example.com";
        var isConfirmed = true;
        var createdAt = DateTimeOffset.UtcNow;

        // Act
        var email = new StudentDto.Email(id, address, isConfirmed, createdAt);

        // Assert
        email.Id.Should().Be(id);
        email.Address.Should().Be(address);
        email.IsConfirmed.Should().BeTrue();
        email.CreatedAt.Should().BeCloseTo(createdAt, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Email_Equality_ShouldBeTrueForSameValues()
    {
        // Arrange
        var id = Guid.NewGuid();
        var createdAt = DateTimeOffset.UtcNow;
        var email1 = new StudentDto.Email(id, "test@test.com", false, createdAt);
        var email2 = new StudentDto.Email(id, "test@test.com", false, createdAt);

        // Assert
        email1.Should().Be(email2);
    }
}
