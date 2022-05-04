using HumanDaoAPI.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace HumanDaoAPI.Services;

public interface ICMarketCapService
{
    public Task<string> GetTokenValue();
    public Task<string> GetDilutedMarketCap();
    public Task<string> GetMarketCap();

}

public class CMarketCapService : ICMarketCapService
{
    private readonly IConfiguration _config;

    public CMarketCapService(IConfiguration config)
    {
        _config = config;
    }
    public async Task<string> GetDilutedMarketCap()
    {
        var options = new RestClientOptions(_config.GetValue<string>("CoinMarketCap:CmcUrl")+"/v2/cryptocurrency/quotes/latest");
        var client = new RestClient(options);
        var request = new RestRequest();
        request.AddHeader("X-CMC_PRO_API_KEY", _config.GetValue<string>("CoinMarketCap:APIKEY"));
        request.AddHeader("Accepts", "application/json");
        request.AddQueryParameter("id", _config.GetValue<string>("CoinMarketCap:Id"));

        var response = await client.GetAsync(request);

        var responseJson = JObject.Parse(response.Content);

        var dilutedMC = responseJson["data"]["15492"]["quote"]["USD"]["fully_diluted_market_cap"].ToString();

        return dilutedMC;

    }

    public async Task<string> GetMarketCap()
    {
        var options = new RestClientOptions(_config.GetValue<string>("CoinMarketCap:CmcUrl") + "/v2/cryptocurrency/quotes/latest");
        var client = new RestClient(options);
        var request = new RestRequest();
        request.AddHeader("X-CMC_PRO_API_KEY", _config.GetValue<string>("CoinMarketCap:APIKEY"));
        request.AddHeader("Accepts", "application/json");
        request.AddQueryParameter("id", _config.GetValue<string>("CoinMarketCap:Id"));

        var response = await client.GetAsync(request);

        var responseJson = JObject.Parse(response.Content);

        var dilutedMC = responseJson["data"]["15492"]["self_reported_market_cap"].ToString();

        return dilutedMC;
    }

    public async Task<string> GetTokenValue()
    {
        var options = new RestClientOptions(_config.GetValue<string>("CoinMarketCap:CmcUrl") + "/v2/cryptocurrency/quotes/latest");
        var client = new RestClient(options);
        var request = new RestRequest();
        request.AddHeader("X-CMC_PRO_API_KEY", _config.GetValue<string>("CoinMarketCap:APIKEY"));
        request.AddHeader("Accepts", "application/json");
        request.AddQueryParameter("id", _config.GetValue<string>("CoinMarketCap:Id"));

        var response = await client.GetAsync(request);

        var responseJson = JObject.Parse(response.Content);

        var dilutedMC = responseJson["data"]["15492"]["quote"]["USD"]["price"].ToString();

        return dilutedMC;
    }

}