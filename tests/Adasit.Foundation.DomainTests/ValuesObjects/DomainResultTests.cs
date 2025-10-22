using Adasit.Foundation.Domain.ValuesObjects;
using FluentAssertions;

namespace Adasit.Foundation.DomainTests.ValuesObjects;

public class DomainResultTests
{
    #region Success Method Tests

    [Fact]
    public void Success_WhenCalledWithoutParameters_ShouldCreateSuccessResult()
    {
        // Act
        var result = DomainResult.Success();

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
        result.Errors.Should().BeEmpty();
        result.Warnings.Should().BeEmpty();
        result.Infos.Should().BeEmpty();
    }

    [Fact]
    public void Success_WithWarnings_ShouldCreateSuccessResultWithWarnings()
    {
        // Arrange
        var warnings = new List<Notification>
        {
            new Notification("Field1", "Warning 1", DomainErrorCode.New(2000)),
            new Notification("Field2", "Warning 2", DomainErrorCode.New(2001))
        };

        // Act
        var result = DomainResult.Success(warnings: warnings);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Warnings.Should().HaveCount(2);
        result.Warnings.Should().Contain(warnings);
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void Success_WithInfos_ShouldCreateSuccessResultWithInfos()
    {
        // Arrange
        var infos = new List<Notification>
        {
            new Notification("Info 1", DomainErrorCode.New(3000))
        };

        // Act
        var result = DomainResult.Success(infos: infos);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Infos.Should().HaveCount(1);
        result.Infos.Should().Contain(infos);
    }

    [Fact]
    public void Success_WithWarningsAndInfos_ShouldCreateSuccessResult()
    {
        // Arrange
        var warnings = new List<Notification>
        {
            new Notification("Warning", DomainErrorCode.New(2000))
        };
        var infos = new List<Notification>
        {
            new Notification("Info", DomainErrorCode.New(3000))
        };

        // Act
        var result = DomainResult.Success(warnings: warnings, infos: infos);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Warnings.Should().HaveCount(1);
        result.Infos.Should().HaveCount(1);
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void Success_WithNullWarnings_ShouldCreateSuccessResultWithEmptyWarnings()
    {
        // Act
        var result = DomainResult.Success(warnings: null);

        // Assert
        result.Warnings.Should().BeEmpty();
    }

    #endregion

    #region Failure Method Tests

    [Fact]
    public void Failure_WithErrors_ShouldCreateFailureResult()
    {
        // Arrange
        var errors = new List<Notification>
        {
            new Notification("Email", "Email is required", DomainErrorCode.New(1000))
        };

        // Act
        var result = DomainResult.Failure(errors);

        // Assert
        result.Should().NotBeNull();
        result.IsFailure.Should().BeTrue();
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
        result.Errors.Should().Contain(errors);
    }

    [Fact]
    public void Failure_WithMultipleErrors_ShouldCreateFailureResult()
    {
        // Arrange
        var errors = new List<Notification>
        {
            new Notification("Field1", "Error 1", DomainErrorCode.New(1001)),
            new Notification("Field2", "Error 2", DomainErrorCode.New(1002)),
            new Notification("Field3", "Error 3", DomainErrorCode.New(1003))
        };

        // Act
        var result = DomainResult.Failure(errors);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().HaveCount(3);
        result.Errors.Should().BeEquivalentTo(errors);
    }

    [Fact]
    public void Failure_WithErrorsAndWarnings_ShouldCreateFailureResult()
    {
        // Arrange
        var errors = new List<Notification>
        {
            new Notification("Field", "Error", DomainErrorCode.New(1000))
        };
        var warnings = new List<Notification>
        {
            new Notification("Field", "Warning", DomainErrorCode.New(2000))
        };

        // Act
        var result = DomainResult.Failure(errors, warnings);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().HaveCount(1);
        result.Warnings.Should().HaveCount(1);
    }

    [Fact]
    public void Failure_WithErrorsWarningsAndInfos_ShouldCreateFailureResult()
    {
        // Arrange
        var errors = new List<Notification>
        {
            new Notification("Field", "Error", DomainErrorCode.New(1000))
        };
        var warnings = new List<Notification>
        {
            new Notification("Warning", DomainErrorCode.New(2000))
        };
        var infos = new List<Notification>
        {
            new Notification("Info", DomainErrorCode.New(3000))
        };

        // Act
        var result = DomainResult.Failure(errors, warnings, infos);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().HaveCount(1);
        result.Warnings.Should().HaveCount(1);
        result.Infos.Should().HaveCount(1);
    }

    [Fact]
    public void Failure_WhenErrorsIsNull_ShouldThrowArgumentException()
    {
        // Act
        var act = () => DomainResult.Failure(null!);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Failure must contain at least one error notification.");
    }

    [Fact]
    public void Failure_WhenErrorsIsEmpty_ShouldThrowArgumentException()
    {
        // Arrange
        var errors = new List<Notification>();

        // Act
        var act = () => DomainResult.Failure(errors);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Failure must contain at least one error notification.");
    }

    #endregion

    #region IsSuccess and IsFailure Tests

    [Fact]
    public void IsSuccess_WhenNoErrors_ShouldReturnTrue()
    {
        // Arrange
        var result = DomainResult.Success();

        // Act & Assert
        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
    }

    [Fact]
    public void IsFailure_WhenErrorsExist_ShouldReturnTrue()
    {
        // Arrange
        var errors = new List<Notification>
        {
            new Notification("Error", DomainErrorCode.New(1000))
        };
        var result = DomainResult.Failure(errors);

        // Act & Assert
        result.IsFailure.Should().BeTrue();
        result.IsSuccess.Should().BeFalse();
    }

    #endregion

    #region Collection Immutability Tests

    [Fact]
    public void Errors_ShouldReturnReadOnlyCollection()
    {
        // Arrange
        var errors = new List<Notification>
        {
            new Notification("Error", DomainErrorCode.New(1000))
        };
        var result = DomainResult.Failure(errors);

        // Act
        var errorCollection = result.Errors;

        // Assert
        errorCollection.Should().BeAssignableTo<IReadOnlyCollection<Notification>>();
    }

    [Fact]
    public void Warnings_ShouldReturnReadOnlyCollection()
    {
        // Arrange
        var warnings = new List<Notification>
        {
            new Notification("Warning", DomainErrorCode.New(2000))
        };
        var result = DomainResult.Success(warnings: warnings);

        // Act
        var warningCollection = result.Warnings;

        // Assert
        warningCollection.Should().BeAssignableTo<IReadOnlyCollection<Notification>>();
    }

    [Fact]
    public void Infos_ShouldReturnReadOnlyCollection()
    {
        // Arrange
        var infos = new List<Notification>
        {
            new Notification("Info", DomainErrorCode.New(3000))
        };
        var result = DomainResult.Success(infos: infos);

        // Act
        var infoCollection = result.Infos;

        // Assert
        infoCollection.Should().BeAssignableTo<IReadOnlyCollection<Notification>>();
    }

    [Fact]
    public void Collections_OriginalListShouldBeAffectedByModifications()
    {
        // Arrange
        var errors = new List<Notification>
        {
            new Notification("Error", DomainErrorCode.New(1000))
        };
        var result = DomainResult.Failure(errors);

        // Act
        errors.Add(new Notification("NewError", DomainErrorCode.New(1001)));

        // Assert
        result.Errors.Should().HaveCount(2);
    }

    #endregion

    #region Integration Tests

    [Fact]
    public void DomainResult_TypicalSuccessScenario_ShouldWorkCorrectly()
    {
        // Arrange
        var warnings = new List<Notification>
        {
            new Notification("Age", "Age is close to limit", DomainErrorCode.New(2000))
        };

        // Act
        var result = DomainResult.Success(warnings: warnings);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();
        result.Warnings.Should().HaveCount(1);
        result.Warnings.First().FieldName.Should().Be("Age");
    }

    [Fact]
    public void DomainResult_TypicalFailureScenario_ShouldWorkCorrectly()
    {
        // Arrange
        var errors = new List<Notification>
        {
            new Notification("Name", "Name is required", DomainErrorCode.New(1002)),
            new Notification("Email", "Email is invalid", DomainErrorCode.New(1002))
        };
        var warnings = new List<Notification>
        {
            new Notification("Password", "Password is weak", DomainErrorCode.New(2001))
        };

        // Act
        var result = DomainResult.Failure(errors, warnings);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().HaveCount(2);
        result.Warnings.Should().HaveCount(1);
    }

    #endregion
}