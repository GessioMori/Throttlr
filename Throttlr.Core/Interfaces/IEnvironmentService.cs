namespace Throttlr.Core.Interfaces;
public interface IEnvironmentService
{
    string ContentRootPath { get; }
    string? EnvironmentName { get; }
}