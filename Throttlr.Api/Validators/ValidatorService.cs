using FluentValidation;
using FluentValidation.Results;
using Throttlr.Core.DTOs;

namespace Throttlr.Api.Validators;

public class ValidatorService : IValidatorService
{
    private readonly IValidator<string> idValidator;
    private readonly IValidator<RouteConfigDto> routeConfigDtoValidator;

    public ValidatorService([FromKeyedServices("IdValidator")] IValidator<string> idValidator,
        IValidator<RouteConfigDto> routeConfigDtoValidator)
    {
        this.idValidator = idValidator;
        this.routeConfigDtoValidator = routeConfigDtoValidator;
    }
    public ValidationResult ValidateId(string id)
    {
        return this.idValidator.Validate(id);
    }

    public ValidationResult ValidateRouteConfigDto(RouteConfigDto route)
    {
        return this.routeConfigDtoValidator.Validate(route);
    }
}