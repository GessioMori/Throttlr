using AutoMapper;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Throttlr.Api.Validators;
using Throttlr.Core.DTOs;
using Throttlr.Core.Entities;
using Throttlr.Core.Interfaces;
using Throttlr.Shared.OperationResult;

namespace Throttlr.Api.Controllers;
[Route("api/routes")]
[ApiController]
public class RouteManagementController : ControllerBase
{
    private readonly IRouteManagerService routeManagerService;
    private readonly IMapper mapper;
    private readonly IValidatorService validatorService;

    public RouteManagementController(IRouteManagerService routeManagerService, IMapper mapper,
        IValidatorService validatorService)
    {
        this.routeManagerService = routeManagerService;
        this.mapper = mapper;
        this.validatorService = validatorService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        (bool isSuccess, string errorMessage) = this.IsIdValid(id);

        if (!isSuccess)
        {
            return this.BadRequest(errorMessage);
        }

        OperationResult<RouteConfig> route = await this.routeManagerService.GetByIdAsync(id);

        if (!route.Success || route.Data is null)
        {
            return this.NotFound(route.Message);
        }

        return this.Ok(this.mapper.Map<RouteConfigReadDto>(route.Data));
    }

    [HttpPost]
    public async Task<IActionResult> Create(RouteConfigDto route)
    {
        if (route is null)
        {
            return this.BadRequest("Route configuration cannot be null.");
        }

        (bool isSuccess, string errorMessage) = this.IsRouteValid(route);

        if (!isSuccess)
        {
            return this.BadRequest(errorMessage);
        }

        RouteConfig routeEntity = this.mapper.Map<RouteConfig>(route);

        OperationResult<string> createResult = await this.routeManagerService.CreateAsync(routeEntity);

        return createResult.Success ?
            this.CreatedAtAction(nameof(Get), new { id = createResult.Data }, route) :
            this.BadRequest(createResult.Message);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, RouteConfigDto route)
    {
        (bool isSuccess, string errorMessage) = this.IsIdValid(id);

        if (!isSuccess)
        {
            return this.BadRequest(errorMessage);
        }

        (isSuccess, errorMessage) = this.IsRouteValid(route);

        if (!isSuccess)
        {
            return this.BadRequest(errorMessage);
        }

        if (route is null)
        {
            return this.BadRequest("Route configuration cannot be null.");
        }

        RouteConfig routeEntity = this.mapper.Map<RouteConfig>(route);
        routeEntity.Id = id;

        OperationResult updateResult = await this.routeManagerService.UpdateAsync(id, routeEntity);
        return updateResult.Success ? this.NoContent() : this.NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        (bool isSuccess, string errorMessage) = this.IsIdValid(id);

        if (!isSuccess)
        {
            return this.BadRequest(errorMessage);
        }

        OperationResult deleteResult = await this.routeManagerService.DeleteAsync(id);
        return deleteResult.Success ? this.NoContent() : this.NotFound();
    }

    private (bool isSuccess, string errorMessage) IsIdValid(string id)
    {
        ValidationResult validationResult = this.validatorService.ValidateId(id);

        if (!validationResult.IsValid)
        {
            string errorMessage = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            return (false, errorMessage);
        }

        return (true, string.Empty);
    }

    private (bool isSuccess, string errorMessage) IsRouteValid(RouteConfigDto route)
    {
        ValidationResult validationResult = this.validatorService.ValidateRouteConfigDto(route);

        if (!validationResult.IsValid)
        {
            string errorMessage = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            return (false, errorMessage);
        }
        return (true, string.Empty);
    }
}