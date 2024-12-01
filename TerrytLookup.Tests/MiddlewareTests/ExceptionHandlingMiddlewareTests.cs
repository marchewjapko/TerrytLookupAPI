using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using TerrytLookup.Infrastructure.ExceptionHandling;
using TerrytLookup.Infrastructure.ExceptionHandling.Exceptions;

namespace TerrytLookup.Tests.MiddlewareTests;

public class ExceptionHandlingMiddlewareTests
{
    [Test]
    public async Task ShouldReturnMappedProblemDetail_AgreementNotFoundException()
    {
        // Arrange
        var mockLogger = new Mock<ILogger<ExceptionHandlingMiddleware>>();
        var expectedException = new CountyNotFoundException(1, 1);
        var httpContext = new DefaultHttpContext();
        var problem = expectedException.GetProblemDetails(expectedException);
        var exceptionHandlingMiddleware =
            new ExceptionHandlingMiddleware(_ => Task.FromException(expectedException), mockLogger.Object);

        // Act
        await exceptionHandlingMiddleware.InvokeAsync(httpContext);

        // Assert
        Assert.That(httpContext.Response.StatusCode, Is.EqualTo(problem.Status));
    }

    [Test]
    public async Task ShouldReturnMappedProblemDetail_ClientNotFoundException()
    {
        // Arrange
        var mockLogger = new Mock<ILogger<ExceptionHandlingMiddleware>>();
        var expectedException = new DatabaseNotEmptyException();
        var httpContext = new DefaultHttpContext();
        var problem = expectedException.GetProblemDetails(expectedException);
        var exceptionHandlingMiddleware =
            new ExceptionHandlingMiddleware(_ => Task.FromException(expectedException), mockLogger.Object);

        // Act
        await exceptionHandlingMiddleware.InvokeAsync(httpContext);

        // Assert
        Assert.That(httpContext.Response.StatusCode, Is.EqualTo(problem.Status));
    }

    [Test]
    public async Task ShouldReturnMappedProblemDetail_EquipmentNotFoundException()
    {
        // Arrange
        var mockLogger = new Mock<ILogger<ExceptionHandlingMiddleware>>();
        var expectedException = new InvalidFileContentTypeExtensionException("txt");
        var httpContext = new DefaultHttpContext();
        var problem = expectedException.GetProblemDetails(expectedException);
        var exceptionHandlingMiddleware =
            new ExceptionHandlingMiddleware(_ => Task.FromException(expectedException), mockLogger.Object);

        // Act
        await exceptionHandlingMiddleware.InvokeAsync(httpContext);

        // Assert
        Assert.That(httpContext.Response.StatusCode, Is.EqualTo(problem.Status));
    }

    [Test]
    public async Task ShouldReturnMappedProblemDetail_PaymentNotFoundException()
    {
        // Arrange
        var mockLogger = new Mock<ILogger<ExceptionHandlingMiddleware>>();
        var expectedException = new StreetNotFoundException(1, 1);
        var httpContext = new DefaultHttpContext();
        var problem = expectedException.GetProblemDetails(expectedException);
        var exceptionHandlingMiddleware =
            new ExceptionHandlingMiddleware(_ => Task.FromException(expectedException), mockLogger.Object);

        // Act
        await exceptionHandlingMiddleware.InvokeAsync(httpContext);

        // Assert
        Assert.That(httpContext.Response.StatusCode, Is.EqualTo(problem.Status));
    }

    [Test]
    public async Task ShouldReturnMappedProblemDetail_UserNotFoundException()
    {
        // Arrange
        var mockLogger = new Mock<ILogger<ExceptionHandlingMiddleware>>();
        var expectedException = new TerrytParsingException(new Exception());
        var httpContext = new DefaultHttpContext();
        var problem = expectedException.GetProblemDetails(expectedException);
        var exceptionHandlingMiddleware =
            new ExceptionHandlingMiddleware(_ => Task.FromException(expectedException), mockLogger.Object);

        // Act
        await exceptionHandlingMiddleware.InvokeAsync(httpContext);

        // Assert
        Assert.That(httpContext.Response.StatusCode, Is.EqualTo(problem.Status));
    }

    [Test]
    public async Task ShouldReturnMappedProblemDetail_UserDoesNotHaveIdClaimException()
    {
        // Arrange
        var mockLogger = new Mock<ILogger<ExceptionHandlingMiddleware>>();
        var expectedException = new TownNotFoundException(1);
        var httpContext = new DefaultHttpContext();
        var problem = expectedException.GetProblemDetails(expectedException);
        var exceptionHandlingMiddleware =
            new ExceptionHandlingMiddleware(_ => Task.FromException(expectedException), mockLogger.Object);

        // Act
        await exceptionHandlingMiddleware.InvokeAsync(httpContext);

        // Assert
        Assert.That(httpContext.Response.StatusCode, Is.EqualTo(problem.Status));
    }

    [Test]
    public async Task ShouldReturnMappedProblemDetail_UserIdClaimInvalidException()
    {
        // Arrange
        var mockLogger = new Mock<ILogger<ExceptionHandlingMiddleware>>();
        var expectedException = new VoivodeshipNotFoundException(1);
        var httpContext = new DefaultHttpContext();
        var problem = expectedException.GetProblemDetails(expectedException);
        var exceptionHandlingMiddleware =
            new ExceptionHandlingMiddleware(_ => Task.FromException(expectedException), mockLogger.Object);

        // Act
        await exceptionHandlingMiddleware.InvokeAsync(httpContext);

        // Assert
        Assert.That(httpContext.Response.StatusCode, Is.EqualTo(problem.Status));
    }

    [Test]
    public async Task ShouldReturnStatus500WhenUnmappedException()
    {
        // Arrange
        var mockLogger = new Mock<ILogger<ExceptionHandlingMiddleware>>();
        var expectedException = new Exception();
        var httpContext = new DefaultHttpContext();

        var exceptionHandlingMiddleware = new ExceptionHandlingMiddleware(_ => Task.FromException(expectedException), mockLogger.Object);

        // Act
        await exceptionHandlingMiddleware.InvokeAsync(httpContext);

        // Assert
        Assert.That(httpContext.Response.StatusCode,
            Is.EqualTo(StatusCodes.Status500InternalServerError));
    }
}