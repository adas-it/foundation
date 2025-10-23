using AdasIt.Foundation.Dto.Reponses;
using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace Foundation.DtoTests.Responses;

public class DefaultResponseTests
{
    [Fact]
    public void DefaultConstructor_ShouldInitializeProperties()
    {
        // Act
        var response = new DefaultResponse<string>();

        // Assert
        response.Data.Should().BeNull();
        response.TraceId.Should().BeEmpty();
        response.Errors.Should().BeEmpty();
        response.Warnings.Should().BeEmpty();
        response.Information.Should().BeEmpty();
    }

    [Fact]
    public void Constructor_WithData_ShouldSetData()
    {
        // Arrange
        var data = "test data";

        // Act
        var response = new DefaultResponse<string>(data);

        // Assert
        response.Data.Should().Be(data);
        response.TraceId.Should().BeEmpty();
        response.Errors.Should().BeEmpty();
        response.Warnings.Should().BeEmpty();
        response.Information.Should().BeEmpty();
    }

    [Fact]
    public void Constructor_WithDataErrorsAndTraceId_ShouldSetProperties()
    {
        // Arrange
        var data = "test data";
        var errors = new List<ErrorModel>
        {
            new ErrorModel(ApplicationErrorCode.New(1), "Error1"),
            new ErrorModel(ApplicationErrorCode.New(2), "Error2")
        };
        var traceId = "trace-123";

        // Act
        var response = new DefaultResponse<string>(data, errors, traceId);

        // Assert
        response.Data.Should().Be(data);
        response.TraceId.Should().Be(traceId);
        response.Errors.Should().HaveCount(2);
        response.Errors.Should().Contain(errors[0]);
        response.Errors.Should().Contain(errors[1]);
        response.Warnings.Should().BeEmpty();
        response.Information.Should().BeEmpty();
    }

    [Fact]
    public void Constructor_WithDataSingleErrorAndTraceId_ShouldSetProperties()
    {
        // Arrange
        var data = "test data";
        var error = new ErrorModel(ApplicationErrorCode.New(1), "Error");
        var traceId = "trace-123";

        // Act
        var response = new DefaultResponse<string>(data, error, traceId);

        // Assert
        response.Data.Should().Be(data);
        response.TraceId.Should().Be(traceId);
        response.Errors.Should().HaveCount(1);
        response.Errors.Should().Contain(error);
        response.Warnings.Should().BeEmpty();
        response.Information.Should().BeEmpty();
    }

    [Fact]
    public void Properties_ShouldBeSettable()
    {
        // Arrange
        var response = new DefaultResponse<string>();
        var data = "new data";
        var traceId = "new trace";
        var error = new ErrorModel(ApplicationErrorCode.New(1), "Error");
        var warning = new ErrorModel(ApplicationErrorCode.New(2), "Warning");
        var info = new ErrorModel(ApplicationErrorCode.New(3), "Info");

        // Act
        response.Data = data;
        response.TraceId = traceId;
        response.Errors.Add(error);
        response.Warnings.Add(warning);
        response.Information.Add(info);

        // Assert
        response.Data.Should().Be(data);
        response.TraceId.Should().Be(traceId);
        response.Errors.Should().Contain(error);
        response.Warnings.Should().Contain(warning);
        response.Information.Should().Contain(info);
    }

    [Fact]
    public void Constructor_WithNullData_ShouldSetDataToNull()
    {
        // Act
        var response = new DefaultResponse<string>(null);

        // Assert
        response.Data.Should().BeNull();
    }

    [Fact]
    public void Constructor_WithEmptyErrorsList_ShouldHaveEmptyErrors()
    {
        // Arrange
        var data = "test data";
        var errors = new List<ErrorModel>();
        var traceId = "trace-123";

        // Act
        var response = new DefaultResponse<string>(data, errors, traceId);

        // Assert
        response.Errors.Should().BeEmpty();
    }
}