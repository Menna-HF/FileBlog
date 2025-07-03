using FluentValidation;
public class RegisterUserValidator : AbstractValidator<RegisterUserRequest>
{
    public RegisterUserValidator()
    {
        RuleFor(x => x.Username)
        .NotEmpty().WithMessage("Username is required.")
        .MinimumLength(8).WithMessage("Username must be at least 8 characters long.")
        .Must(IsUsernameValid).WithMessage("Username must only contain letters, digits and underscore.");

        RuleFor(x => x.Password)
        .NotEmpty().WithMessage("Password is required.")
        .MinimumLength(8).WithMessage("Password must be at least 8 characters long.");

        RuleFor(x => x.Email)
        .NotEmpty().WithMessage("Email is required")
        .EmailAddress().WithMessage("Invalid email address");

        // RuleFor(x => x.Role)
        // .Must(role => role != UserRole.Unidentified).WithMessage("Please choose your role");
    }
    private bool IsUsernameValid(string username)
    {
        foreach (var character in username)
        {
            if (!char.IsLetterOrDigit(character) && character != '_')
                return false;
        }
        return true;
    }
}