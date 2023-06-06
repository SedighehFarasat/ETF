namespace EtfAnalyzer.Entities.Entities;

public class IndiInstiTradingData
{
    public string? InstrumentId { get; set; }
    public DateTime Date { get; private set; }
    public int? IndividualNumberOfTrades_BuySide { get; set; }
    public int? IndividualNumberOfTrades_SellSide { get; set; }
    public long? IndividualTradingVolume_BuySide { get; set; }
    public long? IndividualTradingVolume_SellSide { get; set; }
    public int? InstitutionalNumberOfTrades_BuySide { get; set; }
    public int? InstitutionalNumberOfTrades_SellSide { get; set; }
    public long? InstitutionalTradingVolume_BuySide { get; set; }
    public long? InstitutionalTradingVolume_SellSide { get; set; }
}
