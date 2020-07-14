using AzureFunctionV3Template.Services;
using FluentAssertions;
using Functions.Tests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace AzureFunctionV3Template.Tests
{
    public class MessageFunctionTests
    {
        private readonly Mock<IMessageResponderService> _messageResponderServer;
        private readonly ILogger logger = TestFactory.CreateLogger();

        public MessageFunctionTests()
        {
            _messageResponderServer = new Mock<IMessageResponderService>();

            _messageResponderServer.Setup(x => x.GetPositiveMessage()).Returns("Everything's great!").Verifiable();
            _messageResponderServer.Setup(x => x.GetNegativeMessage()).Returns("The sky is falling!").Verifiable();
            _messageResponderServer.Setup(x => x.GetSecretMessage()).ReturnsAsync("This is classified").Verifiable();
        }

        [Fact]
        public void GetPositiveMessage_Http_trigger_should_return_known_string()
        {
            // Arrange
            MessageFunction messageFunction = new MessageFunction(_messageResponderServer.Object);
            var request = TestFactory.CreateHttpRequest();

            // Act
            var response = (OkObjectResult) messageFunction.GetPositiveMessage(request, logger).Result;
            
            // Assert
            response.Value.Should().Be("Everything's great!");
            _messageResponderServer.Verify(x => x.GetPositiveMessage(), Times.Once);
            _messageResponderServer.Verify(x => x.GetNegativeMessage(), Times.Never);
            _messageResponderServer.Verify(x => x.GetSecretMessage(), Times.Never);
        }

        [Fact]
        public void GetNegativeMessage_Http_trigger_should_return_known_string()
        {
            // Arrange
            MessageFunction messageFunction = new MessageFunction(_messageResponderServer.Object);
            var request = TestFactory.CreateHttpRequest();

            // Act
            var response = (OkObjectResult) messageFunction.GetNegativeMessage(request, logger).Result;

            // Assert
            response.Value.Should().Be("The sky is falling!");
            _messageResponderServer.Verify(x => x.GetPositiveMessage(), Times.Never);
            _messageResponderServer.Verify(x => x.GetNegativeMessage(), Times.Once);
            _messageResponderServer.Verify(x => x.GetSecretMessage(), Times.Never);
        }

        [Fact]
        public void GetSecretMessage_Http_trigger_should_return_known_string()
        {
            // Arrange
            MessageFunction messageFunction = new MessageFunction(_messageResponderServer.Object);
            var request = TestFactory.CreateHttpRequest();

            // Act
            var response = (OkObjectResult)messageFunction.GetSecretMessage(request, logger).Result;

            // Assert
            response.Value.Should().Be("This is classified");
            _messageResponderServer.Verify(x => x.GetPositiveMessage(), Times.Never);
            _messageResponderServer.Verify(x => x.GetNegativeMessage(), Times.Never);
            _messageResponderServer.Verify(x => x.GetSecretMessage(), Times.Once);
        }

        [Fact]
        public void GetPositiveMessage_should_log_message()
        {
            // Arrange
            var logger = (ListLogger) TestFactory.CreateLogger(LoggerTypes.List);
            MessageFunction messageFunction = new MessageFunction(_messageResponderServer.Object);

            // Act
            messageFunction.GetPositiveMessage(null, logger);

            // Assert
            var msg = logger.Logs[0];
            msg.Should().Contain("MessageFunction - C# HTTP trigger function processed a request for GetPositiveMessage.");
        }

        [Fact]
        public void GetNegativeMessage_should_log_message()
        {
            // Arrange
            var logger = (ListLogger)TestFactory.CreateLogger(LoggerTypes.List);
            MessageFunction messageFunction = new MessageFunction(_messageResponderServer.Object);

            // Act
            messageFunction.GetNegativeMessage(null, logger);

            // Assert
            var msg = logger.Logs[0];
            msg.Should().Contain("MessageFunction - C# HTTP trigger function processed a request for GetNegativeMessage.");
        }

        [Fact]
        public void GetSecretMessage_should_log_message()
        {
            // Arrange
            var logger = (ListLogger)TestFactory.CreateLogger(LoggerTypes.List);
            MessageFunction messageFunction = new MessageFunction(_messageResponderServer.Object);

            // Act
            messageFunction.GetSecretMessage(null, logger);

            // Assert
            var msg = logger.Logs[0];
            msg.Should().Contain("MessageFunction - C# HTTP trigger function processed a request for GetSecretMessage.");
        }
    }
}
