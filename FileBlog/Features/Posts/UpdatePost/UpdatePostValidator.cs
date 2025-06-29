using FluentValidation;

public class UpdatePostValidator : AbstractValidator<UpdatePostRequest>
{
    public UpdatePostValidator()
    {
        RuleFor(x => x.Slug)
        .Matches("^[a-zA-Z0-9\\s-]*$").WithMessage("Slug must contain only letters, numbers, spaces, and hyphens.")
        .MaximumLength(40).WithMessage("Slug must have at most 40 characters.")
        .When(x => !string.IsNullOrEmpty(x.Slug));

        RuleFor(x => x.Title)
       .Length(4, 50).WithMessage("Title must be between 4 and 50 characters.")
       .When(x => x.Title != null);

        RuleFor(x => x.Body)
        .NotEmpty().WithMessage("Body is required.")
        .MinimumLength(50).WithMessage("Body must be at least 50 characters.")
        .When(x => x.Status == PostStatus.Published);

        RuleFor(x => x.Description)
        .Length(20, 200).WithMessage("Description must be between 20 and 200 characters.")
        .When(x => x.Description != null);

        RuleFor(x => x.Tags)
        .Must(tags => tags == null || tags.Count <= 4).WithMessage("The maximum number of tags is 4.");
        RuleForEach(x => x.Tags)
        .NotEmpty().WithMessage("Tags can't be empty")
        .Length(2, 20).WithMessage("Each tag must be between 2 and 20 characters.")
        .When(x => x.Tags != null);

        RuleFor(x => x.Categories)
       .Must(categories => categories == null || categories.Count <= 4).WithMessage("The maximum number of categories is 4.");
        RuleForEach(x => x.Categories)
        .NotEmpty().WithMessage("Categories can't be empty")
        .When(x => x.Categories != null);

        RuleFor(x => x.PublishingDate)
        .Must(date => date == null || date >= DateTime.UtcNow.Date).WithMessage("Publishing date must be today or a future date.");
    }
}