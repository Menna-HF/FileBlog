using System.Security.Cryptography.X509Certificates;
using FluentValidation;

public class MediaUploadValidator : AbstractValidator<MediaUploadRequest>
{
    private long _maxFileSize = 5 * 1024 * 1024;
    private List<string> _allowedExtensions = [".jpg", ".png", ".jpeg", ".pdf", ".mp4"];
    public MediaUploadValidator()
    {
        RuleFor(x => x.Files)
        .NotEmpty().WithMessage("Files can't be empty, please upload your file.");

        RuleForEach(x => x.Files)
        .ChildRules(file =>
        {
            file.RuleFor(f => f.Length)
            .LessThanOrEqualTo(_maxFileSize).WithMessage("Each file must be at most 5MB");

            file.RuleFor(f => f.FileName)
            .Must(AllowedExtension).WithMessage("The file extension isn't allowed.");
        });

        RuleFor(x => x.Slug)
        .NotEmpty().WithMessage("Slug is required")
        .Matches("^[a-zA-Z0-9\\s-]*$").WithMessage("Slug must contain only letters, numbers, spaces, and hyphens.");
    }
    private bool AllowedExtension(string fileName)
    {
        var extension = Path.GetExtension(fileName).ToLower();
        return _allowedExtensions.Contains(extension);
    }
}