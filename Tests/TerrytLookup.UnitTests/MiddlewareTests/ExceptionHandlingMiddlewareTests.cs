// using Microsoft.AspNetCore.Http;
// using Microsoft.Extensions.DependencyInjection;
// using Microsoft.Extensions.Logging;
// using Moq;
// using TerrytLookup.Core.Exceptions.CustomExceptions;
//
// namespace TerrytLookup.UnitTests.MiddlewareTests;
//
// public class ExceptionHandlingMiddlewareTests
// {
//     private static readonly Mock<ILogger<ExceptionHandlingMiddleware>> MockLogger = new();
//     private static readonly DefaultHttpContext HttpContext = new();
//
//     public ExceptionHandlingMiddlewareTests()
//     {
//         var loggerFactoryMock = new Mock<ILoggerFactory>();
//
//         loggerFactoryMock
//             .Setup(x => x.CreateLogger(It.IsAny<string>()))
//             .Returns(MockLogger.Object);
//
//         var serviceCollection = new ServiceCollection()
//             .AddSingleton(loggerFactoryMock.Object)
//             .BuildServiceProvider();
//
//         HttpContext.RequestServices = serviceCollection;
//     }
//
//     [Test]
//     public async Task ShouldReturnMappedProblemDetail_AgreementNotFoundException()
//     {
//         // Arrange
//         var expectedException = new CountyNotFoundException(1, 1);
//
//         var problem = expectedException.GetProblemDetails(expectedException);
//         var exceptionHandlingMiddleware = new ExceptionHandlingMiddleware(
//             _ => Task.FromException(expectedException),
//             MockLogger.Object);
//
//         // Act
//         await exceptionHandlingMiddleware.InvokeAsync(HttpContext);
//
//         // Assert
//         Assert.That(HttpContext.Response.StatusCode, Is.EqualTo(problem.Status));
//     }
//
//     [Test]
//     public async Task ShouldReturnMappedProblemDetail_ClientNotFoundException()
//     {
//         // Arrange
//         var expectedException = new DatabaseNotEmptyException();
//         var problem = expectedException.GetProblemDetails(expectedException);
//         var exceptionHandlingMiddleware = new ExceptionHandlingMiddleware(
//             _ => Task.FromException(expectedException),
//             MockLogger.Object);
//
//         // Act
//         await exceptionHandlingMiddleware.InvokeAsync(HttpContext);
//
//         // Assert
//         Assert.That(HttpContext.Response.StatusCode, Is.EqualTo(problem.Status));
//     }
//
//     [Test]
//     public async Task ShouldReturnMappedProblemDetail_EquipmentNotFoundException()
//     {
//         // Arrange
//         var expectedException = new InvalidFileContentTypeExtensionException("txt");
//         var problem = expectedException.GetProblemDetails(expectedException);
//         var exceptionHandlingMiddleware = new ExceptionHandlingMiddleware(
//             _ => Task.FromException(expectedException),
//             MockLogger.Object);
//
//         // Act
//         await exceptionHandlingMiddleware.InvokeAsync(HttpContext);
//
//         // Assert
//         Assert.That(HttpContext.Response.StatusCode, Is.EqualTo(problem.Status));
//     }
//
//     [Test]
//     public async Task ShouldReturnMappedProblemDetail_PaymentNotFoundException()
//     {
//         // Arrange
//         var expectedException = new StreetNotFoundException(1, 1);
//         var problem = expectedException.GetProblemDetails(expectedException);
//         var exceptionHandlingMiddleware = new ExceptionHandlingMiddleware(
//             _ => Task.FromException(expectedException),
//             MockLogger.Object);
//
//         // Act
//         await exceptionHandlingMiddleware.InvokeAsync(HttpContext);
//
//         // Assert
//         Assert.That(HttpContext.Response.StatusCode, Is.EqualTo(problem.Status));
//     }
//
//     [Test]
//     public async Task ShouldReturnMappedProblemDetail_UserNotFoundException()
//     {
//         // Arrange
//         var expectedException = new TerrytParsingException(new Exception());
//         var problem = expectedException.GetProblemDetails(expectedException);
//         var exceptionHandlingMiddleware = new ExceptionHandlingMiddleware(
//             _ => Task.FromException(expectedException),
//             MockLogger.Object);
//
//         // Act
//         await exceptionHandlingMiddleware.InvokeAsync(HttpContext);
//
//         // Assert
//         Assert.That(HttpContext.Response.StatusCode, Is.EqualTo(problem.Status));
//     }
//
//     [Test]
//     public async Task ShouldReturnMappedProblemDetail_UserDoesNotHaveIdClaimException()
//     {
//         // Arrange
//         var expectedException = new TownNotFoundException(1);
//         var problem = expectedException.GetProblemDetails(expectedException);
//         var exceptionHandlingMiddleware = new ExceptionHandlingMiddleware(
//             _ => Task.FromException(expectedException),
//             MockLogger.Object);
//
//         // Act
//         await exceptionHandlingMiddleware.InvokeAsync(HttpContext);
//
//         // Assert
//         Assert.That(HttpContext.Response.StatusCode, Is.EqualTo(problem.Status));
//     }
//
//     [Test]
//     public async Task ShouldReturnMappedProblemDetail_UserIdClaimInvalidException()
//     {
//         // Arrange
//         var expectedException = new VoivodeshipNotFoundException(1);
//         var problem = expectedException.GetProblemDetails(expectedException);
//         var exceptionHandlingMiddleware = new ExceptionHandlingMiddleware(
//             _ => Task.FromException(expectedException),
//             MockLogger.Object);
//
//         // Act
//         await exceptionHandlingMiddleware.InvokeAsync(HttpContext);
//
//         // Assert
//         Assert.That(HttpContext.Response.StatusCode, Is.EqualTo(problem.Status));
//     }
//
//     [Test]
//     public async Task ShouldReturnMappedProblemDetail_InvalidDatabaseConfigurationException()
//     {
//         // Arrange
//         var expectedException = new InvalidDatabaseConfigurationException("");
//         var problem = expectedException.GetProblemDetails(expectedException);
//         var exceptionHandlingMiddleware = new ExceptionHandlingMiddleware(
//             _ => Task.FromException(expectedException),
//             MockLogger.Object);
//
//         // Act
//         await exceptionHandlingMiddleware.InvokeAsync(HttpContext);
//
//         // Assert
//         Assert.That(HttpContext.Response.StatusCode, Is.EqualTo(problem.Status));
//     }
//
//     [Test]
//     public async Task ShouldReturnStatus500WhenUnmappedException()
//     {
//         // Arrange
//         var expectedException = new Exception();
//
//         var exceptionHandlingMiddleware = new ExceptionHandlingMiddleware(
//             _ => Task.FromException(expectedException),
//             MockLogger.Object);
//
//         // Act
//         await exceptionHandlingMiddleware.InvokeAsync(HttpContext);
//
//         // Assert
//         Assert.That(HttpContext.Response.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
//     }
// }

