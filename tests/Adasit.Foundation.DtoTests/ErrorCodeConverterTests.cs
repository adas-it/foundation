using AdasIt.Foundation.Dto.Reponses;
using FluentAssertions;
using System.Text.Json;

namespace Foundation.DtoTests.Responses;

public class ErrorCodeConverterTests
{
    private readonly JsonSerializerOptions _options;

    public ErrorCodeConverterTests()
    {
        _options = new JsonSerializerOptions();
        _options.Converters.Add(new ErrorCodeConverter());
    }

    #region Read Tests

    [Fact]
    public void Read_ObjectWithValue_ShouldDeserializeCorrectly()
    {
        // Arrange
        var json = """{"value": 123}""";
        var expected = ApplicationErrorCode.New(123);

        // Act
        var result = JsonSerializer.Deserialize<ApplicationErrorCode>(json, _options);

        // Assert
        result.Should().NotBeNull();
        result!.Value.Should().Be(expected.Value);
    }

    [Fact]
    public void Read_Number_ShouldDeserializeCorrectly()
    {
        // Arrange
        var json = "456";
        var expected = ApplicationErrorCode.New(456);

        // Act
        var result = JsonSerializer.Deserialize<ApplicationErrorCode>(json, _options);

        // Assert
        result.Should().NotBeNull();
        result!.Value.Should().Be(expected.Value);
    }

    [Fact]
    public void Read_InvalidToken_ShouldThrowJsonException()
    {
        // Arrange
        var json = "\"invalid\"";

        // Act
        var act = () => JsonSerializer.Deserialize<ApplicationErrorCode>(json, _options);

        // Assert
        act.Should().Throw<JsonException>();
    }

    [Fact]
    public void Read_ObjectWithoutValueProperty_ShouldThrowJsonException()
    {
        // Arrange
        var json = """{"other": 123}""";

        // Act
        var act = () => JsonSerializer.Deserialize<ApplicationErrorCode>(json, _options);

        // Assert
        act.Should().Throw<JsonException>();
    }

    [Fact]
    public void Read_ObjectWithNonIntegerValue_ShouldThrowJsonException()
    {
        // Arrange
        var json = """{"value": "notanumber"}""";

        // Act
        var act = () => JsonSerializer.Deserialize<ApplicationErrorCode>(json, _options);

        // Assert
        act.Should().Throw<JsonException>();
    }

    #endregion

    #region Write Tests

    [Fact]
    public void Write_ShouldSerializeToNumber()
    {
        // Arrange
        var errorCode = ApplicationErrorCode.New(789);
        var expectedJson = "789";

        // Act
        var result = JsonSerializer.Serialize(errorCode, _options);

        // Assert
        result.Should().Be(expectedJson);
    }

    [Fact]
    public void Write_ZeroValue_ShouldSerializeCorrectly()
    {
        // Arrange
        var errorCode = ApplicationErrorCode.New(0);
        var expectedJson = "0";

        // Act
        var result = JsonSerializer.Serialize(errorCode, _options);

        // Assert
        result.Should().Be(expectedJson);
    }

    [Fact]
    public void Write_NegativeValue_ShouldSerializeCorrectly()
    {
        // Arrange
        var errorCode = ApplicationErrorCode.New(-1);
        var expectedJson = "-1";

        // Act
        var result = JsonSerializer.Serialize(errorCode, _options);

        // Assert
        result.Should().Be(expectedJson);
    }

    #endregion

    #region Integration Tests

    [Fact]
    public void RoundTrip_ObjectFormat_ShouldPreserveValue()
    {
        // Arrange
        var original = ApplicationErrorCode.New(999);
        var json = JsonSerializer.Serialize(original, _options);

        // Act
        var deserialized = JsonSerializer.Deserialize<ApplicationErrorCode>(json, _options);

        // Assert
        deserialized.Should().NotBeNull();
        deserialized!.Value.Should().Be(original.Value);
    }

    [Fact]
    public void RoundTrip_NumberFormat_ShouldPreserveValue()
    {
        // Arrange
        var original = ApplicationErrorCode.New(111);
        var json = JsonSerializer.Serialize(original, _options);

        // Act
        var deserialized = JsonSerializer.Deserialize<ApplicationErrorCode>(json, _options);

        // Assert
        deserialized.Should().NotBeNull();
        deserialized!.Value.Should().Be(original.Value);
    }

    #endregion
}