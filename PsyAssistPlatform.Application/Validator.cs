using System.Text.RegularExpressions;

namespace PsyAssistPlatform.Application;

public static class Validator
{
    public static bool EmailValidator(string email)
    {
        const string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        return Regex.IsMatch(email, pattern);
    }
}