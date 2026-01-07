using Microsoft.Extensions.Configuration;
using System.Reflection;
using System.Text.Json;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;
using Goldiran.VOIPPanel.HostedService.BackGroundService.IServices;
using Goldiran.VOIPPanel.HostedService.BackGroundService.Models.Response;
using Goldiran.VOIPPanel.HostedService.BackGroundService.Models.Request;
using System.Text;

namespace Goldiran.VOIPPanel.HostedService.BackGroundService.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<TokenService> _logger;

    public TokenService(IConfiguration configuration, ILogger<TokenService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }
    public async Task<GetTokenResponse> GetToken()
    {
        var baseUrl = _configuration.GetSection("HostedServiceSettings").GetValue<string>("TokenBaseUrl");
        var apiUrl = _configuration.GetSection("HostedServiceSettings").GetValue<string>("TokenApi");
        var userName = _configuration.GetSection("HostedServiceSettings").GetValue<string>("UserName");
        var pass = _configuration.GetSection("HostedServiceSettings").GetValue<string>("Pass");
        var positionId = _configuration.GetSection("HostedServiceSettings").GetValue<long>("PositionId");


        var tokenRequest =new GetTokenRequest() { UserName=userName,Password=pass,PositionId=positionId };

        using var client = new HttpClient();
        client.BaseAddress = new Uri(baseUrl);

        var json =JsonConvert.SerializeObject(tokenRequest);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Send POST request
        HttpResponseMessage response = await client.PostAsync(apiUrl, content);

        if (response.IsSuccessStatusCode)
        {
            var responseStream = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<GetTokenResponse>(responseStream);
            return result;
        }
        else
        {
            _logger.LogError("عدم دسترسی ورود");
            throw new UnauthorizedAccessException("عدم دسترسی");
        }

    }
}
