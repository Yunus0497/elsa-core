using Elsa.Identity.Models;

namespace Elsa.Identity.Endpoints.Applications.Create;

internal class Request
{
    public string Name { get; set; } = default!;
    public ICollection<string>? Roles { get; set; }
}

internal record Response(
    string Id, 
    string Name, 
    ICollection<string> Roles,
    string ClientId, 
    string ClientSecret, 
    string ApiKey, 
    HashedSecret HashedApiKey,
    HashedSecret HashedClientSecret
);