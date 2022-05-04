using HumanDaoAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace HumanDaoAPI.Controllers
{
    [ApiController]
    [Route("api")]
    public class BaseController : ControllerBase
    {
        private readonly IChainService _chainService;
        private readonly ICMarketCapService _cMarketCapService;

        public BaseController(IChainService chainService, ICMarketCapService cMarketCapService)
        {
            _chainService = chainService;
            _cMarketCapService = cMarketCapService;
        }

        [HttpGet("circulating-supply")]
        public Task<string> GetCirculatingSupply()
        {
            var result = _chainService.GetCirculatingSupply();
            return result;
        }

        [HttpGet("token-value")]
        public Task<string> GetTokenValue()
        {
            var response = _cMarketCapService.GetTokenValue();
            return response;
        }

        [HttpGet("market-cap")]
        public Task<string> GetMarketCap()
        {
            var response = _cMarketCapService.GetMarketCap();
            return response;
        }

        [HttpGet("diluted-market-cap")]
        public Task<string> GetDilutedMarketCap()
        {
            var response = _cMarketCapService.GetDilutedMarketCap();
            return response;
        }









    }
}