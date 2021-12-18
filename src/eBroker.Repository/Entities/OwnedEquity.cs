namespace eBroker.Data.Entities
{
    public class OwnedEquity : AuditableEntity
    {
        public int EquityId { get; set; }

        public int TraderId { get; set; }

        public int Units { get; set; } 

        public decimal Value { get; set; }

        public Equity Equity { get; set; }

        public Trader Trader { get; set; }

    }
}
