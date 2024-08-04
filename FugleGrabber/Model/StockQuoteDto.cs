namespace FugleGrabber.Model;

public class StockQuoteDto
{
    public DateTime Date { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Exchange { get; set; } = string.Empty;
    public string Market { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public decimal ReferencePrice { get; set; }
    public decimal PreviousClose { get; set; }
    public decimal OpenPrice { get; set; }
    public long OpenTime { get; set; }
    public decimal HighPrice { get; set; }
    public long HighTime { get; set; }
    public decimal LowPrice { get; set; }
    public long LowTime { get; set; }
    public decimal ClosePrice { get; set; }
    public long CloseTime { get; set; }
    public decimal AvgPrice { get; set; }
    public decimal Change { get; set; }
    public decimal ChangePercent { get; set; }
    public decimal Amplitude { get; set; }
    public decimal LastPrice { get; set; }
    public int LastSize { get; set; }
    public List<Bid> Bids { get; set; } = new();
    public List<Ask> Asks { get; set; } = new();
    public Total Total { get; set; } = new();
    public LastTrade LastTrade { get; set; } = new();
    public LastTrial LastTrial { get; set; } = new();
    public bool IsClose { get; set; }
    public long Serial { get; set; }
    public long LastUpdated { get; set; }
}

public class StockQuote
{
    public string Symbol { get; set; }
    public decimal ReferencePrice { get; set; }
    public decimal PreviousClose { get; set; }
    public decimal OpenPrice { get; set; }
    public long OpenTime { get; set; }
    public decimal HighPrice { get; set; }
    public long HighTime { get; set; }
    public decimal LowPrice { get; set; }
    public long LowTime { get; set; }
    public decimal ClosePrice { get; set; }
    public long CloseTime { get; set; }
    public decimal AvgPrice { get; set; }
    public decimal PriceChange { get; set; }
    public decimal ChangePercent { get; set; }
    public decimal Amplitude { get; set; }
    public decimal LastPrice { get; set; }
    public int LastSize { get; set; }
    public bool IsClose { get; set; }
    public long LastUpdated { get; set; }
}

public class Bid
{
    public decimal Price { get; set; }
    public int Size { get; set; }
}

public class Ask
{
    public decimal Price { get; set; }
    public int Size { get; set; }
}

public class Total
{
    public long TradeValue { get; set; }
    public int TradeVolume { get; set; }
    public int TradeVolumeAtBid { get; set; }
    public int TradeVolumeAtAsk { get; set; }
    public int Transaction { get; set; }
    public long Time { get; set; }
}

public class LastTrade
{
    public decimal Bid { get; set; }
    public decimal Ask { get; set; }
    public decimal Price { get; set; }
    public int Size { get; set; }
    public long Time { get; set; }
    public long Serial { get; set; }
}

public class LastTrial
{
    public decimal Bid { get; set; }
    public decimal Ask { get; set; }
    public decimal Price { get; set; }
    public int Size { get; set; }
    public long Time { get; set; }
    public long Serial { get; set; }
}