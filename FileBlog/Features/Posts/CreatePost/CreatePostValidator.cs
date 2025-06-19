using System.Data;
using FluentValidation;

public class CreatePostValidator : AbstractValidator<CreatePostRequest>
{
    public CreatePostValidator()
    {
        RuleFor(x => x.Title)
        .NotEmpty().WithMessage("Title is required.")
        .Length(4, 50).WithMessage("Title must be between 4 and 50 characters.");

        RuleFor(x => x.Body)
        .NotEmpty().WithMessage("Body is required.")
        .MinimumLength(50).WithMessage("Body is too small, body must have at least 50 characters.");

        RuleFor(x => x.Description)
        .NotEmpty().WithMessage("A brief description is required")
        .Length(20, 200).WithMessage("Description must be between 20 and 200 characters.");

        RuleFor(x => x.Tags)
        .Must(tags => tags == null || tags.Count <= 4).WithMessage("The maximum number of tags is 4.");
        RuleForEach(x => x.Tags)
        .NotEmpty().WithMessage("Tags can't be empty")
        .Length(2, 20).WithMessage("Each tag must be between 2 and 20 characters.");

        RuleFor(x => x.Categories)
       .Must(categories => categories == null || categories.Count <= 4).WithMessage("The maximum number of categories is 4.");
        RuleForEach(x => x.Categories)
        .NotEmpty().WithMessage("Categories can't be empty");

        RuleFor(x => x.PublishingDate)
        .Must(date => date==null || date >= DateTime.UtcNow.Date).WithMessage("Publishing date must be today or a future date.");
    }
}