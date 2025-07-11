using FluentValidation;

public class LoginValidator : AbstractValidator<LoginRequest>
{
    public LoginValidator()
    {
        RuleFor(x => x.Username)
        .NotEmpty().WithMessage("Username is required.");

        RuleFor(x => x.Password)
        .NotEmpty().WithMessage("Password is required");
    }
}