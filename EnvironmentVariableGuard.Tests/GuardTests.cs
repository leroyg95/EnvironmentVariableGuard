namespace EnvironmentVariableGuard.Tests;

public class GuardTests
{
    [Fact]
    public void Require_DoesNotThrowWhenAllVariablesAreSet()
    {
        using var first = new TestEnvironmentVariable("first");
        using var second = new TestEnvironmentVariable("second");

        Guard.Require(first.Name, second.Name);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" \t")]
    public void Require_ThrowsWhenVariableIsMissingOrWhitespace(string? value)
    {
        using var variable = new TestEnvironmentVariable(value);

        var exception = Assert.Throws<InvalidOperationException>(() => Guard.Require(variable.Name));

        Assert.Equal(
            $"[EnvironmentVariableGuard] Missing required environment variable(s):\n - {variable.Name}",
            exception.Message);
    }

    [Fact]
    public void Require_ReportsAllMissingVariables()
    {
        using var first = new TestEnvironmentVariable(null);
        using var second = new TestEnvironmentVariable(" ");

        var exception = Assert.Throws<InvalidOperationException>(
            () => Guard.Require(first.Name, second.Name));

        Assert.Equal(
            $"[EnvironmentVariableGuard] Missing required environment variable(s):\n - {first.Name}\n - {second.Name}",
            exception.Message);
    }

    [Fact]
    public void RequirePattern_DoesNotThrowWhenValueMatches()
    {
        using var variable = new TestEnvironmentVariable("abc-123");

        Guard.RequirePattern(variable.Name, @"^[a-z]{3}-\d{3}$");
    }

    [Fact]
    public void RequirePattern_ThrowsWithDefaultMessageWhenValueDoesNotMatch()
    {
        using var variable = new TestEnvironmentVariable("invalid");
        const string pattern = @"^\d+$";

        var exception = Assert.Throws<InvalidOperationException>(
            () => Guard.RequirePattern(variable.Name, pattern));

        Assert.Equal(
            $"[EnvironmentVariableGuard] Environment variable '{variable.Name}' does not match the expected pattern ({pattern}).",
            exception.Message);
    }

    [Fact]
    public void RequirePattern_ThrowsWithCustomMessageWhenValueDoesNotMatch()
    {
        using var variable = new TestEnvironmentVariable("invalid");
        const string customMessage = "The value must contain only digits.";

        var exception = Assert.Throws<InvalidOperationException>(
            () => Guard.RequirePattern(variable.Name, @"^\d+$", customMessage));

        Assert.Equal($"[EnvironmentVariableGuard] {customMessage}", exception.Message);
    }

    private sealed class TestEnvironmentVariable : IDisposable
    {
        private readonly string? _originalValue;

        public TestEnvironmentVariable(string? value)
        {
            Name = $"EnvironmentVariableGuard.Tests.{Guid.NewGuid():N}";
            _originalValue = Environment.GetEnvironmentVariable(Name);
            Environment.SetEnvironmentVariable(Name, value);
        }

        public string Name { get; }

        public void Dispose() => Environment.SetEnvironmentVariable(Name, _originalValue);
    }
}
