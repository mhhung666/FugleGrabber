using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FugleGrabber.Model;

 [Table("tickerInfo")]
    public class TickerInfo
    {
        [Key]
        [Column("symbol")]
        public string Symbol { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("type")]
        public string Type { get; set; }

        [Column("exchange")]
        public string Exchange { get; set; }

        [Column("market")]
        public string Market { get; set; }

        [Column("industry")]
        public string Industry { get; set; }

        [Column("security_type")]
        public string SecurityType { get; set; }

        [Column("previous_close")]
        public decimal? PreviousClose { get; set; }

        [Column("reference_price")]
        public decimal? ReferencePrice { get; set; }

        [Column("limit_up_price")]
        public decimal? LimitUpPrice { get; set; }

        [Column("limit_down_price")]
        public decimal? LimitDownPrice { get; set; }

        [Column("can_day_trade")]
        public bool? CanDayTrade { get; set; }

        [Column("can_buy_day_trade")]
        public bool? CanBuyDayTrade { get; set; }

        [Column("can_below_flat_margin_short_sell")]
        public bool? CanBelowFlatMarginShortSell { get; set; }

        [Column("can_below_flat_sbl_short_sell")]
        public bool? CanBelowFlatSblShortSell { get; set; }

        [Column("is_attention")]
        public bool? IsAttention { get; set; }

        [Column("is_disposition")]
        public bool? IsDisposition { get; set; }

        [Column("is_unusually_recommended")]
        public bool? IsUnusuallyRecommended { get; set; }

        [Column("is_specific_abnormally")]
        public bool? IsSpecificAbnormally { get; set; }

        [Column("matching_interval")]
        public int? MatchingInterval { get; set; }

        [Column("security_status")]
        public string SecurityStatus { get; set; }

        [Column("board_lot")]
        public int? BoardLot { get; set; }

        [Column("trading_currency")]
        public string TradingCurrency { get; set; }

        [Column("update_time")]
        public DateTime? UpdateTime { get; set; }
    }