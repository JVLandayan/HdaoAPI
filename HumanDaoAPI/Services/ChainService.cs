using HumanDaoAPI.Models;
using Microsoft.OpenApi.Models;
using RestSharp;
using RestSharp.Authenticators;

namespace HumanDaoAPI.Services;



public interface IChainService
{
    public Task<string> GetCirculatingSupply();

}

public class ChainService : IChainService

{
    private readonly IConfiguration _config;
    private const string ContractAddressEth = "0xdaC657ffD44a3B9d8aba8749830Bf14BEB66fF2D";
    private const string ContractAddressPoly = "0x72928d5436Ff65e57F72D5566dCd3BaEDC649A88";
    private const decimal MaxSupply = 1000000000;
    List<string> addressesFromEth = new()
    {
        "0xc63bd01cfb9ce9837424f96c969e5435a9b9a46d",
        "0x6c48814701c98f0d24b1b891fac254a817aadfdf",
        "0x639752551071049d77766c69416591663a1F8211",
        "0xFf29b08b3a62a35726749Aa9EeAA3E3F75Edfc81",
        "0x8F8c1fe08af8D85E8D711Af1930c31D116FE4a07"
    };

    List<string> addressesFromPolygon = new()
    {
        "0x08C724340c1438fe5e20b84BA9CaC89a20144414",
        "0x2228C903fcE2EEB66e03e11776eAA354aece544c",
        "0xc86b27D0f67871eFaC0630f7fEab71446591352b",
        "0x7A6f415c4ebae0D0b5E4A3A67b9949E40b664331"
    };

    public ChainService(IConfiguration config)
    {
        _config = config;
    }

    public async Task<string> GetCirculatingSupply()
    {
        var tokenValues = await GetValuesForEachAddresses(addressesFromEth, addressesFromPolygon);
        var CirculatingValueFromEth = await ComputeCirculatingSupply(tokenValues);
        return CirculatingValueFromEth;
    }



    public async Task<List<string>> GetValuesForEachAddresses(List<string> addressListEth, List<string> addressListPoly)
    {
        List<string> valuesList = new List<string>();
        foreach (var address in addressListEth)
        {
            valuesList.Add(await GetValueFromEtherscan(address));
        }

        foreach (var address in addressListPoly)
        {
            valuesList.Add(await GetValueFromPolygonScan(address));
        }

        return valuesList;
    }


    public async Task<string> GetValueFromEtherscan(string walletAddress)
    {
        var options = new RestClientOptions(_config.GetValue<string>("BlockChain:Ethereum:EthScanUrl"));
        var client = new RestClient(options);
        var request = new RestRequest();

        request.AddQueryParameter("module", "account");
        request.AddQueryParameter("action", "tokenbalance");
        request.AddQueryParameter("contractaddress", ContractAddressEth);
        request.AddQueryParameter("address", walletAddress);
        request.AddQueryParameter("tag", "latest");
        request.AddQueryParameter("apikey", _config.GetValue<string>("BlockChain:Ethereum:APIKEY"));

        var response = await client.GetAsync<ResponseDto>(request);

        return response.result;
    }

    public async Task<string> GetValueFromPolygonScan(string walletAddress)
    {
        var options = new RestClientOptions(_config.GetValue<string>("BlockChain:Polygon:PolyScanUrl"));
        var client = new RestClient(options);
        var request = new RestRequest();

        request.AddQueryParameter("module", "account");
        request.AddQueryParameter("action", "tokenbalance");
        request.AddQueryParameter("contractaddress", ContractAddressPoly);
        request.AddQueryParameter("address", walletAddress);
        request.AddQueryParameter("tag", "latest");
        request.AddQueryParameter("apikey", _config.GetValue<string>("BlockChain:Polygon:APIKEY"));

        var response = await client.GetAsync<ResponseDto>(request);

        return response.result;
    }



    public async Task<string> ComputeCirculatingSupply(List<string> tokenQuantities)
    {
        decimal Maxsupplycopy = MaxSupply;

        foreach (var quantity in tokenQuantities)
        {
            var result = ConvertStringToDecimal(quantity);

            Maxsupplycopy -= result;
        }

        return Maxsupplycopy.ToString();
    }


    public decimal ConvertStringToDecimal(string value)
    {
        var decimalValue = value.Substring(value.Length - 18);
        var wholeValue = value.Substring(0, value.Length - 18);
        decimal result = decimal.Parse(wholeValue) + decimal.Parse("." + decimalValue);

        return result;
    }

}