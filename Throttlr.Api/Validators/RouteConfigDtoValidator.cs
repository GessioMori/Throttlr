using FluentValidation;
using Throttlr.Core.DTOs;

namespace Throttlr.Api.Validators;

public class RouteConfigDtoValidator : AbstractValidator<RouteConfigDto>
{
    private static readonly string[] allowedVerbs = ["GET", "POST", "PUT", "DELETE", "PATCH"];

    public RouteConfigDtoValidator()
    {
        this.RuleFor(route => route.Path)
            .NotEmpty().WithMessage("Path is required.")
            .MaximumLength(200).WithMessage("Path cannot exceed 200 characters.");

        this.RuleFor(route => route.UpstreamUrl)
            .NotEmpty().WithMessage("Upstream URL is required.")
            .Must(url => Uri.IsWellFormedUriString(url, UriKind.Absolute))
            .WithMessage("Upstream URL must be a valid absolute URL.");

        this.RuleFor(route => route.HttpVerb)
            .NotEmpty().WithMessage("HTTP Verb is required.")
            .Must(verb => allowedVerbs.Contains(verb.ToUpperInvariant()))
            .WithMessage("HTTP Verb must be one of the following: GET, POST, PUT, DELETE, PATCH.");
    }
}