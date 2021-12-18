using Microsoft.EntityFrameworkCore;
using eBroker.Data;
using Microsoft.EntityFrameworkCore.Infrastructure;
using eBroker.Data.Entities;
using Xunit;
using eBroker.Data.Repositories;

namespace eBroker.Data.Tests
{
    public class RepositoryTest
    {
        private readonly DbContextOptions _options;
        public RepositoryTest()
        {
            var builder = new DbContextOptionsBuilder<EBrokerDbContext>();
            builder.UseInMemoryDatabase(databaseName: "EBroker");
            _options = builder.Options;
            using (var context = new EBrokerDbContext(_options))
            {

                context.Equities.Add(new Equity { Id = 1, Name = "Axis Midcap", NAV = 76.68f, });
                context.Equities.Add(new Equity { Id = 2, Name = "Mirae Asset", NAV = 33.45f });
                context.Equities.Add(new Equity { Id = 3, Name = "DSP Tax Saver Fund", NAV = 84.71f });

                context.Traders.Add(new Trader { Id = 1, Name = "ABC Trader", FundValue = 1000 });

                context.OwnedEquities.Add(new OwnedEquity { EquityId = 1, TraderId = 1, Units = 10 , Value = 766.88m });

                context.SaveChanges();
            }
        }

        [Fact]
        public void CheckIfEquityFetchedCorrectly_True()
        {
            using (var context = new EBrokerDbContext(_options))
            {
                var repository = new Repository<Equity>(context);

                var equity = repository.GetAsync(1);
                Assert.False(equity.Result == null);
                Assert.True(equity.Result?.Name == "Axis Midcap");
            }
        }

        [Fact]
        public void CheckIfFundAddedCorrectly_True()
        {
            using (var context = new EBrokerDbContext(_options))
            {
                var repository = new Repository<Trader>(context);

                var trader = context.Traders.Find(1);
                Assert.False(trader == null);
                if (trader == null)
                    return;

                trader.FundValue = trader.FundValue + 1000;

                repository.UpdateAsync(trader).Wait();

                var updatedTrader =  context.Traders.Find(1);
                Assert.False(updatedTrader == null);

                Assert.True(updatedTrader?.FundValue == 2000);
            }
        }


        [Fact]
        public void CheckIfFetchOwnedEquityCorrectly_True()
        {
            using (var context = new EBrokerDbContext(_options))
            {
                var repository = new Repository<OwnedEquity>(context);

                var ownedEquity = repository.Find(e=> e.TraderId == 1 && e.EquityId == 1).FirstOrDefaultAsync();

                Assert.True(ownedEquity?.Result != null);

                Assert.True(ownedEquity?.Result?.Units == 10);
            }
        }

        [Fact]
        public void CheckIfFetchOwnedEquity_False()
        {
            using (var context = new EBrokerDbContext(_options))
            {
                var repository = new Repository<OwnedEquity>(context);

                var ownedEquity = repository.Find(e => e.TraderId == 1 && e.EquityId == 2).FirstOrDefaultAsync();

                Assert.True(ownedEquity?.Result == null);
            }
        }

    }
}
