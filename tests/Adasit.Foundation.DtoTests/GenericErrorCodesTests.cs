using AdasIt.Foundation.Dto.Reponses;
using FluentAssertions;

namespace Foundation.DtoTests.Responses;

public class GenericErrorCodesTests
{
    [Fact]
    public void Generic_ShouldHaveCorrectValue()
    {
        // Assert
        GenericErrorCodes.Generic.Value.Should().Be(10_000);
    }

    [Fact]
    public void UnavailableFeatureFlag_ShouldHaveCorrectValue()
    {
        // Assert
        GenericErrorCodes.UnavailableFeatureFlag.Value.Should().Be(10_001);
    }

    [Fact]
    public void ClientHttp_ShouldHaveCorrectValue()
    {
        // Assert
        GenericErrorCodes.ClientHttp.Value.Should().Be(10_002);
    }

    [Fact]
    public void Validation_ShouldHaveCorrectValue()
    {
        // Assert
        GenericErrorCodes.Validation.Value.Should().Be(10_003);
    }

    [Fact]
    public void InvalidOperationOnPatch_ShouldHaveCorrectValue()
    {
        // Assert
        GenericErrorCodes.InvalidOperationOnPatch.Value.Should().Be(10_004);
    }

    [Fact]
    public void InvalidPathOnPatch_ShouldHaveCorrectValue()
    {
        // Assert
        GenericErrorCodes.InvalidPathOnPatch.Value.Should().Be(10_005);
    }

    [Fact]
    public void NotificationValuesError_ShouldHaveCorrectValue()
    {
        // Assert
        GenericErrorCodes.NotificationValuesError.Value.Should().Be(10_006);
    }

    [Fact]
    public void DataBaseError_ShouldHaveCorrectValue()
    {
        // Assert
        GenericErrorCodes.DataBaseError.Value.Should().Be(10_007);
    }

    [Fact]
    public void Errors_Generic_ShouldReturnCorrectErrorModel()
    {
        // Act
        var error = Errors.Generic();

        // Assert
        error.Code.Should().Be(GenericErrorCodes.Generic);
        error.Message.Should().Be("Unfortunately an error occurred during the processing.");
        error.InnerMessage.Should().BeEmpty();
    }

    [Fact]
    public void Errors_GenericDataBaseError_ShouldReturnCorrectErrorModel()
    {
        // Act
        var error = Errors.GenericDataBaseError();

        // Assert
        error.Code.Should().Be(GenericErrorCodes.DataBaseError);
        error.Message.Should().Be("Unfortunately an error occurred during the processing.");
        error.InnerMessage.Should().BeEmpty();
    }

    [Fact]
    public void Errors_UnavailableFeatureFlag_ShouldReturnCorrectErrorModel()
    {
        // Act
        var error = Errors.UnavailableFeatureFlag();

        // Assert
        error.Code.Should().Be(GenericErrorCodes.UnavailableFeatureFlag);
        error.Message.Should().Be("Unavailable FeatureFlag.");
        error.InnerMessage.Should().BeEmpty();
    }

    [Fact]
    public void Errors_ClientHttp_ShouldReturnCorrectErrorModel()
    {
        // Act
        var error = Errors.ClientHttp();

        // Assert
        error.Code.Should().Be(GenericErrorCodes.ClientHttp);
        error.Message.Should().Be("Client HTTP error.");
        error.InnerMessage.Should().BeEmpty();
    }

    [Fact]
    public void Errors_Validation_ShouldReturnCorrectErrorModel()
    {
        // Act
        var error = Errors.Validation();

        // Assert
        error.Code.Should().Be(GenericErrorCodes.Validation);
        error.Message.Should().Be("Unfortunately your request do not pass in our validation process.");
        error.InnerMessage.Should().BeEmpty();
    }

    [Fact]
    public void Errors_InvalidOperationOnPatch_ShouldReturnCorrectErrorModel()
    {
        // Act
        var error = Errors.InvalidOperationOnPatch();

        // Assert
        error.Code.Should().Be(GenericErrorCodes.InvalidOperationOnPatch);
        error.Message.Should().Be("This operation are not valid on patch.");
        error.InnerMessage.Should().BeEmpty();
    }

    [Fact]
    public void Errors_InvalidPathOnPatch_ShouldReturnCorrectErrorModel()
    {
        // Act
        var error = Errors.InvalidPathOnPatch();

        // Assert
        error.Code.Should().Be(GenericErrorCodes.InvalidPathOnPatch);
        error.Message.Should().Be("This path cannot be changed on patch.");
        error.InnerMessage.Should().BeEmpty();
    }

    [Fact]
    public void Errors_NotificationValuesError_ShouldReturnCorrectErrorModel()
    {
        // Act
        var error = Errors.NotificationValuesError();

        // Assert
        error.Code.Should().Be(GenericErrorCodes.NotificationValuesError);
        error.Message.Should().Be("Error on creating a notification.");
        error.InnerMessage.Should().BeEmpty();
    }

    [Fact]
    public void GenericErrorCodes_ShouldInheritFromApplicationErrorCode()
    {
        // Assert
        typeof(GenericErrorCodes).Should().BeAssignableTo<ApplicationErrorCode>();
    }

    [Fact]
    public void GenericErrorCodes_ShouldBeSealed()
    {
        // Assert
        typeof(GenericErrorCodes).IsSealed.Should().BeTrue();
    }
}