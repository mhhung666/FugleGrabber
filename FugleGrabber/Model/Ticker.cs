using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FugleGrabber.Model;

public class Ticker
{
    [Key]
    [Column("symbol")]
    public required string Symbol { get; set; }

    [Column("name")]
    public string? Name { get; set; }

    [Column("type")]
    public string? Type { get; set; }

    [Column("exchange")]
    public string? Exchange { get; set; }
    
    [Column("market")]
    public string? Market { get; set; }

    [Column("update_time")]
    public DateTime? UpdateTime { get; set; }
}