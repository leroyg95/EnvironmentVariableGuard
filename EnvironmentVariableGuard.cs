using System.Text.RegularExpressions;

namespace EnvironmentVariableGuard;

public static class Guard
{
    /// <summary>
    /// Ensures that the specified environment variables exist and are not null, empty, or whitespace.
    /// Throws an <see cref="InvalidOperationException"/> if any variables are missing.
    /// </summary>
    /// <param name="variableNames">The names of the required environment variables.</param>
    public static void Require(params string[] variableNames)
    {
        var missingVariables = new List<string>();

        foreach (var name in variableNames)
        {
            var value = Environment.GetEnvironmentVariable(name);
            if (string.IsNullOrWhiteSpace(value))
            {
                missingVariables.Add(name);
            }
        }

        if (missingVariables.Count > 0)
        {
            throw new InvalidOperationException(
                $"[EnvGuard] Missing required environment variable(s):\n - {string.Join("\n - ", missingVariables)}"
            );
        }
    }

    /// <summary>
    /// Ensures that an environment variable exists and matches the provided Regex pattern.
    /// </summary>
    /// <param name="variableName">The name of the environment variable.</param>
    /// <param name="regexPattern">The regular expression pattern to validate against.</param>
    /// <param name="customErrorMessage">Optional custom error message to show if validation fails.</param>
    public static void RequirePattern(string variableName, string regexPattern, string? customErrorMessage = null)
    {
        Require(variableName);

        var value = Environment.GetEnvironmentVariable(variableName);
        if (value != null && !Regex.IsMatch(value, regexPattern))
        {
            var message = customErrorMessage ?? $"Environment variable '{variableName}' does not match the expected pattern ({regexPattern}).";
            throw new InvalidOperationException($"[EnvGuard] {message}");
        }
    }
}