namespace eBroker.Data.Entities
{
    public class Equity : AuditableEntity
    {
        public Equity()
        {
            OwnedEquities = new HashSet<OwnedEquity>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public float NAV { get; set; }

        public ICollection<OwnedEquity> OwnedEquities { get; private set; }

    }
}
