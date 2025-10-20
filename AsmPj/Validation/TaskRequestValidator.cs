using AsmPj.Models.Dtos;
using FluentValidation;

namespace AsmPj.Validation;

public class TaskRequestValidator : AbstractValidator<TaskRequest>
{
    private static readonly string[] AllowedStatuses = { "Todo", "InProgress", "Completed" };

    public TaskRequestValidator()
    {
        RuleFor(x => x.BoardId).NotEmpty();
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).MaximumLength(1000);
        RuleFor(x => x.Status).NotEmpty()
            .Must(status => AllowedStatuses.Contains(status))
            .WithMessage($"Status must be on of the following: {string.Join(", ", AllowedStatuses)}");
    }
}