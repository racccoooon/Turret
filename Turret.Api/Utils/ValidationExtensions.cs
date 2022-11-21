using System.Text.RegularExpressions;
using FluentValidation;
using FluentValidation.Validators;

namespace Turret.Api.Utils;

public static class ValidationExtensions
{
    public static IRuleBuilderOptions<T, string> Password<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        var validator = new PasswordComplexityValidator<T>();
        return ruleBuilder.SetValidator(validator);
    }
}

public class PasswordComplexityValidator<T> : PropertyValidator<T,string>
{
    private static readonly Regex Regex = CreateRegEx();
    private const string Expression = @"[\w\[\]`!@#$%\^&*()={}:;<>+'-]*";

    public override bool IsValid(ValidationContext<T> context, string? value)
    {
        if (value == null)
            return true;

        return Regex.IsMatch(value);
    }
    
    private static Regex CreateRegEx() {
        const RegexOptions options = RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture;
        return new Regex(Expression, options, TimeSpan.FromSeconds(2.0));
    }

    public override string Name => "PasswordComplexityValidator";
}