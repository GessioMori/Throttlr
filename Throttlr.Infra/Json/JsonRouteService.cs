using System.Text.Json;
using Throttlr.Core.Entities;
using Throttlr.Core.Interfaces;
using Throttlr.Core.Utils;

namespace Throttlr.Infra.Json;
public class JsonRouteService : IRouteService
{
    private const string _filePath = "routes.json";
    private readonly JsonSerializerOptions _jsonSerializerOptions;
    private readonly List<RouteConfig> _routes;

    public JsonRouteService(IEnvironmentService environmentService)
    {
        string filePath = Path.Combine(environmentService.ContentRootPath, _filePath);

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"The routes file '{_filePath}' does not exist at path '{filePath}'.");
        }

        string jsonContent = File.ReadAllText(filePath);

        this._jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };

        this._jsonSerializerOptions.Converters.Add(new HttpMethodJsonConverter());

        this._routes = JsonSerializer.Deserialize<List<RouteConfig>>(jsonContent, this._jsonSerializerOptions)
                       ?? throw new InvalidOperationException($"Failed to deserialize routes from '{_filePath}'.");
    }

    public RouteConfig? GetRoute(string path, HttpMethod method)
    {
        return this._routes.FirstOrDefault(r =>
            r.Path.Equals(path, StringComparison.OrdinalIgnoreCase) &&
            r.Method.Equals(method));
    }
}