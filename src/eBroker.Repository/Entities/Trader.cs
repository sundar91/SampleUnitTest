namespace eBroker.Data.Entities
{
    public class Trader : AuditableEntity
    {
        public Trader()
        {
            OwnedEquities = new HashSet<OwnedEquity>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal FundValue { get; set; }

        public ICollection<OwnedEquity> OwnedEquities { get; private set; }

    }
}
