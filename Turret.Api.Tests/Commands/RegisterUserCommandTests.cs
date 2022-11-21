using FluentAssertions;
using FluentValidation;
using MediatR;
using Moq;
using Turret.Api.Commands;
using Turret.Api.Services;

namespace Turret.Api.Tests.Commands;

public class RegisterUserCommandTests : DbTest
{
    protected RegisterUserCommandTests(DbFixture databaseFixture) : base(databaseFixture)
    {
        
    }

    [Fact]
    public async Task Handle_InvalidEmail_ThrowsValidationError()
    {
        // arrange
        var securityService = new Mock<ISecurityService>();
        var command = new RegisterUserCommand("Test Name", "invalid email", "Password1!");
        var handler = new RegisterUserCommandHandler(DbContextFactory, securityService.Object);

        // act
        var action = async () => await handler.Handle(command);

        // assert
        await action.Should().ThrowAsync<ValidationException>();

    }
}