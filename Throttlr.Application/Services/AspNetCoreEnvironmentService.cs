using Microsoft.AspNetCore.Hosting;
using Throttlr.Core.Interfaces;

namespace Throttlr.Application.Services;

public class AspNetCoreEnvironmentService : IEnvironmentService
{
    private readonly IWebHostEnvironment _webHostEnvironment;

    public AspNetCoreEnvironmentService(IWebHostEnvironment webHostEnvironment)
    {
        this._webHostEnvironment = webHostEnvironment ?? throw new ArgumentNullException(nameof(webHostEnvironment));
    }

    public string ContentRootPath => this._webHostEnvironment.ContentRootPath;

    public string? EnvironmentName => this._webHostEnvironment.EnvironmentName;
}