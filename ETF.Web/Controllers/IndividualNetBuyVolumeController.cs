using EtfAnalyzer.Entities.Entities;
using EtfAnalyzer.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace EtfAnalyzer.Web.Controllers
{
    public class IndividualNetBuyVolumeController : Controller
    {
        public readonly IHttpClientFactory _clientFactory;

        public IndividualNetBuyVolumeController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<IActionResult> Index()
        {
            List<IndiInstiTradingDataViewModel> modelList = new();
            Regex pattern = new("^.*\\d$");
            HttpClient client = _clientFactory.CreateClient(name: "CapitalMarketDataWebApi");

            HttpRequestMessage etfListRequest = new(HttpMethod.Get, $"api/v1/Instrument/Etfs");
            HttpResponseMessage etfListResponse = await client.SendAsync(etfListRequest);
            var etfs = await etfListResponse.Content.ReadFromJsonAsync<List<Entities.Entities.ETF>>();
            if (etfs is null) return NotFound();

            foreach (var etf in etfs)
            {
                if (!pattern.IsMatch(etf.Ticker) && etf.Type == "Etf" && etf.Subsector == "FixedIncomeEtf")
                {
                    HttpRequestMessage instiIndiRequest = new(HttpMethod.Get, $"api/v1/IndividualInstitutionalTradingData/{etf.Id}");
                    HttpResponseMessage instiIndiResponse = await client.SendAsync(instiIndiRequest);
                    var data = await instiIndiResponse.Content.ReadFromJsonAsync<IndiInstiTradingData>();
                    if (data is not null)
                    {
                        IndiInstiTradingDataViewModel model = new()
                        {
                            Ticker = etf.Ticker,
                            Date = DateTime.Now,
                            NetVolume = data.IndividualTradingVolume_BuySide - data.IndividualTradingVolume_SellSide,
                        };
                        modelList.Add(model);
                    }
                }
            }

            return View(modelList);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}