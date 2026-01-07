using Goldiran.VOIPPanel.HostedService.BackGroundService.IServices;
using Goldiran.VOIPPanel.HostedService.BackGroundService.Models.Request;
using Goldiran.VOIPPanel.HostedService.BackGroundService.Models.Response;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;

namespace Goldiran.VOIPPanel.HostedService.BackGroundService.Services;

public class ContactDetailsService : IContactDetailsService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<FlatDataJobService> _logger;
    private readonly ITokenService _tokenService;
    public ContactDetailsService(IConfiguration configuration, ILogger<FlatDataJobService> logger, ITokenService tokenService)
    {
        _configuration = configuration;
        _logger = logger;
        _tokenService = tokenService;
    }
    public async Task<CreateFlatContactDetailListJobResponse> CreateFlatContactDetailJob(CreateFlatContactDetailListJobRequest request, string token)
    {
        var baseUrl = _configuration.GetSection("HostedServiceSettings").GetValue<string>("FlatBaseUrl");

        string apiUrl = string.Empty;
        if (request.ContactReportType == Enums.ContactReportType.Detail)
            apiUrl = _configuration.GetSection("HostedServiceSettings").GetValue<string>("PostContactDetailApi"); 
        else
            apiUrl = _configuration.GetSection("HostedServiceSettings").GetValue<string>("PostAutoDialApi");

        var tokenResponse = await _tokenService.GetToken();

        using var client = new HttpClient();
        client.Timeout = TimeSpan.FromMinutes(10);
        client.BaseAddress = new Uri(baseUrl);

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.AccessToken);
        var json = JsonConvert.SerializeObject(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Send POST request
        HttpResponseMessage response = await client.PostAsync(apiUrl, content);

        if (response.IsSuccessStatusCode)
        {
            var responseStream = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<CreateFlatContactDetailListJobResponse>(responseStream);
            return result;
        }
        else
        {
            _logger.LogError("مشکل در بازیابی اطلاعات");
            throw new Exception("مشکل در بازیابی اطلاعات");
        }
    }

   
}
