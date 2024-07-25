using Newtonsoft.Json;

namespace FugleGrabber.Model;

public class ApiResponse
{
    [JsonProperty("date")]
    public string Date { get; set; }
    [JsonProperty("type")]
    public string Type { get; set; }
    [JsonProperty("exchange")]
    public string Exchange { get; set; }
    [JsonProperty("data")]
    public List<TickerData> Data { get; set; }
}

public class TickerData
{
    [JsonProperty("symbol")]
    public string Symbol { get; set; }
    [JsonProperty("name")]
    public string Name { get; set; }
}