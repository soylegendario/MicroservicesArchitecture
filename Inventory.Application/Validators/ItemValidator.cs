using FluentValidation;
using Inventory.Application.Dto;

namespace Inventory.Application.Validators;

public class ItemValidator : AbstractValidator<ItemDto>
{
    public ItemValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.ExpirationDate)
            .NotEmpty()
            .GreaterThan(DateTime.UtcNow.Date.AddDays(-1));
    }
}