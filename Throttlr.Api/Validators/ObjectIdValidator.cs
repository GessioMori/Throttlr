using FluentValidation;
using MongoDB.Bson;

namespace Throttlr.Api.Validators;

public class ObjectIdValidator : AbstractValidator<string>
{
    public ObjectIdValidator()
    {
        this.RuleFor(id => id)
            .NotEmpty().WithMessage("Id is required")
            .Must(this.BeValidObjectId).WithMessage("Invalid ObjectId format");
    }

    private bool BeValidObjectId(string id) => ObjectId.TryParse(id, out _);
}