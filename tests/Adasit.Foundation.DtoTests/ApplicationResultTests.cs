using AdasIt.Foundation.Dto.Reponses;
using FluentAssertions;

namespace Foundation.DtoTests.Responses;

public class ApplicationResultTests
{
    [Fact]
    public void Success_ShouldCreateSuccessfulResult()
    {
        // Arrange
        var data = "test data";

        // Act
        var result = ApplicationResult<string>.Success(Data: data);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
        result.Data.Should().Be(data);
        result.Errors.Should().BeEmpty();
        result.Warnings.Should().BeEmpty();
        result.Information.Should().BeEmpty();
    }

    [Fact]
    public void Success_WithWarningsAndInformation_ShouldCreateSuccessfulResult()
    {
        // Arrange
        var warnings = new List<ErrorModel> { new ErrorModel(ApplicationErrorCode.New(1), "Warning") };
        var information = new List<ErrorModel> { new ErrorModel(ApplicationErrorCode.New(2), "Info") };
        var data = "test data";

        // Act
        var result = ApplicationResult<string>.Success(Warnings: warnings, Information: information, Data: data);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().Be(data);
        result.Errors.Should().BeEmpty();
        result.Warnings.Should().HaveCount(1);
        result.Information.Should().HaveCount(1);
    }

    [Fact]
    public void Failure_ShouldCreateFailedResult()
    {
        // Arrange
        var errors = new List<ErrorModel> { new ErrorModel(ApplicationErrorCode.New(1), "Error") };
        var data = "test data";

        // Act
        var result = ApplicationResult<string>.Failure(Errors: errors, Data: data);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();
        result.Data.Should().Be(data);
        result.Errors.Should().HaveCount(1);
        result.Warnings.Should().BeEmpty();
        result.Information.Should().BeEmpty();
    }

    [Fact]
    public void Failure_WithAllCollections_ShouldCreateFailedResult()
    {
        // Arrange
        var errors = new List<ErrorModel> { new ErrorModel(ApplicationErrorCode.New(1), "Error") };
        var warnings = new List<ErrorModel> { new ErrorModel(ApplicationErrorCode.New(2), "Warning") };
        var information = new List<ErrorModel> { new ErrorModel(ApplicationErrorCode.New(3), "Info") };
        var data = "test data";

        // Act
        var result = ApplicationResult<string>.Failure(Errors: errors, Warnings: warnings, Information: information, Data: data);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Data.Should().Be(data);
        result.Errors.Should().HaveCount(1);
        result.Warnings.Should().HaveCount(1);
        result.Information.Should().HaveCount(1);
    }

    [Fact]
    public void ImplicitOperator_ShouldConvertValueToSuccessResult()
    {
        // Arrange
        var data = "test data";

        // Act
        ApplicationResult<string> result = data;

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().Be(data);
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void AddError_ShouldAddErrorToResult()
    {
        // Arrange
        var result = ApplicationResult<string>.Success();
        var error = new ErrorModel(ApplicationErrorCode.New(1), "Error");

        // Act
        result.AddError(error);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain(error);
    }

    [Fact]
    public void AddErrors_ShouldAddMultipleErrorsToResult()
    {
        // Arrange
        var result = ApplicationResult<string>.Success();
        var errors = new List<ErrorModel>
        {
            new ErrorModel(ApplicationErrorCode.New(1), "Error1"),
            new ErrorModel(ApplicationErrorCode.New(2), "Error2")
        };

        // Act
        result.AddErrors(errors);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().HaveCount(2);
        result.Errors.Should().Contain(errors[0]);
        result.Errors.Should().Contain(errors[1]);
    }

    [Fact]
    public void AddWarnings_ShouldAddWarningToResult()
    {
        // Arrange
        var result = ApplicationResult<string>.Success();
        var warning = new ErrorModel(ApplicationErrorCode.New(1), "Warning");

        // Act
        result.AddWarnings(warning);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Warnings.Should().Contain(warning);
    }

    [Fact]
    public void AddInformation_ShouldAddInformationToResult()
    {
        // Arrange
        var result = ApplicationResult<string>.Success();
        var info = new ErrorModel(ApplicationErrorCode.New(1), "Info");

        // Act
        result.AddInformation(info);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Information.Should().Contain(info);
    }

    [Fact]
    public void SetData_ShouldSetDataOnResult()
    {
        // Arrange
        var result = ApplicationResult<string>.Success();
        var newData = "new data";

        // Act
        result.SetData(newData);

        // Assert
        result.Data.Should().Be(newData);
    }

    [Fact]
    public void Collections_ShouldBeReadOnly()
    {
        // Arrange
        var result = ApplicationResult<string>.Success();

        // Assert
        result.Errors.Should().BeAssignableTo<IReadOnlyCollection<ErrorModel>>();
        result.Warnings.Should().BeAssignableTo<IReadOnlyCollection<ErrorModel>>();
        result.Information.Should().BeAssignableTo<IReadOnlyCollection<ErrorModel>>();
    }

    [Fact]
    public void Success_WithNullCollections_ShouldHandleNulls()
    {
        // Act
        var result = ApplicationResult<string>.Success(Warnings: null, Information: null);

        // Assert
        result.Warnings.Should().BeEmpty();
        result.Information.Should().BeEmpty();
    }

    [Fact]
    public void Failure_WithNullCollections_ShouldHandleNulls()
    {
        // Act
        var result = ApplicationResult<string>.Failure(Errors: null, Warnings: null, Information: null);

        // Assert
        result.Errors.Should().BeEmpty();
        result.Warnings.Should().BeEmpty();
        result.Information.Should().BeEmpty();
    }
}