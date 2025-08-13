using FluentValidation.Results;
using Throttlr.Core.DTOs;

namespace Throttlr.Api.Validators;
public interface IValidatorService
{
    ValidationResult ValidateId(string id);
    ValidationResult ValidateRouteConfigDto(RouteConfigDto route);
}