using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using PaymentServiceHTTP;

public class Function1Tests
{
    [Fact]
    public async Task Run_ReturnsExpectedResponse_WhenNameProvidedInQuery()
    {
        // Arrange
        var context = new DefaultHttpContext();
        var request = context.Request;
        request.QueryString = new QueryString("?name=TestUser");
        request.Body = new MemoryStream(Encoding.UTF8.GetBytes("{}"));

        var loggerMock = new Mock<ILogger>();

        // Act
        var result = await GetPayment.Run(request, loggerMock.Object);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Hello, TestUser. This HTTP triggered function executed successfully.", okResult.Value);
    }

    [Fact]
    public async Task Run_ReturnsExpectedResponse_WhenNameProvidedInBody()
    {
        // Arrange
        var context = new DefaultHttpContext();
        var request = context.Request;
        request.QueryString = new QueryString("");
        var jsonBody = "{\"name\":\"BodyUser\"}";
        request.Body = new MemoryStream(Encoding.UTF8.GetBytes(jsonBody));

        var loggerMock = new Mock<ILogger>();

        // Act
        var result = await GetPayment.Run(request, loggerMock.Object);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Hello, BodyUser. This HTTP triggered function executed successfully.", okResult.Value);
    }

    [Fact]
    public async Task Run_ReturnsDefaultResponse_WhenNameNotProvided()
    {
        // Arrange
        var context = new DefaultHttpContext();
        var request = context.Request;
        request.QueryString = new QueryString("");
        request.Body = new MemoryStream(Encoding.UTF8.GetBytes("{}"));

        var loggerMock = new Mock<ILogger>();

        // Act
        var result = await GetPayment.Run(request, loggerMock.Object);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response.", okResult.Value);
    }
}
