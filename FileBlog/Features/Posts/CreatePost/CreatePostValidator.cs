using FluentValidation;

public class CreatePostValidator : AbstractValidator<CreatePostRequest>
{
    public CreatePostValidator()
    {
        RuleFor(x => x.Title)
        .NotEmpty().WithMessage("Title is required.")
        .Must(title => !string.IsNullOrWhiteSpace(title)).WithMessage("Title can't be whitespace.")
        .Length(4, 50).WithMessage("Title must be between 4 and 50 characters.");

        RuleFor(x => x.Body)
        .NotEmpty().WithMessage("Body is required.")
        .MinimumLength(50).WithMessage("Body must have at least 50 characters.");

        RuleFor(x => x.Description)
        .NotEmpty().WithMessage("A brief description is required")
        .Length(20, 200).WithMessage("Description must be between 20 and 200 characters.");

        RuleFor(x => x.Slug)
        .Matches("^[a-zA-Z0-9\\s-]*$").WithMessage("Slug must contain only letters, numbers, spaces, and hyphens.")
        .MaximumLength(40).WithMessage("Slug must have at most 40 characters.")
        .When(x => !string.IsNullOrEmpty(x.Slug));

        RuleFor(x => x.Tags)
        .Must(tags => tags == null || tags.Count <= 4).WithMessage("The maximum number of tags is 4.");
        RuleForEach(x => x.Tags)
        .ChildRules(tag =>
        {
            tag.RuleFor(t => t)
            .NotEmpty().WithMessage("Tag can't be empty")
            .Length(2, 20).WithMessage("Each tag must be between 2 and 20 characters.");
        });

        RuleFor(x => x.Categories)
       .Must(categories => categories == null || categories.Count <= 4).WithMessage("The maximum number of categories is 4.");
        RuleForEach(x => x.Categories)
        .ChildRules(categories =>
        {
            categories.RuleFor(c => c)
            .NotEmpty().WithMessage("Category can't be empty")
            .Length(2, 20).WithMessage("Each category must be between 2 and 20 characters.");
        });

        RuleFor(x => x.PublishingDate)
        .Must(date => date==null || date >= DateTime.UtcNow.Date).WithMessage("Publishing date must be today or a future date.");
    }
}