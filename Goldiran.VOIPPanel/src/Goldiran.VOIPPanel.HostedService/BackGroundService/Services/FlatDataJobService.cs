using Goldiran.VOIPPanel.HostedService.BackGroundService.IServices;
using Goldiran.VOIPPanel.HostedService.BackGroundService.Models.Request;
using Goldiran.VOIPPanel.HostedService.BackGroundService.Models.Response;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using System.Web;

namespace Goldiran.VOIPPanel.HostedService.BackGroundService.Services;

public class FlatDataJobService : IFlatDataJobService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<FlatDataJobService> _logger;
    private readonly ITokenService _tokenService;
    public FlatDataJobService(IConfiguration configuration, ILogger<FlatDataJobService> logger, ITokenService tokenService)
    {
        _configuration = configuration;
        _logger = logger;
        _tokenService = tokenService;
    }
    public async Task<long> CreateFlatDataJob(CreateFaltDataJobRequest request, string token)
    {
        var baseUrl = _configuration.GetSection("HostedServiceSettings").GetValue<string>("FlatBaseUrl");
        var apiUrl = _configuration.GetSection("HostedServiceSettings").GetValue<string>("PostLastFlatDataApi");
        var tokenResponse = await _tokenService.GetToken();

        using var client = new HttpClient();
        client.BaseAddress = new Uri(baseUrl);

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.AccessToken);
        var json = JsonConvert.SerializeObject(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Send POST request
        HttpResponseMessage response = await client.PostAsync(apiUrl, content);

        if (response.IsSuccessStatusCode)
        {
            var responseStream = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<long>(responseStream);
            return result;
        }
        else
        {
            _logger.LogError("مشکل در بازیابی اطلاعات");
            throw new Exception("مشکل در بازیابی اطلاعات");
        }
    }

    public async Task<GetFlatDataJobLastResponse> GetFlatDataJobLast(GetFlatDataJobLastRequest request, string token)
    {
        var baseUrl = _configuration.GetSection("HostedServiceSettings").GetValue<string>("FlatBaseUrl");
        var apiUrl = _configuration.GetSection("HostedServiceSettings").GetValue<string>("GetLastFlatData");

        using var client = new HttpClient();
        client.BaseAddress = new Uri(baseUrl);


        var query = HttpUtility.ParseQueryString(string.Empty);
        query["ReportType"] = ((int)request.ReportType).ToString();

        var endpoint = $"/{apiUrl}?{query}";
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await client.GetAsync(endpoint);

        if (response.IsSuccessStatusCode)
        {
            var responseStream = await response.Content.ReadAsStringAsync();
            //var result = JsonConvert.DeserializeObject<PaginatedList<FetchFraudPersonsResponse>>(responseStream);
            var result = JsonConvert.DeserializeObject<GetFlatDataJobLastResponse>(responseStream);
            return result;
        }
        else
        {
            _logger.LogError("مشکل در بازیابی اطلاعات");
            throw new Exception("مشکل در بازیابی اطلاعات");
        }

    }
}
